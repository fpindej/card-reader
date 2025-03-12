using CardReader.Application.Services;
using CardReader.WebApi.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace CardReader.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    public async Task<ActionResult<int>> CreateUser([FromBody] CreateUserRequest request)
    {
        var userId = await _userService.CreateUserAsync(request.FirstName, request.LastName, request.Email);

        if (userId is null)
        {
            return BadRequest(new { message = "User could not be created." });
        }

        return Ok(userId);
    }
}