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
    public async Task<ActionResult<int>> CreateMembership([FromBody] MembershipCreateRequest request)
    {
        var isCreated = await _membershipService.CreateMembershipAsync(
            request.CustomerId, 
            request.CardNumber, 
            request.ExpiresAt);

        if (isCreated is false)
        {
            return BadRequest(new { message = "Membership could not be created." });
        }

        return Ok("Membership created successfully.");
    }

    [HttpPost]
    [Route("validate/{cardNumber}")]
    public async Task<ActionResult<bool>> ValidateCard(string cardNumber)
    {
        var isValid = await _membershipService.ValidateCardAccessAsync(cardNumber);
        
        if (isValid is false)
        {
            return BadRequest(new { message = "Card is not valid." });
        }
        
        return Ok("Card is valid.");
    }
}