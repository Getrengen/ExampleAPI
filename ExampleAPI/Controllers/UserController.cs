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
    public async Task<IActionResult> GetAllProducts()
    {
        var result = await _service.GetAllProducts();
        
        return result.ToActionResult();
    }
    
    [HttpGet("GetMe")]
    public IActionResult GetMe()
    {
        if (!int.TryParse(User.FindFirst("sub")?.Value, out int userId))
            return Unauthorized();
        
        var result = _service.GetMe(userId);

        return result.ToActionResult();
    }

    [HttpGet("GetOrder")]
    public IActionResult GetOrder()
    {
        if (!int.TryParse(User.FindFirst("sub")?.Value, out int userId))
            return Unauthorized();
        
        var result = _service.GetUserOrders(userId);
        
        return result.ToActionResult();
    }

    [HttpPost("CreateOrder")]
    public IActionResult CreateOrder([FromBody] List<ProductsInOrder> order)
    {
        if (!int.TryParse(User.FindFirst("sub")?.Value, out int userId))
            return Unauthorized();
        
        Result<int> result = _service.PostNewOrder(order, userId);
        
        return result.ToActionResult();
    }
    
    
}
