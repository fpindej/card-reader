using Microsoft.AspNetCore.Mvc;

namespace CardReader.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RfidController : ControllerBase
{
    private readonly ILogger<RfidController> _logger;

    public RfidController(ILogger<RfidController> logger)
    {
        _logger = logger;
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

        _logger.LogInformation("Received RFID request: {NuidHex}", request.NuidHex);

        return Ok();
    }
}

public record RfidRequest(string? NuidHex);