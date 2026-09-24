namespace AliHussainPortfolio.Domain.Entities;

public class Project
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string ShortDescription { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string? GitHubUrl { get; set; }
    public string? LiveDemoUrl { get; set; }
    public bool IsFeatured { get; set; } = false;
    public bool IsPublished { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<ProjectTechnology> Technologies { get; set; } = new List<ProjectTechnology>();
    public ICollection<ProjectImage> Images { get; set; } = new List<ProjectImage>();
}