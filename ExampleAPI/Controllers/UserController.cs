using System.Security.Claims;
using ExampleAPI.DTOs;
using ExampleAPI.Entities;
using ExampleAPI.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExampleAPI.Controllers;


[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UserController: ControllerBase
{
    private readonly UserService _service;
    public UserController(UserService service)
    {
        _service = service;
    }

    [HttpGet("GetAllProducts")]
    [AllowAnonymous]
    public IActionResult GetAllProducts()
    {
        return Ok(_service.GetAllProducts());
    }
    
    [HttpGet("GetMe")]
    public async Task<IActionResult> GetMe()
    {
        if (!int.TryParse(User.FindFirst("sub")?.Value, out int userId))
            return Unauthorized();
        
        var result = _service.GetMe(userId);

        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpGet("GetOrder")]
    public async Task<IActionResult> GetOrder()
    {
        if (!int.TryParse(User.FindFirst("sub")?.Value, out int userId))
            return Unauthorized();
        
        return Ok(_service.GetUserOrders(userId));
    }

    [HttpPost("CreateOrder")]
    public async Task<IActionResult> CreateOrder([FromBody] List<ProductsInOrder> order)
    {
        if (!int.TryParse(User.FindFirst("sub")?.Value, out int userId))
            return Unauthorized();
        
        bool isOk = _service.PostNewOrder(order, userId);
        
        if (isOk) 
            return Ok();
        
        return BadRequest();
    }
    
    
    
}
