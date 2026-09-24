using AliHussainPortfolio.Application.DTOs;
using AliHussainPortfolio.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AliHussainPortfolio.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContactController : ControllerBase
{
    private readonly IContactService _contactService;

    public ContactController(IContactService contactService)
    {
        _contactService = contactService;
    }

    [HttpPost]
    public async Task<IActionResult> Submit([FromBody] CreateContactMessageDto request, CancellationToken cancellationToken)
    {
        try
        {
            var message = await _contactService.CreateMessageAsync(request, cancellationToken);
            return CreatedAtAction(nameof(Submit), new { id = message.Id }, message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var messages = await _contactService.GetMessagesAsync(cancellationToken);
        return Ok(messages);
    }

    [HttpPatch("{id:int}/read")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> MarkAsRead(int id, CancellationToken cancellationToken)
    {
        var message = await _contactService.MarkAsReadAsync(id, cancellationToken);
        return message is null ? NotFound() : Ok(message);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var deleted = await _contactService.DeleteMessageAsync(id, cancellationToken);
        return deleted ? NoContent() : NotFound();
    }
}
