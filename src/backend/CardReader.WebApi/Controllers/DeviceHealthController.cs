using CardReader.Application.Services;
using CardReader.WebApi.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace CardReader.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DeviceHealthController : ControllerBase
{
    private readonly IDeviceHealthService _deviceHealthService;

    public DeviceHealthController(IDeviceHealthService deviceHealthService)
    {
        _deviceHealthService = deviceHealthService;
    }


    [HttpPost]
    [Route("log")]
    public async Task<IActionResult> Log([FromBody] DeviceHealthLogRequest request)
    {
        await _deviceHealthService.LogHealthAsync(request.DeviceId, request.MaxAllocHeap,
            request.MinFreeHeap, request.FreeHeap, request.Uptime, request.FreeSketchSpace);

        return Ok();
    }

    [HttpGet]
    [Route("getall")]
    public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var logs = await _deviceHealthService.GetAllLogsAsync(pageNumber, pageSize);
        
        return Ok(logs);
    }
}
