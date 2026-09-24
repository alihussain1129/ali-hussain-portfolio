namespace AliHussainPortfolio.Application.DTOs;

public class ProjectListItemDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string ShortDescription { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string? GitHubUrl { get; set; }
    public string? LiveDemoUrl { get; set; }
    public bool IsFeatured { get; set; }
    public bool IsPublished { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public List<string> Technologies { get; set; } = new();
}

public class ProjectDetailDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string ShortDescription { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string? GitHubUrl { get; set; }
    public string? LiveDemoUrl { get; set; }
    public bool IsFeatured { get; set; }
    public bool IsPublished { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public List<string> Technologies { get; set; } = new();
    public List<string> Images { get; set; } = new();
}

public class CreateProjectDto
{
    public string Title { get; set; } = string.Empty;
    public string ShortDescription { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string? GitHubUrl { get; set; }
    public string? LiveDemoUrl { get; set; }
    public bool IsFeatured { get; set; }
    public bool IsPublished { get; set; } = false;
    public List<string> Technologies { get; set; } = new();
    public List<string> ImageUrls { get; set; } = new();
}

public class UpdateProjectDto
{
    public string? Title { get; set; }
    public string? ShortDescription { get; set; }
    public string? Description { get; set; }
    public string? Category { get; set; }
    public string? GitHubUrl { get; set; }
    public string? LiveDemoUrl { get; set; }
    public bool? IsFeatured { get; set; }
    public bool? IsPublished { get; set; }
    public List<string>? Technologies { get; set; }
    public List<string>? ImageUrls { get; set; }
}
