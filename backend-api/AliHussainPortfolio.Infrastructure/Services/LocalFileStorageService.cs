using AliHussainPortfolio.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace AliHussainPortfolio.Infrastructure.Services;

public class LocalFileStorageService : IFileStorageService
{
    private readonly string _rootPath;

    public LocalFileStorageService(IWebHostEnvironment environment, IConfiguration configuration)
    {
        var configuredRoot = configuration["FileStorage:RootPath"];
        _rootPath = Path.GetFullPath(string.IsNullOrWhiteSpace(configuredRoot)
            ? Path.Combine(environment.ContentRootPath, "App_Data", "uploads")
            : configuredRoot);
        Directory.CreateDirectory(_rootPath);
    }

    public async Task<string> SaveAsync(IFormFile file, string? folder = null, CancellationToken cancellationToken = default)
    {
        if (file is null || file.Length == 0)
        {
            throw new InvalidOperationException("A file is required.");
        }

        var extension = Path.GetExtension(file.FileName);
        if (string.IsNullOrWhiteSpace(extension) || !string.Equals(extension, ".pdf", StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException("Only PDF files are allowed.");
        }

        var contentType = file.ContentType?.Trim();
        if (!string.Equals(contentType, "application/pdf", StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException("The file must have a PDF content type.");
        }

        var safeName = Guid.NewGuid().ToString("N") + extension;
        var normalizedFolder = string.IsNullOrWhiteSpace(folder)
            ? string.Empty
            : $"{folder.Trim('/', '\\').Replace('\\', '/')}/";
        var storageKey = normalizedFolder + safeName;
        var fullPath = ResolveStoragePath(storageKey);
        Directory.CreateDirectory(Path.GetDirectoryName(fullPath) ?? _rootPath);

        await using var stream = new FileStream(fullPath, FileMode.Create, FileAccess.Write, FileShare.None);
        await file.CopyToAsync(stream, cancellationToken);

        return storageKey;
    }

    public Task DeleteAsync(string storageKey, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(storageKey))
        {
            return Task.CompletedTask;
        }

        var path = ResolveStoragePath(storageKey);

        if (File.Exists(path))
        {
            File.Delete(path);
        }

        return Task.CompletedTask;
    }

    public Task<Stream> OpenReadStreamAsync(string storageKey, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(storageKey))
        {
            throw new FileNotFoundException("Storage key is empty.");
        }

        var path = ResolveStoragePath(storageKey);

        if (!File.Exists(path))
        {
            throw new FileNotFoundException("The requested file was not found.");
        }

        return Task.FromResult<Stream>(new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read));
    }

    private string ResolveStoragePath(string storageKey)
    {
        if (string.IsNullOrWhiteSpace(storageKey))
        {
            throw new InvalidOperationException("A storage key is required.");
        }

        var normalized = storageKey.Replace('\\', '/').Trim('/');
        var path = Path.GetFullPath(Path.Combine(_rootPath, normalized.Replace('/', Path.DirectorySeparatorChar)));
        var relativePath = Path.GetRelativePath(_rootPath, path);
        if (Path.IsPathRooted(relativePath)
            || relativePath == ".."
            || relativePath.StartsWith($"..{Path.DirectorySeparatorChar}", StringComparison.Ordinal))
        {
            throw new InvalidOperationException("The storage key is outside the configured storage directory.");
        }

        return path;
    }
}
