using AliHussainPortfolio.Application.DTOs;

namespace AliHussainPortfolio.Application.Interfaces;

public interface ICourseService
{
    Task<IReadOnlyList<CourseListItemDto>> GetPublishedCoursesAsync(CancellationToken cancellationToken = default);
    Task<CourseDetailDto?> GetCourseBySlugAsync(string slug, CancellationToken cancellationToken = default);
    Task<CourseDetailDto> CreateCourseAsync(CreateCourseDto request, CancellationToken cancellationToken = default);
    Task<CourseDetailDto?> UpdateCourseAsync(int id, UpdateCourseDto request, CancellationToken cancellationToken = default);
    Task<bool> DeleteCourseAsync(int id, CancellationToken cancellationToken = default);
}
