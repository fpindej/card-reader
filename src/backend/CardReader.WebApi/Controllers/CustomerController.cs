﻿using CardReader.Application.Services;
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
    public async Task<IActionResult> CreateCustomer([FromBody] CustomerCreateRequest request)
    {
        var result = await _customerService.CreateUserAsync(request.FirstName, request.LastName, request.Email);

        if (!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }
        
        var customer = result.Value!;

        return Ok(new
        {
            Message = "Customer created successfully.",
            Customer = new
            {
                Id = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email
            }
        });
    }
    
    [HttpGet]
    [Route("get/{id:int}")]
    public async Task<ActionResult<CustomerGetResponse>> GetCustomerById(int id)
    {
        var customer = await _customerService.GetByIdAsync(id);
    
        if (customer is null)
        {
            return NotFound(new { message = "Customer not found." });
        }

        return Ok(new CustomerGetResponse(customer.Id, customer.FirstName, customer.LastName, customer.Email));
    }
    
    [HttpGet]
    [Route("get/{email}")]
    public async Task<ActionResult<CustomerGetResponse>> GetCustomerById([FromRoute] string email)
    {
        var result = await _customerService.GetByEmailAsync(email);
    
        if (!result.IsSuccess)
        {
            return NotFound(result.Error);
        }
        
        var customer = result.Value!;

        return Ok(new CustomerGetResponse(customer.Id, customer.FirstName, customer.LastName, customer.Email));
    }

    [HttpGet]
    [Route("getall")]
    public async Task<ActionResult<IEnumerable<CustomerGetResponse>>> GetAllCustomers([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var customers = await _customerService.GetAllAsync(pageNumber, pageSize);
        return Ok(customers.Select(c => new CustomerGetResponse(c.Id, c.FirstName, c.LastName, c.Email)));
    }
    
    [HttpPut]
    [Route("update")]
    public async Task<ActionResult> UpdateCustomer([FromBody] CustomerUpdateRequest request)
    {
        var isUpdated = await _customerService.UpdateAsync(request.Id, request.FirstName, request.LastName, request.Email);

        if (!isUpdated)
            return BadRequest("Customer could not be updated.");

        return NoContent();
    }
    
    [HttpDelete]
    [Route("delete/{id:int}")]
    public async Task<ActionResult> DeleteCustomer([FromRoute] int id)
    {
        var deleted = await _customerService.DeleteByIdAsync(id);
        if (!deleted)
            return NotFound("Customer not found.");

        return Ok("Customer deleted.");
    }
}

