using CardReader.Application.Services;
using CardReader.WebApi.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace CardReader.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomerController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomerController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpPost]
    public async Task<ActionResult<int>> CreateCustomer([FromBody] CreateCustomerRequest request)
    {
        var userId = await _customerService.CreateUserAsync(request.FirstName, request.LastName, request.Email);

        if (userId is null)
        {
            return BadRequest(new { message = "Customer could not be created." });
        }

        return Ok(userId);
    }
}