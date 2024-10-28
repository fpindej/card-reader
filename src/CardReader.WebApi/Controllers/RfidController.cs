using CardReader.Application;
using CardReader.WebApi.Dtos;
using CardReader.WebApi.Mappings;
using Microsoft.AspNetCore.Mvc;

namespace CardReader.WebApi.Controllers;

[ApiController]
[Route("api/rfidcard")]
public class RfidCardController : ControllerBase
{
    private readonly IRfidCardService _rfidCardService;

    public RfidCardController(IRfidCardService rfidCardService)
    {
        _rfidCardService = rfidCardService;
    }

    [HttpPost]
    [Route("create")]
    public async Task<ActionResult<CreateRfidCardResponse>> Create([FromBody] CreateRfidCardRequest createRfidCardRequest)
    {
        var rfidCard = createRfidCardRequest.ToDomain();

        var newRfidCard = await _rfidCardService.CreateAsync(rfidCard);

        return CreatedAtAction(nameof(GetById), new { id = newRfidCard.Id }, newRfidCard.ToResponse());
    }

    [HttpGet]
    [Route("get/{id}")]
    public async Task<ActionResult<RfidCardResponse>> GetById(string id)
    {
        var rfidCard = await _rfidCardService.GetByIdAsync(id);
        if (rfidCard == null)
        {
            return NotFound();
        }

        return Ok(rfidCard.ToResponse());
    }

    [HttpGet]
    [Route("getallactive")]
    public async Task<ActionResult<IEnumerable<string>>> GetAllActive()
    {
        var activeCards = await _rfidCardService.GetAllActiveAsync();
        var activeCardIds = activeCards.Select(card => card.Id);
        
        return Ok(activeCardIds);
    }

    [HttpPost]
    [Route("activate/{id}")]
    public async Task<ActionResult> Activate(string id)
    {
        await _rfidCardService.ActivateAsync(id);
        return NoContent();
    }

    [HttpPost]
    [Route("deactivate/{id}")]
    public async Task<ActionResult> Deactivate(string id)
    {
        await _rfidCardService.DeactivateAsync(id);
        return NoContent();
    }

    [HttpDelete]
    [Route("delete/{id}")]
    public async Task<ActionResult> Delete(string id)
    {
        var result = await _rfidCardService.DeleteAsync(id);
        if (!result)
        {
            return NotFound();
        }

        return NoContent();
    }
}