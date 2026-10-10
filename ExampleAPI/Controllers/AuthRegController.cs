using ExampleAPI.DTOs;
using ExampleAPI.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExampleAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthRegController : ControllerBase
{
    private readonly AuthRegService _authRegService;
    
    public AuthRegController(AuthRegService authRegService)
    {
        _authRegService = authRegService;
    }

    [HttpPost("Login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromQuery] LoginAppUser user)
    {
        string? token = _authRegService.LoginUser(user);
        
        if (token == null) 
            return Unauthorized();
        
        return Ok(new
        {
            access_token = token,
            token_type = "Bearer"
        });
    }

    [HttpPost("Register")]
    [AllowAnonymous]
    public async Task<IActionResult> Registration([FromForm] RegAppUser user)
    {
        if(_authRegService.RegUser(user))
            return Ok();
        
        return BadRequest();
    }
}

