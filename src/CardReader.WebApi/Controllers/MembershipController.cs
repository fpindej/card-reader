using CardReader.Application.Services;
using CardReader.WebApi.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace CardReader.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MembershipController : ControllerBase
{
    private readonly IMembershipService _membershipService;

    public MembershipController(IMembershipService membershipService)
    {
        _membershipService = membershipService;
    }

    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> CreateMembership([FromBody] MembershipCreateRequest request)
    {
        var result = await _membershipService.CreateMembershipAsync(
            request.CustomerId, 
            request.CardNumber, 
            request.ExpiresAt);

        if (!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }

        return Ok("Membership created successfully.");
    }

    [HttpPost]
    [Route("validate/{cardNumber}")]
    public async Task<IActionResult> ValidateCard(string cardNumber)
    {
        var result = await _membershipService.ValidateCardAccessAsync(cardNumber);
        
        if (!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }
        
        return Ok("Card is valid.");
    }
    
    [HttpPut]
    [Route("extend")]
    public async Task<IActionResult> ExtendMembership([FromBody] MembershipExtendRequest request)
    {
        var result = await _membershipService.ExtendMembershipAsync(
            request.CustomerId,
            request.DaysToExtend);

        if (!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }

        return Ok("Membership successfully extended.");
    }
    
    [HttpPost]
    [Route("revoke")]
    public async Task<IActionResult> RevokeMembership([FromBody] MembershipRevokeRequest request)
    {
        var revoked = await _membershipService.RevokeMembershipAsync(request.CustomerId);

        if (!revoked)
        {
            return BadRequest(new { message = "Could not revoke membership." });
        }

        return Ok("Membership revoked successfully.");
    }
}