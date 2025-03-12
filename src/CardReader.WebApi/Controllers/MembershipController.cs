using CardReader.Application;
using CardReader.WebApi.Dtos;
using CardReader.WebApi.Mappings;
using Microsoft.AspNetCore.Mvc;

namespace CardReader.WebApi.Controllers;

[ApiController]
[Route("api/membership")]
public class MembershipController : ControllerBase
{
    private readonly IMembershipService _membershipService;

    public MembershipController(IMembershipService membershipService)
    {
        _membershipService = membershipService;
    }

    [HttpPost]
    [Route("activate/{id}")]
    public async Task<ActionResult> Activate(int id)
    {
        var result = await _membershipService.ActivateAsync(id);
        if (!result)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpPost]
    [Route("deactivate/{id}")]
    public async Task<ActionResult> Deactivate(int id)
    {
        var result = await _membershipService.DeactivateAsync(id);
        if (!result)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpPost]
    [Route("extend/{id}")]
    public async Task<ActionResult> Extend(int id, [FromBody] ExtendMembershipRequest request)
    {
        var result = await _membershipService.ExtendAsync(id, request.Days);
        if (!result)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpPost]
    [Route("revoke/{id}")]
    public async Task<ActionResult> Revoke(int id)
    {
        var result = await _membershipService.RevokeAsync(id);
        if (!result)
        {
            return NotFound();
        }

        return NoContent();
    }
}
