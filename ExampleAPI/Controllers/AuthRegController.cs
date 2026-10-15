using ExampleAPI.DTOs;
using ExampleAPI.DTOs.AppUserDTOs;
using ExampleAPI.Entities;
using ExampleAPI.Service.AuthReg;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExampleAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthRegController : ControllerBase
{
    private readonly IAuthRegService _authRegService;
    
    public AuthRegController(IAuthRegService authRegService)
    {
        _authRegService = authRegService;
    }

    [HttpPost("Login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginUserDto user)
    {
        var result = await _authRegService.LoginUser(user);
        
        return result.ToActionResult();
    }

    [HttpPost("Register")]
    [AllowAnonymous]
    public async Task<IActionResult> Registration([FromBody] CreateAppUserDto user)
    {
        var result = await _authRegService.RegUser(user);
        
        return result.ToActionResult();
    }
    
    
}

