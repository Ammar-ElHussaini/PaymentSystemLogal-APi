using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using PaymentSystem.DTOs;
using PaymentSystem.Services.Imp;

[ApiController]
[Route("api/[controller]")]
public class UserCo : ControllerBase
{
    private readonly UserService _userService;

    public UserCo(UserService userService)
    {
        _userService = userService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto model)
    {
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError(string.Empty, "Invalid input data.");
            return BadRequest(ModelState);
        }

        var token = await _userService.LoginAsync(model);

        if (token == null)
        {
            ModelState.AddModelError(string.Empty, "Invalid username or password.");
            return BadRequest(ModelState);
        }

        Response.Cookies.Append("access_token", token, new CookieOptions
        {
            HttpOnly = true,
            Secure = true, // Set to true if using HTTPS
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddMinutes(15)
        });

        return Ok("Logged in successfully.");
    }


    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto model)
    {
        
        var token = await _userService.RegisterAsync(
            model
        );

        if (token == null)
        {
            ModelState.AddModelError(string.Empty, "Users already exists.");
            return Conflict(ModelState);
        }

        Response.Cookies.Append("access_token", token, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddMinutes(15)
        });

        return Ok(token);
    }

}
