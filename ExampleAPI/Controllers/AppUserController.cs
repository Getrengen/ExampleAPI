using ExampleAPI.DTOs.AppUserDTOs;
using ExampleAPI.Entities;
using ExampleAPI.Service.EntitiesService.AppUserS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExampleAPI.Controllers;

[ApiController]
[Route("/[controller]")]
[Authorize]
public class AppUserController : ControllerBase
{
    IAppUserService _service;
    
    public AppUserController(IAppUserService service)
    {
        _service = service;
    }

    [HttpGet("Users")]
    [Authorize(Roles = "Администратор")]
    public async Task<IActionResult> Get()
    {
        var result = await _service.GetAllAsync();
        return result.ToActionResult();
    }

    [HttpGet("Users/{id}")]
    [Authorize(Roles = "Администратор")]
    public async Task<IActionResult> Get([FromQuery]int id)
    {
        var result = await _service.GetByIdAsync(id);
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

    [HttpPost("Register")]
    [Authorize(Roles = "Администратор")]
    public async Task<IActionResult> Registration([FromBody] CreateAppUserDto user)
    {
        var result = await _service.CreateAsync(user);
        return result.ToActionResult();
    }

    [HttpPatch("ChangePassword")]
    [Authorize]
    public async Task<IActionResult> Patch(string oldPassword, string newPassword)
    {
        if (!int.TryParse(User.FindFirst("sub")?.Value, out int userId))
            return Unauthorized();
        
        var result = await _service.PatchUserPasswordAsync(userId, oldPassword, newPassword);
        return result.ToActionResult();
    }

    [HttpPut("UpdateUser")]
    [Authorize(Roles = "Администратор")]
    public async Task<IActionResult> Update(int userID, [FromBody] UpdateAppUserDto dto)
    {
        var result = await _service.UpdateAsync(userID, dto);
        return result.ToActionResult();
    }

    [HttpPatch("ChangeUserRole")]
    [Authorize(Roles = "Администратор")]
    public async Task<IActionResult> ChangeUserRole(int userID, int dto)
    {
        var result = await _service.ChangeRoleAsync(userID, dto);
        return result.ToActionResult();
    }
    
}