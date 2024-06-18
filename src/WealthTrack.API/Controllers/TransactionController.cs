using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WealthTrack.Application.DTO;
using WealthTrack.Application.Services.TransactionS;
using WealthTrack.Domain.Exceptions;

namespace WealthTrack.API.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class TransactionController : ControllerBase
{
    private readonly ITransactionService _transactionService;

    public TransactionController(ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TransactionDto>>> GetAll()
    {
        try
        {
            var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            return Ok(await _transactionService.GetAllAsync(userId));
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet("byCategories")]
    public async Task<ActionResult<IEnumerable<TransactionDto>>> GetAllByCategories([FromQuery] List<int> categoriesIds)
    {
        try
        {
            var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            return Ok(await _transactionService.GetAllByCategoriesAsync(categoriesIds, userId));
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
    
    [HttpGet("byWallets")]
    public async Task<ActionResult<IEnumerable<TransactionDto>>> GetAllByWallets([FromQuery] List<int> walletIds)
    {
        try
        {
            var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            return Ok(await _transactionService.GetAllByWalletsAsync(walletIds, userId));
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateTransactionDto request)
    {
        try
        {
            var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            await _transactionService.CreateTransactionAsync(request, userId);
            return Ok();
        }        
        catch (TransactionInvalidAmount e)
        {
            return BadRequest(e.Message);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (WalletLimitExceededException e)
        {
            return BadRequest(e.Message);
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

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateTransactionDto request)
    {
        try
        {
            var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            await _transactionService.UpdateTransactionAsync(id, request, userId);
            return Ok();
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (WalletLimitExceededException e)
        {
            return BadRequest(e.Message);
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
            await _transactionService.DeleteTransactionAsync(id, userId);
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