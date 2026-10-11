using ExampleAPI.DTOs;
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
        List<TakeInfoOfAppUser> userInfo = _service.GetAllUser();
        
        return Ok(userInfo);
    }

    [HttpPatch("UpdateUserRole")]
    public async Task<IActionResult> PatchUser([FromQuery]int userID, [FromQuery]int newRoleID)
    {
        if(_service.PatchUserRole(userID, newRoleID))
            return Ok();
        return BadRequest();
    }
    
    [HttpPut("UpdateUser")]
    public async Task<IActionResult> PutUser([FromQuery]int userID, [FromQuery]PutAppUser newRoleID)
    {
        if(_service.PutUser(userID, newRoleID))
            return Ok();
        return BadRequest();
    }
    
    
}

//Для теста есть админ Test
//Login: Test
//Password: Test1
