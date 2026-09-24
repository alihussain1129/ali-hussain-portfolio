using AliHussainPortfolio.Application.DTOs;
using Microsoft.AspNetCore.Http;

namespace AliHussainPortfolio.Application.Interfaces;

public interface IResourceService
{
    Task<IReadOnlyList<ResourceListItemDto>> GetPublishedResourcesAsync(CancellationToken cancellationToken = default);
    Task<ResourceDetailDto?> GetResourceByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<ResourceDetailDto> UploadResourceAsync(ResourceUploadDto request, CancellationToken cancellationToken = default);
    Task<ResourceDetailDto?> UpdateResourceAsync(int id, ResourceUpdateDto request, CancellationToken cancellationToken = default);
    Task<bool> DeleteResourceAsync(int id, CancellationToken cancellationToken = default);
    Task<(Stream FileStream, string ContentType, string FileName)> GetResourceFileAsync(int id, bool allowPrivate = false, CancellationToken cancellationToken = default);
    Task IncrementDownloadAsync(int id, CancellationToken cancellationToken = default);
}
