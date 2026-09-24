using AliHussainPortfolio.Application.DTOs;
using AliHussainPortfolio.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AliHussainPortfolio.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CoursesController : ControllerBase
{
    private readonly ICourseService _courseService;

    public CoursesController(ICourseService courseService)
    {
        _courseService = courseService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var courses = await _courseService.GetPublishedCoursesAsync(cancellationToken);
        return Ok(courses);
    }

    [HttpGet("{slug}")]
    public async Task<IActionResult> GetBySlug(string slug, CancellationToken cancellationToken)
    {
        var course = await _courseService.GetCourseBySlugAsync(slug, cancellationToken);
        if (course is null)
        {
            return NotFound();
        }

        return Ok(course);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateCourseDto request, CancellationToken cancellationToken)
    {
        try
        {
            var course = await _courseService.CreateCourseAsync(request, cancellationToken);
            return CreatedAtAction(nameof(GetBySlug), new { slug = course.Slug }, course);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateCourseDto request, CancellationToken cancellationToken)
    {
        var course = await _courseService.UpdateCourseAsync(id, request, cancellationToken);
        if (course is null)
        {
            return NotFound();
        }

        return Ok(course);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var deleted = await _courseService.DeleteCourseAsync(id, cancellationToken);
        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}
