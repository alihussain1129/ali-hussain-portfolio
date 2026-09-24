using AliHussainPortfolio.Application.DTOs;

namespace AliHussainPortfolio.Application.Interfaces;

public interface IContactService
{
    Task<ContactMessageDto> CreateMessageAsync(CreateContactMessageDto request, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ContactMessageDto>> GetMessagesAsync(CancellationToken cancellationToken = default);
    Task<ContactMessageDto?> MarkAsReadAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> DeleteMessageAsync(int id, CancellationToken cancellationToken = default);
}
