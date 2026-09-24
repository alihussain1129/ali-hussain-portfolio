namespace AliHussainPortfolio.Domain.Entities;

public class CourseResource
{
    public int Id { get; set; }
    public int CourseId { get; set; }
    public Course? Course { get; set; }

    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }

    public string FileName { get; set; } = string.Empty;
    public string StorageKey { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public long FileSize { get; set; }

    public int ChapterNumber { get; set; }
    public int LessonNumber { get; set; }
    public int DownloadCount { get; set; } = 0;

    public bool IsPublished { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}