using AliHussainPortfolio.Application.DTOs;

namespace AliHussainPortfolio.Application.Interfaces;

public interface IServiceCatalogService
{
    Task<IReadOnlyList<ServiceItemDto>> GetPublishedServicesAsync(CancellationToken cancellationToken = default);
    Task<ServiceItemDto?> GetServiceByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<ServiceItemDto> CreateServiceAsync(CreateServiceDto request, CancellationToken cancellationToken = default);
    Task<ServiceItemDto?> UpdateServiceAsync(int id, UpdateServiceDto request, CancellationToken cancellationToken = default);
    Task<bool> DeleteServiceAsync(int id, CancellationToken cancellationToken = default);
}
