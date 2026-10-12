using ExampleAPI.DTOs;
using ExampleAPI.Entities;
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
    public async Task<IActionResult> Login([FromBody] LoginAppUser user)
    {
        var result = await _authRegService.LoginUser(user);
        
        return result.ToActionResult();
    }

    [HttpPost("Register")]
    [AllowAnonymous]
    public async Task<IActionResult> Registration([FromBody] RegAppUser user)
    {
        var result = await _authRegService.RegUser(user);
        
        return result.ToActionResult();
    }
}

