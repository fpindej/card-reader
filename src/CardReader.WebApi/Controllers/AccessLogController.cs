using CardReader.Application.Services;
using CardReader.WebApi.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace CardReader.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccessLogController : ControllerBase
{
    private readonly IAccessLogService _accessLogService;

    public AccessLogController(IAccessLogService accessLogService)
    {
        _accessLogService = accessLogService;
    }

    [HttpPost]
    [Route("log")]
    public async Task<IActionResult> LogAccess([FromBody] AccessLogRequest request)
    {
        await _accessLogService.LogAccessAsync(request.CardNumber, request.IsSuccessful);
        return Ok(new { Message = "Access log created successfully." });
    }

    [HttpGet]
    [Route("getall")]
    public async Task<IActionResult> GetAllLogs()
    {
        var logs = await _accessLogService.GetAllLogsAsync();
        return Ok(logs);
    }
}
