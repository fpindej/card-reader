using CardReader.Application.Services;
using CardReader.Domain;
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
        await _accessLogService.LogAccessAsync(request.CardNumber, request.IsSuccessful, request.Timestamp);
        return Ok();
    }
    
    [HttpPost]
    [Route("logbatch")]
    public async Task<IActionResult> LogAccessBatch([FromBody] IEnumerable<AccessLogRequest> requests)
    {
        await _accessLogService.LogAccessBatchAsync(requests.Select(r => new AccessLog()
        {
            CardNumber = r.CardNumber,
            IsSuccessful = r.IsSuccessful,
            EventDateTime = r.Timestamp
        }));
        return Ok();
    }

    [HttpGet]
    [Route("getall")]
    public async Task<IActionResult> GetAllLogs([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var logs = await _accessLogService.GetAllLogsAsync(pageNumber, pageSize);
        return Ok(logs);
    }
}
