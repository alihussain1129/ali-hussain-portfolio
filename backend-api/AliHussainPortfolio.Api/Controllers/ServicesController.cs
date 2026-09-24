using AliHussainPortfolio.Application.DTOs;
using AliHussainPortfolio.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AliHussainPortfolio.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ServicesController : ControllerBase
{
    private readonly IServiceCatalogService _serviceCatalogService;

    public ServicesController(IServiceCatalogService serviceCatalogService)
    {
        _serviceCatalogService = serviceCatalogService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var services = await _serviceCatalogService.GetPublishedServicesAsync(cancellationToken);
        return Ok(services);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var service = await _serviceCatalogService.GetServiceByIdAsync(id, cancellationToken);
        if (service is null)
        {
            return NotFound();
        }

        return Ok(service);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateServiceDto request, CancellationToken cancellationToken)
    {
        try
        {
            var service = await _serviceCatalogService.CreateServiceAsync(request, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = service.Id }, service);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateServiceDto request, CancellationToken cancellationToken)
    {
        var service = await _serviceCatalogService.UpdateServiceAsync(id, request, cancellationToken);
        if (service is null)
        {
            return NotFound();
        }

        return Ok(service);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var deleted = await _serviceCatalogService.DeleteServiceAsync(id, cancellationToken);
        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}
