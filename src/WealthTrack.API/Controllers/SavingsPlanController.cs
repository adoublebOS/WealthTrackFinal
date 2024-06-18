using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WealthTrack.Application.DTO;
using WealthTrack.Application.Services.SavingsPlanS;
using WealthTrack.Domain.Exceptions;

namespace WealthTrack.API.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class SavingsPlanController : ControllerBase
{
    private readonly ISavingsPlanService _savingsPlanService;

    public SavingsPlanController(ISavingsPlanService savingsPlanService)
    {
        _savingsPlanService = savingsPlanService;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<SavingsPlanDto>>> Get()
    {
        try
        {
            var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            return Ok(await _savingsPlanService.GetAllAsync(userId));
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> Create(CreateSavingsPlanDto request)
    {
        try
        {
            var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            await _savingsPlanService.CreateSavingsPlanAsync(request, userId);
            return Ok();
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateSavingsPlanDto request)
    {
        try
        {
            var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            await _savingsPlanService.UpdateSavingsPlanAsync(id, request, userId);
            return Ok();
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (AccessDeniedException e)
        {
            return Unauthorized(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            await _savingsPlanService.DeleteSavingsPlanAsync(id, userId);
            return NoContent();
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (AccessDeniedException e)
        {
            return Unauthorized(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
}