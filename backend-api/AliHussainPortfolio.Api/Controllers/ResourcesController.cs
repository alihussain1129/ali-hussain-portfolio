using AliHussainPortfolio.Application.DTOs;
using AliHussainPortfolio.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AliHussainPortfolio.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ResourcesController : ControllerBase
{
    private readonly IResourceService _resourceService;

    public ResourcesController(IResourceService resourceService)
    {
        _resourceService = resourceService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var resources = await _resourceService.GetPublishedResourcesAsync(cancellationToken);
        return Ok(resources);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var resource = await _resourceService.GetResourceByIdAsync(id, cancellationToken);
        if (resource is null)
        {
            return NotFound();
        }

        return Ok(resource);
    }

    [HttpPost("upload")]
    [Authorize(Roles = "Admin")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Upload([FromForm] ResourceUploadDto request, CancellationToken cancellationToken)
    {
        try
        {
            var resource = await _resourceService.UploadResourceAsync(request, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = resource.Id }, resource);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Update(int id, [FromForm] ResourceUpdateDto request, CancellationToken cancellationToken)
    {
        try
        {
            var resource = await _resourceService.UpdateResourceAsync(id, request, cancellationToken);
            if (resource is null)
            {
                return NotFound();
            }

            return Ok(resource);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var deleted = await _resourceService.DeleteResourceAsync(id, cancellationToken);
        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpGet("{id:int}/view")]
    public async Task<IActionResult> View(int id, CancellationToken cancellationToken)
    {
        try
        {
            var (stream, contentType, fileName) = await _resourceService.GetResourceFileAsync(id, allowPrivate: false, cancellationToken);
            await _resourceService.IncrementDownloadAsync(id, cancellationToken);
            return File(stream, contentType, fileName);
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (FileNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpGet("{id:int}/download")]
    public async Task<IActionResult> Download(int id, CancellationToken cancellationToken)
    {
        try
        {
            var (stream, contentType, fileName) = await _resourceService.GetResourceFileAsync(id, allowPrivate: false, cancellationToken);
            await _resourceService.IncrementDownloadAsync(id, cancellationToken);
            return File(stream, contentType, fileName, enableRangeProcessing: true);
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (FileNotFoundException)
        {
            return NotFound();
        }
    }
}
