using AliHussainPortfolio.Application.DTOs;

namespace AliHussainPortfolio.Application.Interfaces;

public interface IProjectService
{
    Task<IReadOnlyList<ProjectListItemDto>> GetPublishedProjectsAsync(CancellationToken cancellationToken = default);
    Task<ProjectDetailDto?> GetProjectByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<ProjectDetailDto?> GetProjectBySlugAsync(string slug, CancellationToken cancellationToken = default);
    Task<ProjectDetailDto> CreateProjectAsync(CreateProjectDto request, CancellationToken cancellationToken = default);
    Task<ProjectDetailDto?> UpdateProjectAsync(int id, UpdateProjectDto request, CancellationToken cancellationToken = default);
    Task<bool> DeleteProjectAsync(int id, CancellationToken cancellationToken = default);
}
