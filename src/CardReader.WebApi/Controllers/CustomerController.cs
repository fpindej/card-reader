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
    [Route("create")]
    public async Task<ActionResult<int>> CreateCustomer([FromBody] CreateCustomerRequest request)
    {
        var userId = await _customerService.CreateUserAsync(request.FirstName, request.LastName, request.Email);

        if (userId is null)
        {
            return BadRequest(new { message = "Customer could not be created." });
        }

        return Ok(userId);
    }
    
    [HttpGet]
    [Route("get/{id:int}")]
    public async Task<ActionResult<CustomerResponse>> GetCustomerById(int id)
    {
        var customer = await _customerService.GetByIdAsync(id);
    
        if (customer is null)
        {
            return NotFound(new { message = "Customer not found." });
        }

        return Ok(new CustomerResponse(customer.Id, customer.FirstName, customer.LastName, customer.Email));
    }

    [HttpGet]
    [Route("getall")]
    public async Task<ActionResult<IEnumerable<CustomerResponse>>> GetAllCustomers([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var customers = await _customerService.GetAllAsync(pageNumber, pageSize);
        return Ok(customers.Select(c => new CustomerResponse(c.Id, c.FirstName, c.LastName, c.Email)));
    }
}