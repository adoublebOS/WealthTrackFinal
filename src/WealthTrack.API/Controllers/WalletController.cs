using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WealthTrack.Application.DTO;
using WealthTrack.Application.Services.WalletS;
using WealthTrack.Domain.Exceptions;

namespace WealthTrack.API.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class WalletController : ControllerBase
{
    private readonly IWalletService _walletService;

    public WalletController(IWalletService walletService)
    {
        _walletService = walletService;
    }

    [HttpGet("wallet")]
    public async Task<ActionResult<IEnumerable<WalletDto>>> GetWallets()
    {
        try
        {
            var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            return Ok(await _walletService.GetAllAsync(userId));
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpPost("wallet")]
    public async Task<IActionResult> CreateWallet(CreateWalletDto request)
    {
        try
        {
            var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            await _walletService.CreateWalletAsync(request, userId);
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

    [HttpPut("wallet/{id}")]
    public async Task<IActionResult> UpdateWallet(int id, UpdateWalletDto request)
    {
        try
        {
            var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            await _walletService.UpdateWalletAsync(id, request, userId);
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

    [HttpDelete("wallet/{id}")]
    public async Task<IActionResult> DeleteWallet(int id)
    {
        try
        {
            var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            await _walletService.DeleteWalletAsync(id, userId);
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

    [HttpGet("walletLimit/{walletId}")]
    public async Task<ActionResult<IEnumerable<WalletLimitDto>>> GetWalletLimits(int walletId)
    {
        try
        {
            var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            return Ok(await _walletService.GetWalletLimitsAsync(walletId, userId));
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

    [HttpGet("walletLimit/byMonth/{walletId}")]
    public async Task<ActionResult<IEnumerable<WalletLimitDto>>> GetWalletLimitByMonth(int walletId, DateTime month)
    {
        try
        {
            var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            return Ok(await _walletService.GetWalletLimitByMonthAsync(walletId, userId, month));
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

    [HttpPost("walletLimit")]
    public async Task<IActionResult> CreateWalletLimit(CreateWalletLimitDto request)
    {
        try
        {
            var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            await _walletService.CreateWalletLimitAsync(request, userId);
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

    [HttpPut("walletLimit/{id}")]
    public async Task<IActionResult> UpdateWalletLimit(int id, UpdateWalletLimitDto request)
    {
        try
        {
            var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            await _walletService.UpdateWalletLimitAsync(id, request, userId);
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

    [HttpDelete("walletLimit/{id}")]
    public async Task<IActionResult> DeleteWalletLimit(int id)
    {
        try
        {
            var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            await _walletService.DeleteWalletLimitAsync(id, userId);
            return NoContent();
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
}