using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WealthTrack.Application.DTO;
using WealthTrack.Application.Services.CardS;
using WealthTrack.Domain.Exceptions;

namespace WealthTrack.API.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class CardController : ControllerBase
{
    private readonly ICardService _cardService;
    public CardController(ICardService cardService)
    {
        _cardService = cardService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CardDto>>> GetAll()
    {
        try
        {
            var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            return Ok(await _cardService.GetAllAsync(userId));
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateCardDto request)
    {
        try
        {
            var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            await _cardService.CreateCardAsync(request, userId);
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
    public async Task<IActionResult> Update(int id, UpdateCardDto request)
    {
        try
        {
            var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            await _cardService.UpdateCardAsync(id, request, userId);
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
            await _cardService.DeleteCardAsync(id, userId);
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
}