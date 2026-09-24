using AliHussainPortfolio.Application.DTOs;
using AliHussainPortfolio.Application.Interfaces;
using AliHussainPortfolio.Domain.Entities;
using AliHussainPortfolio.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AliHussainPortfolio.Infrastructure.Services;

public class ContactService : IContactService
{
    private readonly AppDbContext _dbContext;

    public ContactService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ContactMessageDto> CreateMessageAsync(CreateContactMessageDto request, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            throw new InvalidOperationException("Name is required.");
        }

        if (string.IsNullOrWhiteSpace(request.Email) || !request.Email.Contains('@'))
        {
            throw new InvalidOperationException("A valid email is required.");
        }

        if (string.IsNullOrWhiteSpace(request.Message))
        {
            throw new InvalidOperationException("Message is required.");
        }

        var entity = new ContactMessage
        {
            Name = request.Name.Trim(),
            Email = request.Email.Trim(),
            ProjectType = request.ProjectType,
            BudgetRange = request.BudgetRange,
            Message = request.Message.Trim(),
            CreatedAt = DateTime.UtcNow,
            IsRead = false,
        };

        _dbContext.ContactMessages.Add(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Map(entity);
    }

    public async Task<IReadOnlyList<ContactMessageDto>> GetMessagesAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.ContactMessages
            .AsNoTracking()
            .OrderByDescending(m => m.CreatedAt)
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
    }

    public async Task<ContactMessageDto?> MarkAsReadAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _dbContext.ContactMessages
            .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);

        if (entity is null)
        {
            return null;
        }

        entity.IsRead = true;
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Map(entity);
    }

    public async Task<bool> DeleteMessageAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _dbContext.ContactMessages
            .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);

        if (entity is null)
        {
            return false;
        }

        _dbContext.ContactMessages.Remove(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }

    private static ContactMessageDto Map(ContactMessage message)
    {
        return new ContactMessageDto
        {
            Id = message.Id,
            Name = message.Name,
            Email = message.Email,
            ProjectType = message.ProjectType,
            BudgetRange = message.BudgetRange,
            Message = message.Message,
            CreatedAt = message.CreatedAt,
            IsRead = message.IsRead,
        };
    }
}
