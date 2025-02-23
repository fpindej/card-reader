﻿using CardReader.Application;
using CardReader.Domain;
using CardReader.WebApi.Dtos;
using CardReader.WebApi.Mappings;
using Microsoft.AspNetCore.Mvc;

namespace CardReader.WebApi.Controllers;

[Route("api/user")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    [Route("create")]
    public async Task<ActionResult> Create([FromBody] CreateUserRequest createUserRequest)
    {
        var user = createUserRequest.ToDomain();

        var newUser = await _userService.CreateAsync(user);

        return CreatedAtAction(nameof(GetById), new { id = newUser.Id }, newUser.ToResponse());
    }

    [HttpGet]
    [Route("get/{id:int}")]
    public async Task<ActionResult> GetById([FromRoute] int id)
    {
        var user = await _userService.GetByIdAsync(id);
        
        if (user is null)
            return NotFound($"User {id} not found.");

        return Ok(user);
    }

    [HttpGet]
    [Route("getall")]
    public async Task<ActionResult<IEnumerable<User>>> GetAll([FromQuery] int pageNumber, [FromQuery] int pageSize)
    {
        var users = await _userService.GetAllAsync(pageNumber, pageSize);

        return Ok(users);
    }

    [HttpPut]
    [Route("update")]
    public async Task<ActionResult> Update([FromBody] UpdateUserRequest updateUserRequest)
    {
        var user = updateUserRequest.ToDomain();

        var isUpdated = await _userService.UpdateAsync(user);

        if (!isUpdated)
            return BadRequest();

        return NoContent();
    }

    [HttpDelete]
    [Route("delete/{id:int}")]
    public async Task<ActionResult> Delete([FromRoute] int id)
    {
        var deleted = await _userService.DeleteByIdAsync(id);
        if (!deleted)
            return NotFound();

        return Ok();
    }
}