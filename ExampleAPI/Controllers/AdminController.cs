using ExampleAPI.DTOs;
using ExampleAPI.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExampleAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Администратор")]
public class AdminController : ControllerBase
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
}