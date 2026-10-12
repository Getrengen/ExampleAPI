using ExampleAPI.DTOs;
using ExampleAPI.Entities;
using ExampleAPI.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExampleAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Администратор")]
public class AdminController: ControllerBase
{
    
    private readonly AdminService _service;
    public AdminController(AdminService service)
    {
        _service = service;
    }
    
    [HttpGet("GetAllUsers")]
    public async Task<IActionResult> GetAllUser()
    {
        var result = await _service.GetAllUser();
        return result.ToActionResult();
    }

    [HttpPatch("UpdateUserRole")]
    public async Task<IActionResult> PatchUser([FromQuery]int userID, [FromQuery]int newRoleID)
    {
        var result = await _service.PatchUserRole(userID, newRoleID);
        return result.ToActionResult();
    }
    
    [HttpPut("UpdateUser")]
    public async Task<IActionResult> PutUser([FromQuery]int userID, [FromBody]PutAppUser newRoleID)
    {
        var result = await _service.PutUser(userID, newRoleID);
        return result.ToActionResult();
    }
    
    
}

//Для теста есть админ Test
//Login: Test
//Password: Test1
