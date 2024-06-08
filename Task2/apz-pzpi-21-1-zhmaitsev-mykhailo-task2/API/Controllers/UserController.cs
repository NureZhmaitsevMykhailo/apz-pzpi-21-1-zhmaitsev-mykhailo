using API.Core.Constants;
using API.Core.DTOs.User;
using API.Core.Entities;
using API.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController(IUserService userService, IJwtService jwtService) : ControllerBase
{
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IEnumerable<User>>> GetUsers()
    {
        var users = await userService.GetUsersAsync();
        return Ok(users);
    }
    
    [HttpGet("{id:int}")]
    [Authorize]
    public async Task<ActionResult<User>> GetUser(int id)
    {
        var user = await userService.GetUserAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        return Ok(user);
    }
    
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<User>> CreateUser([FromBody] CreateUserDto user)
    {
        try
        {
            await userService.CreateUserAsync(user);
            return Ok();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(500, "Internal Server Error");
        }
    }
    
    [HttpPut("{id:int}")]
    [Authorize]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserDto user)
    {

        try
        {
            await userService.UpdateUserAsync(id, user);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpDelete("{id:int}")]
    [Authorize]
    public async Task<IActionResult> DeleteUser(int id)
    {
        try
        {
            await userService.DeleteUserAsync(id);
            return NoContent();
        }
        catch (Exception)
        {
            return StatusCode(500, "Internal Server Error");
        }
    }
    
    [HttpPost("login")]
    public async Task<ActionResult<string>> Login([FromBody] UserLoginDto userLoginDto)
    {
        var user = await userService.LogInAsync(userLoginDto.Email, userLoginDto.Password);

        if (user == null)
        {
            return Unauthorized("Invalid email or password");
        }
        
        var token = jwtService.GenerateToken(user.Id, Role.UserRole);
        return Ok(token);
    }
    
    [HttpPost("signup")]
    public async Task<ActionResult> SignUp([FromBody] UserSignUpDto userSignUpDto)
    {
        try
        {
            var result = await userService.SignUpAsync(userSignUpDto);

            if (result)
            {
                return Ok("User registration successful");
            }

            return BadRequest("User with the same email already exists");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal Server Error: {ex.Message}");
        }
    }
}
