using System.Text;
using AliHussainPortfolio.Application.Interfaces;
using AliHussainPortfolio.Domain.Entities;
using AliHussainPortfolio.Infrastructure.Persistence;
using AliHussainPortfolio.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? "Host=localhost;Port=5432;Database=AliHussainPortfolioDb;Username=postgres;Password=postgresres";

var jwtSecret = builder.Configuration["JwtSettings:SecretKey"]
    ?? Environment.GetEnvironmentVariable("JWT_SECRET")
    ?? "this-is-a-very-long-development-secret-key-for-local-dev";
var jwtIssuer = builder.Configuration["JwtSettings:Issuer"] ?? "AliHussainPortfolio";
var jwtAudience = builder.Configuration["JwtSettings:Audience"] ?? "AliHussainPortfolioClient";
var migrationConnectionString = builder.Configuration.GetConnectionString("MigrationConnection");

if (builder.Environment.IsProduction())
{
    var configuredConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    var configuredMigrationConnectionString = builder.Configuration.GetConnectionString("MigrationConnection");
    var databaseHost = configuredConnectionString is null
        ? null
        : new Npgsql.NpgsqlConnectionStringBuilder(configuredConnectionString).Host;
    var migrationDatabaseHost = configuredMigrationConnectionString is null
        ? null
        : new Npgsql.NpgsqlConnectionStringBuilder(configuredMigrationConnectionString).Host;
    var corsOrigins = builder.Configuration.GetSection("Cors:Origins").Get<string[]>() ?? [];

    if (string.IsNullOrWhiteSpace(configuredConnectionString)
        || string.IsNullOrWhiteSpace(databaseHost)
        || databaseHost.Equals("localhost", StringComparison.OrdinalIgnoreCase)
        || databaseHost.Equals("127.0.0.1", StringComparison.OrdinalIgnoreCase))
    {
        throw new InvalidOperationException("Production requires a non-local PostgreSQL connection string.");
    }

    if (string.IsNullOrWhiteSpace(migrationDatabaseHost)
        || migrationDatabaseHost.Equals("localhost", StringComparison.OrdinalIgnoreCase)
        || migrationDatabaseHost.Equals("127.0.0.1", StringComparison.OrdinalIgnoreCase)
        || migrationDatabaseHost.Contains("-pooler", StringComparison.OrdinalIgnoreCase))
    {
        throw new InvalidOperationException("Production requires a direct, non-pooled PostgreSQL MigrationConnection.");
    }

    if (string.IsNullOrWhiteSpace(builder.Configuration["JwtSettings:SecretKey"])
        && string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("JWT_SECRET")))
    {
        throw new InvalidOperationException("Production requires JwtSettings:SecretKey or JWT_SECRET.");
    }

    if (Encoding.UTF8.GetByteCount(jwtSecret) < 32)
    {
        throw new InvalidOperationException("The production JWT signing key must be at least 32 bytes.");
    }

    if (string.IsNullOrWhiteSpace(builder.Configuration["Admin:Email"])
        || string.IsNullOrWhiteSpace(builder.Configuration["Admin:Password"]))
    {
        throw new InvalidOperationException("Production requires Admin:Email and Admin:Password.");
    }

    if (corsOrigins.Length == 0 || corsOrigins.Any(origin =>
            !Uri.TryCreate(origin, UriKind.Absolute, out var uri)
            || uri.Scheme != Uri.UriSchemeHttps
            || uri.AbsolutePath != "/"
            || !string.IsNullOrEmpty(uri.Query)
            || !string.IsNullOrEmpty(uri.Fragment)
            || !string.IsNullOrEmpty(uri.UserInfo)))
    {
        throw new InvalidOperationException("Production requires at least one HTTPS origin in Cors:Origins.");
    }
}

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddIdentity<User, IdentityRole<int>>(options =>
    {
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequiredLength = 8;
        options.User.RequireUniqueEmail = true;
    })
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
            ClockSkew = TimeSpan.FromMinutes(2),
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        var origins = builder.Configuration.GetSection("Cors:Origins").Get<string[]>()
            ?? new[] { "http://localhost:5173" };

        policy.WithOrigins(origins)
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IResourceService, ResourceService>();
builder.Services.AddScoped<IServiceCatalogService, ServiceCatalogService>();
builder.Services.AddScoped<IContactService, ContactService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddSingleton<IFileStorageService, LocalFileStorageService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    try
    {
        var migrationOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseNpgsql(migrationConnectionString ?? connectionString)
            .Options;
        await using var migrationDbContext = new AppDbContext(migrationOptions);
        await migrationDbContext.Database.MigrateAsync();

        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
        var adminEmail = builder.Configuration["Admin:Email"]
            ?? Environment.GetEnvironmentVariable("ADMIN_EMAIL")
            ?? "admin@alihussain.dev";
        var adminPassword = builder.Configuration["Admin:Password"]
            ?? Environment.GetEnvironmentVariable("ADMIN_PASSWORD")
            ?? "AliHussain@2025!";
        var existingAdmin = await userManager.FindByEmailAsync(adminEmail);

        if (existingAdmin is null)
        {
            var adminUser = new User
            {
                Name = "Ali Hussain Admin",
                UserName = adminEmail,
                Email = adminEmail,
                Role = "Admin",
                IsActive = true,
                EmailConfirmed = true,
                CreatedAt = DateTime.UtcNow,
            };

            var created = await userManager.CreateAsync(adminUser, adminPassword);
            if (!created.Succeeded)
            {
                throw new InvalidOperationException(string.Join("; ", created.Errors.Select(e => e.Description)));
            }
        }
    }
    catch (Exception ex) when (!app.Environment.IsProduction())
    {
        logger.LogWarning(ex, "Database was not available during startup; app will continue without migrations/seeding.");
    }
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCors("Frontend");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapGet("/health", async (AppDbContext dbContext, CancellationToken cancellationToken) =>
{
    var databaseAvailable = await dbContext.Database.CanConnectAsync(cancellationToken);
    return databaseAvailable
        ? Results.Ok(new { status = "ok" })
        : Results.StatusCode(StatusCodes.Status503ServiceUnavailable);
});

app.Run();
