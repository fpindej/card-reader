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
            request.CardNumber.ToLowerInvariant(), // ToDo: figure out a cleaner way to store lowercase variant
            request.ExpiresAt);

        if (!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }
        
        var membership = result.Value!;

        return Ok(new
        {
            Message = "Membership created successfully.",
            Membership = new
            {
                Id = membership.Id,
                CustomerId = membership.CustomerId,
                CardNumber = membership.CardNumber,
                ValidTo = membership.ExpiresAt!.Value.ToString("dd/MM/yyyy")
            }
        });
    }

    [HttpPost]
    [Route("checkactive/{cardNumber}")]
    public async Task<IActionResult> CheckMembershipByCard(string cardNumber)
    {
        var result = await _membershipService.CheckMembershipByCardNumberAsync(cardNumber);
        
        if (!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }
        
        var membership = result.Value!;
        
        return Ok(new
        {
            Message = "Membership is active.",
            Membership = new
            {
                Id = membership.Id,
                CustomerId = membership.CustomerId,
                CardNumber = membership.CardNumber,
                ValidTo = result.Value!.ExpiresAt!.Value.ToString("dd/MM/yyyy")
            }
        });
    }

    [HttpPut]
    [Route("extenddays")]
    public async Task<IActionResult> ExtendMembershipDays([FromBody] MembershipExtendDaysRequest request)
    {
        var result = await _membershipService.ExtendMembershipByDaysAsync(
            request.CustomerId,
            request.DaysToExtend);

        if (!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }

        var membership = result.Value!;

        return Ok(new
        {
            Message = "Membership extended successfully.",
            Membership = new
            {
                Id = membership.Id,
                CustomerId = membership.CustomerId,
                CardNumber = membership.CardNumber,
                ValidTo = membership.ExpiresAt!.Value.ToString("dd/MM/yyyy")
            }
        });
    }

    [HttpPut]
    [Route("extendmonths")]
    public async Task<IActionResult> ExtendMembershipMonths([FromBody] MembershipExtendMonthsRequest request)
    {
        var result = await _membershipService.ExtendMembershipByMonthsAsync(
            request.CustomerId,
            request.MonthsToExtend);

        if (!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }

        var membership = result.Value!;

        return Ok(new
        {
            Message = "Membership extended successfully.",
            Membership = new
            {
                Id = membership.Id,
                CustomerId = membership.CustomerId,
                CardNumber = membership.CardNumber,
                ValidTo = membership.ExpiresAt!.Value.ToString("dd/MM/yyyy")
            }
        });
    }
    
    [HttpPost]
    [Route("revoke")]
    public async Task<IActionResult> RevokeMembership([FromBody] MembershipRevokeRequest request)
    {
        var result = await _membershipService.RevokeMembershipAsync(request.CustomerId);

        if (!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }

        return Ok("Membership revoked successfully.");
    }
    
    [HttpGet]
    [Route("getactivecards")]
    public async Task<IActionResult> GetActiveCards()
    {
        var result = await _membershipService.GetActiveCardNumbersAsync();

        if (!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }

        return Ok(result.Value);
    }

    [HttpPost]
    [Route("getallcards")]
    public async Task<IActionResult> GetAllCards()
    {
        var result = await _membershipService.GetAllCardsAsync();
        
        if (!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }

        return Ok(result.Value);

    }
}