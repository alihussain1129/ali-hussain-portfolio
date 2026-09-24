using AliHussainPortfolio.Application.DTOs;
using AliHussainPortfolio.Application.Interfaces;
using AliHussainPortfolio.Domain.Entities;
using AliHussainPortfolio.Infrastructure.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace AliHussainPortfolio.Infrastructure.Services;

public class ResourceService : IResourceService
{
    private readonly AppDbContext _dbContext;
    private readonly IFileStorageService _fileStorageService;

    public ResourceService(AppDbContext dbContext, IFileStorageService fileStorageService)
    {
        _dbContext = dbContext;
        _fileStorageService = fileStorageService;
    }

    public async Task<IReadOnlyList<ResourceListItemDto>> GetPublishedResourcesAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.CourseResources
            .AsNoTracking()
            .Include(r => r.Course)
            .Where(r => r.IsPublished)
            .OrderByDescending(r => r.UpdatedAt)
            .Select(r => new ResourceListItemDto
            {
                Id = r.Id,
                Title = r.Title,
                Description = r.Description ?? string.Empty,
                CourseTitle = r.Course != null ? r.Course.Title : string.Empty,
                CourseId = r.CourseId,
                ChapterNumber = r.ChapterNumber,
                LessonNumber = r.LessonNumber,
                ContentType = r.ContentType,
                FileSize = r.FileSize,
                DownloadCount = r.DownloadCount,
                IsPublished = r.IsPublished,
                CreatedAt = r.CreatedAt,
                UpdatedAt = r.UpdatedAt,
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<ResourceDetailDto?> GetResourceByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var resource = await _dbContext.CourseResources
            .AsNoTracking()
            .Include(r => r.Course)
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);

        if (resource is null)
        {
            return null;
        }

        return MapDetail(resource);
    }

    public async Task<ResourceDetailDto> UploadResourceAsync(ResourceUploadDto request, CancellationToken cancellationToken = default)
    {
        if (request.File is null || request.File.Length == 0)
        {
            throw new InvalidOperationException("A PDF file is required.");
        }

        if (string.IsNullOrWhiteSpace(request.Title))
        {
            throw new InvalidOperationException("Resource title is required.");
        }

        var courseExists = await _dbContext.Courses.AnyAsync(c => c.Id == request.CourseId, cancellationToken);
        if (!courseExists)
        {
            throw new InvalidOperationException("The selected course does not exist.");
        }

        var extension = Path.GetExtension(request.File.FileName);
        if (!string.Equals(extension, ".pdf", StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException("Only PDF files are accepted.");
        }

        if (!string.Equals(request.File.ContentType, "application/pdf", StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException("The uploaded file is not a valid PDF document.");
        }

        if (request.File.Length > 25 * 1024 * 1024)
        {
            throw new InvalidOperationException("The PDF file must be smaller than 25MB.");
        }

        var storageKey = await _fileStorageService.SaveAsync(request.File, "resources", cancellationToken);
        var entity = new CourseResource
        {
            CourseId = request.CourseId,
            Title = request.Title.Trim(),
            Description = request.Description,
            FileName = Path.GetFileName(request.File.FileName),
            StorageKey = storageKey,
            ContentType = request.File.ContentType,
            FileSize = request.File.Length,
            ChapterNumber = request.ChapterNumber,
            LessonNumber = request.LessonNumber,
            IsPublished = request.IsPublished,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
        };

        _dbContext.CourseResources.Add(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return MapDetail(entity);
    }

    public async Task<ResourceDetailDto?> UpdateResourceAsync(int id, ResourceUpdateDto request, CancellationToken cancellationToken = default)
    {
        var resource = await _dbContext.CourseResources
            .Include(r => r.Course)
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);

        if (resource is null)
        {
            return null;
        }

        if (request.Title is not null)
        {
            resource.Title = request.Title.Trim();
        }

        if (request.Description is not null)
        {
            resource.Description = request.Description;
        }

        if (request.CourseId.HasValue)
        {
            var courseExists = await _dbContext.Courses.AnyAsync(c => c.Id == request.CourseId.Value, cancellationToken);
            if (!courseExists)
            {
                throw new InvalidOperationException("The selected course does not exist.");
            }

            resource.CourseId = request.CourseId.Value;
        }

        if (request.ChapterNumber.HasValue)
        {
            resource.ChapterNumber = request.ChapterNumber.Value;
        }

        if (request.LessonNumber.HasValue)
        {
            resource.LessonNumber = request.LessonNumber.Value;
        }

        if (request.IsPublished.HasValue)
        {
            resource.IsPublished = request.IsPublished.Value;
        }

        if (request.File is not null)
        {
            var extension = Path.GetExtension(request.File.FileName);
            if (!string.Equals(extension, ".pdf", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("Only PDF files are accepted.");
            }

            if (request.File.Length > 25 * 1024 * 1024)
            {
                throw new InvalidOperationException("The PDF file must be smaller than 25MB.");
            }

            await _fileStorageService.DeleteAsync(resource.StorageKey, cancellationToken);
            resource.StorageKey = await _fileStorageService.SaveAsync(request.File, "resources", cancellationToken);
            resource.FileName = Path.GetFileName(request.File.FileName);
            resource.ContentType = request.File.ContentType;
            resource.FileSize = request.File.Length;
        }

        resource.UpdatedAt = DateTime.UtcNow;
        await _dbContext.SaveChangesAsync(cancellationToken);

        return MapDetail(resource);
    }

    public async Task<bool> DeleteResourceAsync(int id, CancellationToken cancellationToken = default)
    {
        var resource = await _dbContext.CourseResources
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);

        if (resource is null)
        {
            return false;
        }

        await _fileStorageService.DeleteAsync(resource.StorageKey, cancellationToken);
        _dbContext.CourseResources.Remove(resource);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<(Stream FileStream, string ContentType, string FileName)> GetResourceFileAsync(int id, bool allowPrivate = false, CancellationToken cancellationToken = default)
    {
        var resource = await _dbContext.CourseResources
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);

        if (resource is null)
        {
            throw new FileNotFoundException("Resource not found.");
        }

        if (!allowPrivate && !resource.IsPublished)
        {
            throw new UnauthorizedAccessException("This resource is not available publicly.");
        }

        var stream = await _fileStorageService.OpenReadStreamAsync(resource.StorageKey, cancellationToken);
        return (stream, resource.ContentType, resource.FileName);
    }

    public async Task IncrementDownloadAsync(int id, CancellationToken cancellationToken = default)
    {
        var resource = await _dbContext.CourseResources
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);

        if (resource is null)
        {
            return;
        }

        resource.DownloadCount += 1;
        resource.UpdatedAt = DateTime.UtcNow;
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private static ResourceDetailDto MapDetail(CourseResource resource)
    {
        return new ResourceDetailDto
        {
            Id = resource.Id,
            Title = resource.Title,
            Description = resource.Description ?? string.Empty,
            CourseId = resource.CourseId,
            CourseTitle = resource.Course?.Title ?? string.Empty,
            ChapterNumber = resource.ChapterNumber,
            LessonNumber = resource.LessonNumber,
            FileName = resource.FileName,
            ContentType = resource.ContentType,
            FileSize = resource.FileSize,
            DownloadCount = resource.DownloadCount,
            IsPublished = resource.IsPublished,
            CreatedAt = resource.CreatedAt,
            UpdatedAt = resource.UpdatedAt,
            PublicUrl = $"/uploads/{resource.StorageKey.Replace('\\', '/')}"
        };
    }
}
