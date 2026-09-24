using AliHussainPortfolio.Application.DTOs;
using AliHussainPortfolio.Application.Interfaces;
using AliHussainPortfolio.Domain.Entities;
using AliHussainPortfolio.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AliHussainPortfolio.Infrastructure.Services;

public class CourseService : ICourseService
{
    private readonly AppDbContext _dbContext;

    public CourseService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<CourseListItemDto>> GetPublishedCoursesAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Courses
            .AsNoTracking()
            .Where(c => c.IsPublished)
            .OrderByDescending(c => c.UpdatedAt)
            .Select(c => new CourseListItemDto
            {
                Id = c.Id,
                Title = c.Title,
                Slug = c.Slug,
                Description = c.Description,
                Category = c.Category,
                ThumbnailUrl = c.ThumbnailUrl,
                Author = c.Author,
                IsPublished = c.IsPublished,
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt,
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<CourseDetailDto?> GetCourseBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(slug))
        {
            return null;
        }

        var course = await _dbContext.Courses
            .AsNoTracking()
            .Include(c => c.Resources)
            .FirstOrDefaultAsync(c => c.Slug == slug, cancellationToken);

        if (course is null)
        {
            return null;
        }

        return MapDetail(course);
    }

    public async Task<CourseDetailDto> CreateCourseAsync(CreateCourseDto request, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.Title))
        {
            throw new InvalidOperationException("Course title is required.");
        }

        var entity = new Course
        {
            Title = request.Title.Trim(),
            Slug = CreateSlug(request.Title),
            Description = request.Description ?? string.Empty,
            Category = request.Category ?? string.Empty,
            ThumbnailUrl = request.ThumbnailUrl,
            Author = string.IsNullOrWhiteSpace(request.Author) ? "Ali Hussain" : request.Author.Trim(),
            IsPublished = request.IsPublished,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
        };

        _dbContext.Courses.Add(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return MapDetail(entity);
    }

    public async Task<CourseDetailDto?> UpdateCourseAsync(int id, UpdateCourseDto request, CancellationToken cancellationToken = default)
    {
        var entity = await _dbContext.Courses
            .Include(c => c.Resources)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

        if (entity is null)
        {
            return null;
        }

        if (!string.IsNullOrWhiteSpace(request.Title))
        {
            entity.Title = request.Title.Trim();
            entity.Slug = CreateSlug(entity.Title);
        }

        if (request.Description is not null)
        {
            entity.Description = request.Description;
        }

        if (request.Category is not null)
        {
            entity.Category = request.Category;
        }

        if (request.ThumbnailUrl is not null)
        {
            entity.ThumbnailUrl = request.ThumbnailUrl;
        }

        if (request.Author is not null)
        {
            entity.Author = request.Author.Trim();
        }

        if (request.IsPublished.HasValue)
        {
            entity.IsPublished = request.IsPublished.Value;
        }

        entity.UpdatedAt = DateTime.UtcNow;
        await _dbContext.SaveChangesAsync(cancellationToken);

        return MapDetail(entity);
    }

    public async Task<bool> DeleteCourseAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _dbContext.Courses
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

        if (entity is null)
        {
            return false;
        }

        _dbContext.Courses.Remove(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }

    private static CourseDetailDto MapDetail(Course course)
    {
        return new CourseDetailDto
        {
            Id = course.Id,
            Title = course.Title,
            Slug = course.Slug,
            Description = course.Description,
            Category = course.Category,
            ThumbnailUrl = course.ThumbnailUrl,
            Author = course.Author,
            IsPublished = course.IsPublished,
            CreatedAt = course.CreatedAt,
            UpdatedAt = course.UpdatedAt,
            Resources = course.Resources
                .OrderBy(r => r.ChapterNumber)
                .ThenBy(r => r.LessonNumber)
                .Select(r => new ResourceListItemDto
                {
                    Id = r.Id,
                    Title = r.Title,
                    Description = r.Description ?? string.Empty,
                    CourseTitle = course.Title,
                    CourseId = course.Id,
                    ChapterNumber = r.ChapterNumber,
                    LessonNumber = r.LessonNumber,
                    ContentType = r.ContentType,
                    FileSize = r.FileSize,
                    DownloadCount = r.DownloadCount,
                    IsPublished = r.IsPublished,
                    CreatedAt = r.CreatedAt,
                    UpdatedAt = r.UpdatedAt,
                })
                .ToList(),
        };
    }

    private static string CreateSlug(string source)
    {
        var slug = source.Trim();
        slug = slug.ToLowerInvariant();
        slug = new string(slug.Select(ch => char.IsLetterOrDigit(ch) || ch == '-' || ch == ' ' ? ch : '-').ToArray());
        slug = slug.Replace(" ", "-");
        slug = string.Join('-', slug.Split('-', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries));
        return string.IsNullOrWhiteSpace(slug) ? Guid.NewGuid().ToString("N") : slug;
    }
}
