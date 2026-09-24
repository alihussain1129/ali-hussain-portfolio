using Microsoft.AspNetCore.Http;

namespace AliHussainPortfolio.Application.DTOs;

public class ResourceListItemDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string CourseTitle { get; set; } = string.Empty;
    public int CourseId { get; set; }
    public int ChapterNumber { get; set; }
    public int LessonNumber { get; set; }
    public string ContentType { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public int DownloadCount { get; set; }
    public bool IsPublished { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class ResourceDetailDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int CourseId { get; set; }
    public string CourseTitle { get; set; } = string.Empty;
    public int ChapterNumber { get; set; }
    public int LessonNumber { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public int DownloadCount { get; set; }
    public bool IsPublished { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string PublicUrl { get; set; } = string.Empty;
}

public class ResourceUploadDto
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int CourseId { get; set; }
    public int ChapterNumber { get; set; }
    public int LessonNumber { get; set; }
    public bool IsPublished { get; set; } = false;
    public IFormFile? File { get; set; }
}

public class ResourceUpdateDto
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public int? CourseId { get; set; }
    public int? ChapterNumber { get; set; }
    public int? LessonNumber { get; set; }
    public bool? IsPublished { get; set; }
    public IFormFile? File { get; set; }
}
