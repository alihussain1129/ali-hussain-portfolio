using AliHussainPortfolio.Application.DTOs;
using AliHussainPortfolio.Application.Interfaces;
using AliHussainPortfolio.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AliHussainPortfolio.Infrastructure.Services;

public class DashboardService : IDashboardService
{
    private readonly AppDbContext _dbContext;

    public DashboardService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<DashboardStatsDto> GetDashboardStatsAsync(CancellationToken cancellationToken = default)
    {
        var totalProjects = await _dbContext.Projects.CountAsync(cancellationToken);
        var totalCourses = await _dbContext.Courses.CountAsync(cancellationToken);
        var totalResources = await _dbContext.CourseResources.CountAsync(cancellationToken);
        var totalDownloads = await _dbContext.CourseResources.SumAsync(r => (long?)r.DownloadCount, cancellationToken) ?? 0;
        var unreadMessages = await _dbContext.ContactMessages.CountAsync(m => !m.IsRead, cancellationToken);

        var recentProjects = await _dbContext.Projects
            .AsNoTracking()
            .OrderByDescending(p => p.CreatedAt)
            .Take(5)
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

        var recentResources = await _dbContext.CourseResources
            .AsNoTracking()
            .OrderByDescending(r => r.CreatedAt)
            .Take(5)
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

        var recentMessages = await _dbContext.ContactMessages
            .AsNoTracking()
            .OrderByDescending(m => m.CreatedAt)
            .Take(5)
            .Select(m => new ContactMessageDto
            {
                Id = m.Id,
                Name = m.Name,
                Email = m.Email,
                ProjectType = m.ProjectType,
                BudgetRange = m.BudgetRange,
                Message = m.Message,
                CreatedAt = m.CreatedAt,
                IsRead = m.IsRead,
            })
            .ToListAsync(cancellationToken);

        return new DashboardStatsDto
        {
            TotalProjects = totalProjects,
            TotalCourses = totalCourses,
            TotalResources = totalResources,
            TotalDownloads = (int)totalDownloads,
            UnreadMessages = unreadMessages,
            RecentProjects = recentProjects,
            RecentResources = recentResources,
            RecentMessages = recentMessages,
        };
    }
}
