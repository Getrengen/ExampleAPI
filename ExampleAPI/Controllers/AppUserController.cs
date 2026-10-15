using ExampleAPI.DTOs.AppUserDTOs;
using ExampleAPI.Entities;
using ExampleAPI.Service.EntitiesService.AppUserS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExampleAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AppUserController : ControllerBase
{
    IAppUserService _service;
    
    public AppUserController(IAppUserService service)
    {
        _service = service;
    }

    [HttpGet]
    [Authorize(Roles = "Администратор")]
    public async Task<IActionResult> Get()
    {
        var result = await _service.GetAllAsync();
        return result.ToActionResult();
    }

    [HttpGet("{userID}")]
    [Authorize(Roles = "Администратор")]
    public async Task<IActionResult> Get([FromRoute]int userID)
    {
        var result = await _service.GetByIdAsync(userID);
        return result.ToActionResult();
    }
    
    [HttpGet("Me")]
    [Authorize]
    public async Task<IActionResult> GetMe()
    {
        if (!int.TryParse(User.FindFirst("sub")?.Value, out int userId))
            return Unauthorized();
        
        var result = await _service.GetByIdAsync(userId);
        return result.ToActionResult();
    }

    [HttpPost]
    [Authorize(Roles = "Администратор")]
    public async Task<IActionResult> Create([FromBody] CreateAppUserDto user)
    {
        var result = await _service.CreateAsync(user);
        return result.ToActionResult();
    }

    [HttpPatch("Me/Password")]
    [Authorize]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
    {
        if (!int.TryParse(User.FindFirst("sub")?.Value, out int userId))
            return Unauthorized();
        
        var result = await _service.PatchUserPasswordAsync(userId, dto);
        return result.ToActionResult();
    }

    [HttpPut("{userID}")]
    [Authorize(Roles = "Администратор")]
    public async Task<IActionResult> Update([FromRoute]int userID, [FromBody] UpdateAppUserDto dto)
    {
        var result = await _service.UpdateAsync(userID, dto);
        return result.ToActionResult();
    }

    [HttpPatch("{userID}/Role")]
    [Authorize(Roles = "Администратор")]
    public async Task<IActionResult> ChangeUserRole([FromRoute] int userID, [FromQuery] int roleId)
    {
        var result = await _service.ChangeRoleAsync(userID, roleId);
        return result.ToActionResult();
    }
    
}