
using Microsoft.AspNetCore.Mvc;
using MovieTicketBooking.DTOs.Auth;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    // POST: api/Auth/register
    [HttpPost("register")]
    public async Task<IActionResult> Register(
        RegisterRequestDto dto)
    {
        try
        {
            var response = await _authService.RegisterAsync(dto);

            return Ok(response);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // POST: api/Auth/login
    [HttpPost("login")]
    public async Task<IActionResult> Login(
        LoginRequestDto dto)
    {
        var response = await _authService.LoginAsync(dto);

        if (response == null)
        {
            return Unauthorized(new
            {
                message = "Invalid email or password."
            });
        }

        return Ok(response);
    }
}
