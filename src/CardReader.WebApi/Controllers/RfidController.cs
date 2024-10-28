using CardReader.Application;
using Microsoft.AspNetCore.Mvc;

namespace CardReader.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RfidController : ControllerBase
{
    private readonly ILogger<RfidController> _logger;
    private readonly IUserService _userService;

    public RfidController(ILogger<RfidController> logger, IUserService userService)
    {
        _logger = logger;
        _userService = userService;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] RfidRequest? request)
    {
        if (request is null)
        {
            _logger.LogWarning("Received empty RFID request");
            return BadRequest();
        }

        if (string.IsNullOrWhiteSpace(request.NuidHex))
        {
            _logger.LogWarning("Received empty NUID");
            return BadRequest();
        }

        var user = await _userService.GetByRfidId(request.NuidHex);

        if (user is null)
        {
            return BadRequest("RFID has no valid user assigned.");
        }

        _logger.LogInformation("Received RFID request: {NuidHex}", request.NuidHex);

        return Ok($"User: {user.FirstName} {user.LastName}\nACCESS APPROVED");
    }
    
    [HttpGet("validUsers")]
    public async Task<IActionResult> GetValidUsers()
    {
        var users = await _userService.GetValidUsersAsync();
        return Ok(users.Select(u => u.RfidId));
    }
}

public record RfidRequest(string? NuidHex);