using Microsoft.AspNetCore.Http;

namespace AliHussainPortfolio.Application.Interfaces;

public interface IFileStorageService
{
    Task<string> SaveAsync(IFormFile file, string? folder = null, CancellationToken cancellationToken = default);
    Task DeleteAsync(string storageKey, CancellationToken cancellationToken = default);
    Task<Stream> OpenReadStreamAsync(string storageKey, CancellationToken cancellationToken = default);
    Task<string> GetPublicUrlAsync(string storageKey, CancellationToken cancellationToken = default);
}
