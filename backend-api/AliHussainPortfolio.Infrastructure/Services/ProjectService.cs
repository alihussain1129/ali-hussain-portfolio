using AliHussainPortfolio.Application.DTOs;
using AliHussainPortfolio.Application.Interfaces;
using AliHussainPortfolio.Domain.Entities;
using AliHussainPortfolio.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AliHussainPortfolio.Infrastructure.Services;

public class ProjectService : IProjectService
{
    private readonly AppDbContext _dbContext;

    public ProjectService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<ProjectListItemDto>> GetPublishedProjectsAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Projects
            .AsNoTracking()
            .Include(p => p.Technologies)
            .Where(p => p.IsPublished)
            .OrderByDescending(p => p.UpdatedAt)
            .Select(p => new ProjectListItemDto
            {
                Id = p.Id,
                Title = p.Title,
                Slug = p.Slug,
                ShortDescription = p.ShortDescription,
                Category = p.Category,
                GitHubUrl = p.GitHubUrl,
                LiveDemoUrl = p.LiveDemoUrl,
                IsFeatured = p.IsFeatured,
                IsPublished = p.IsPublished,
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt,
                Technologies = p.Technologies.Select(t => t.TechnologyName).ToList(),
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<ProjectDetailDto?> GetProjectByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var project = await _dbContext.Projects
            .AsNoTracking()
            .Include(p => p.Technologies)
            .Include(p => p.Images)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

        return project is null ? null : MapDetail(project);
    }

    public async Task<ProjectDetailDto?> GetProjectBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(slug))
        {
            return null;
        }

        var project = await _dbContext.Projects
            .AsNoTracking()
            .Include(p => p.Technologies)
            .Include(p => p.Images)
            .FirstOrDefaultAsync(p => p.Slug == slug, cancellationToken);

        return project is null ? null : MapDetail(project);
    }

    public async Task<ProjectDetailDto> CreateProjectAsync(CreateProjectDto request, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.Title))
        {
            throw new InvalidOperationException("Project title is required.");
        }

        var entity = new Project
        {
            Title = request.Title.Trim(),
            Slug = CreateSlug(request.Title),
            ShortDescription = request.ShortDescription ?? string.Empty,
            Description = request.Description ?? string.Empty,
            Category = request.Category ?? string.Empty,
            GitHubUrl = request.GitHubUrl,
            LiveDemoUrl = request.LiveDemoUrl,
            IsFeatured = request.IsFeatured,
            IsPublished = request.IsPublished,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
        };

        entity.Technologies = (request.Technologies ?? new List<string>())
            .Where(t => !string.IsNullOrWhiteSpace(t))
            .Select((t, index) => new ProjectTechnology { TechnologyName = t.Trim(), Project = entity, Id = index + 1 })
            .ToList();

        entity.Images = (request.ImageUrls ?? new List<string>())
            .Where(url => !string.IsNullOrWhiteSpace(url))
            .Select((url, index) => new ProjectImage { ImageUrl = url.Trim(), DisplayOrder = index })
            .ToList();

        _dbContext.Projects.Add(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return MapDetail(entity);
    }

    public async Task<ProjectDetailDto?> UpdateProjectAsync(int id, UpdateProjectDto request, CancellationToken cancellationToken = default)
    {
        var entity = await _dbContext.Projects
            .Include(p => p.Technologies)
            .Include(p => p.Images)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

        if (entity is null)
        {
            return null;
        }

        if (request.Title is not null)
        {
            entity.Title = request.Title.Trim();
            entity.Slug = CreateSlug(entity.Title);
        }

        if (request.ShortDescription is not null) entity.ShortDescription = request.ShortDescription;
        if (request.Description is not null) entity.Description = request.Description;
        if (request.Category is not null) entity.Category = request.Category;
        if (request.GitHubUrl is not null) entity.GitHubUrl = request.GitHubUrl;
        if (request.LiveDemoUrl is not null) entity.LiveDemoUrl = request.LiveDemoUrl;
        if (request.IsFeatured.HasValue) entity.IsFeatured = request.IsFeatured.Value;
        if (request.IsPublished.HasValue) entity.IsPublished = request.IsPublished.Value;

        if (request.Technologies is not null)
        {
            _dbContext.ProjectTechnologies.RemoveRange(entity.Technologies);
            entity.Technologies = request.Technologies
                .Where(t => !string.IsNullOrWhiteSpace(t))
                .Select(t => new ProjectTechnology { TechnologyName = t.Trim() })
                .ToList();
        }

        if (request.ImageUrls is not null)
        {
            _dbContext.ProjectImages.RemoveRange(entity.Images);
            entity.Images = request.ImageUrls
                .Where(url => !string.IsNullOrWhiteSpace(url))
                .Select((url, index) => new ProjectImage { ImageUrl = url.Trim(), DisplayOrder = index })
                .ToList();
        }

        entity.UpdatedAt = DateTime.UtcNow;
        await _dbContext.SaveChangesAsync(cancellationToken);
        return MapDetail(entity);
    }

    public async Task<bool> DeleteProjectAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _dbContext.Projects
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

        if (entity is null)
        {
            return false;
        }

        _dbContext.Projects.Remove(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }

    private static ProjectDetailDto MapDetail(Project project)
    {
        return new ProjectDetailDto
        {
            Id = project.Id,
            Title = project.Title,
            Slug = project.Slug,
            ShortDescription = project.ShortDescription,
            Description = project.Description,
            Category = project.Category,
            GitHubUrl = project.GitHubUrl,
            LiveDemoUrl = project.LiveDemoUrl,
            IsFeatured = project.IsFeatured,
            IsPublished = project.IsPublished,
            CreatedAt = project.CreatedAt,
            UpdatedAt = project.UpdatedAt,
            Technologies = project.Technologies.Select(t => t.TechnologyName).ToList(),
            Images = project.Images.OrderBy(i => i.DisplayOrder).Select(i => i.ImageUrl).ToList(),
        };
    }

    private static string CreateSlug(string source)
    {
        var slug = source.Trim();
        slug = slug.ToLowerInvariant();
        slug = new string(slug.Select(ch => char.IsLetterOrDigit(ch) || ch == '-' || ch == ' ' ? ch : '-').ToArray());
        slug = slug.Replace(' ', '-');
        slug = string.Join('-', slug.Split('-', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries));
        return string.IsNullOrWhiteSpace(slug) ? Guid.NewGuid().ToString("N") : slug;
    }
}
