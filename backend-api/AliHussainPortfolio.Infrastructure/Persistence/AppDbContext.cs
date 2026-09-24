using AliHussainPortfolio.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AliHussainPortfolio.Infrastructure.Persistence;

public class AppDbContext : IdentityDbContext<User, IdentityRole<int>, int>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Course> Courses => Set<Course>();
    public DbSet<CourseResource> CourseResources => Set<CourseResource>();
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<ProjectTechnology> ProjectTechnologies => Set<ProjectTechnology>();
    public DbSet<ProjectImage> ProjectImages => Set<ProjectImage>();
    public DbSet<Service> Services => Set<Service>();
    public DbSet<ContactMessage> ContactMessages => Set<ContactMessage>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("Users");
            entity.Property(x => x.Name).IsRequired().HasMaxLength(150);
            entity.Property(x => x.Email).HasMaxLength(256);
            entity.HasIndex(x => x.Email).IsUnique();
            entity.Property(x => x.Role).IsRequired().HasMaxLength(100);
            entity.Property(x => x.IsActive).HasDefaultValue(true);
        });

        modelBuilder.Entity<IdentityRole<int>>(entity => entity.ToTable("Roles"));
        modelBuilder.Entity<IdentityUserRole<int>>(entity => entity.ToTable("UserRoles"));
        modelBuilder.Entity<IdentityUserClaim<int>>(entity => entity.ToTable("UserClaims"));
        modelBuilder.Entity<IdentityUserLogin<int>>(entity => entity.ToTable("UserLogins"));
        modelBuilder.Entity<IdentityUserToken<int>>(entity => entity.ToTable("UserTokens"));
        modelBuilder.Entity<IdentityRoleClaim<int>>(entity => entity.ToTable("RoleClaims"));

        modelBuilder.Entity<Course>(entity =>
        {
            entity.Property(x => x.Title).IsRequired().HasMaxLength(200);
            entity.Property(x => x.Slug).IsRequired().HasMaxLength(200);
            entity.HasIndex(x => x.Slug).IsUnique();
            entity.Property(x => x.Category).IsRequired().HasMaxLength(100);
            entity.Property(x => x.Author).IsRequired().HasMaxLength(150);
            entity.HasMany(x => x.Resources)
                .WithOne(x => x.Course)
                .HasForeignKey(x => x.CourseId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<CourseResource>(entity =>
        {
            entity.Property(x => x.Title).IsRequired().HasMaxLength(250);
            entity.Property(x => x.FileName).IsRequired().HasMaxLength(300);
            entity.Property(x => x.StorageKey).IsRequired().HasMaxLength(500);
            entity.HasIndex(x => x.StorageKey).IsUnique();
            entity.Property(x => x.ContentType).IsRequired().HasMaxLength(150);
        });

        modelBuilder.Entity<Project>(entity =>
        {
            entity.Property(x => x.Title).IsRequired().HasMaxLength(200);
            entity.Property(x => x.Slug).IsRequired().HasMaxLength(200);
            entity.HasIndex(x => x.Slug).IsUnique();
            entity.Property(x => x.ShortDescription).IsRequired().HasMaxLength(300);
            entity.Property(x => x.Category).IsRequired().HasMaxLength(100);
            entity.HasMany(x => x.Technologies)
                .WithOne(x => x.Project)
                .HasForeignKey(x => x.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasMany(x => x.Images)
                .WithOne(x => x.Project)
                .HasForeignKey(x => x.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<ProjectTechnology>(entity =>
        {
            entity.Property(x => x.TechnologyName).IsRequired().HasMaxLength(100);
        });

        modelBuilder.Entity<ProjectImage>(entity =>
        {
            entity.Property(x => x.ImageUrl).IsRequired().HasMaxLength(500);
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.Property(x => x.Title).IsRequired().HasMaxLength(150);
            entity.Property(x => x.Description).IsRequired().HasMaxLength(500);
        });

        modelBuilder.Entity<ContactMessage>(entity =>
        {
            entity.Property(x => x.Name).IsRequired().HasMaxLength(150);
            entity.Property(x => x.Email).IsRequired().HasMaxLength(256);
            entity.Property(x => x.Message).IsRequired().HasMaxLength(2000);
        });
    }
}