using Bob_sells_corn.Extensions;
using Bob_sells_corn.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bob_sells_corn.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ClientController : Controller
{
    private readonly IClientService _clientService;
    private readonly IRateLimiterService _rateLimiter;

    public ClientController(IClientService clientService, IRateLimiterService rateLimiter)
    {
        _clientService = clientService;
        _rateLimiter = rateLimiter;
    }

    [HttpPost("buy-corn")]
    public async Task<IActionResult> BuyCorn()
    {
        // logic to process the purchase of corn for the client
        //get clientID first
        Guid clientId = User.GetClientId();

        if (!_rateLimiter.CanPurchase(clientId))
            return StatusCode(429, "You exceeded the limit. Please wait before purchasing again.");

        //Record Purchase and update data
        var client = await _clientService.PurchaseCornAsync(clientId);
        _rateLimiter.RecordPurchase(clientId);

        return Ok(new { success = true, totalCornPurchased = client.TotalCornPurchased });
    }

    [HttpGet("get-corn-purchased")]
    public async Task<IActionResult> GetCornPurchased()
    {
        Guid clientId = User.GetClientId();
        var totalCornPurchased = await _clientService.GetTotalCornPurchasedAsync(clientId);
        return Ok(new { totalCornPurchased });
    }
}