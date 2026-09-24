namespace AliHussainPortfolio.Application.DTOs;

public class CreateContactMessageDto
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? ProjectType { get; set; }
    public string? BudgetRange { get; set; }
    public string Message { get; set; } = string.Empty;
}

public class ContactMessageDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? ProjectType { get; set; }
    public string? BudgetRange { get; set; }
    public string Message { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public bool IsRead { get; set; }
}

public class DashboardStatsDto
{
    public int TotalProjects { get; set; }
    public int TotalCourses { get; set; }
    public int TotalResources { get; set; }
    public int TotalDownloads { get; set; }
    public int UnreadMessages { get; set; }
    public List<ProjectListItemDto> RecentProjects { get; set; } = new();
    public List<ResourceListItemDto> RecentResources { get; set; } = new();
    public List<ContactMessageDto> RecentMessages { get; set; } = new();
}
