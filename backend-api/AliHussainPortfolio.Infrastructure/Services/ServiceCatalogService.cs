using AliHussainPortfolio.Application.DTOs;
using AliHussainPortfolio.Application.Interfaces;
using AliHussainPortfolio.Domain.Entities;
using AliHussainPortfolio.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AliHussainPortfolio.Infrastructure.Services;

public class ServiceCatalogService : IServiceCatalogService
{
    private readonly AppDbContext _dbContext;

    public ServiceCatalogService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<ServiceItemDto>> GetPublishedServicesAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Services
            .AsNoTracking()
            .Where(s => s.IsPublished)
            .OrderBy(s => s.DisplayOrder)
            .Select(s => new ServiceItemDto
            {
                Id = s.Id,
                Title = s.Title,
                Description = s.Description,
                Icon = s.Icon,
                DisplayOrder = s.DisplayOrder,
                IsPublished = s.IsPublished,
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<ServiceItemDto?> GetServiceByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var service = await _dbContext.Services
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

        return service is null ? null : Map(service);
    }

    public async Task<ServiceItemDto> CreateServiceAsync(CreateServiceDto request, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.Title))
        {
            throw new InvalidOperationException("Service title is required.");
        }

        var entity = new Service
        {
            Title = request.Title.Trim(),
            Description = request.Description ?? string.Empty,
            Icon = request.Icon,
            DisplayOrder = request.DisplayOrder,
            IsPublished = request.IsPublished,
        };

        _dbContext.Services.Add(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return Map(entity);
    }

    public async Task<ServiceItemDto?> UpdateServiceAsync(int id, UpdateServiceDto request, CancellationToken cancellationToken = default)
    {
        var entity = await _dbContext.Services
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

        if (entity is null)
        {
            return null;
        }

        if (request.Title is not null) entity.Title = request.Title.Trim();
        if (request.Description is not null) entity.Description = request.Description;
        if (request.Icon is not null) entity.Icon = request.Icon;
        if (request.DisplayOrder.HasValue) entity.DisplayOrder = request.DisplayOrder.Value;
        if (request.IsPublished.HasValue) entity.IsPublished = request.IsPublished.Value;

        await _dbContext.SaveChangesAsync(cancellationToken);
        return Map(entity);
    }

    public async Task<bool> DeleteServiceAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _dbContext.Services
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

        if (entity is null)
        {
            return false;
        }

        _dbContext.Services.Remove(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }

    private static ServiceItemDto Map(Service service)
    {
        return new ServiceItemDto
        {
            Id = service.Id,
            Title = service.Title,
            Description = service.Description,
            Icon = service.Icon,
            DisplayOrder = service.DisplayOrder,
            IsPublished = service.IsPublished,
        };
    }
}
