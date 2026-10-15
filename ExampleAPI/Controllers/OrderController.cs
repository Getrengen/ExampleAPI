using ExampleAPI.DTOs.OrderDTOs;
using ExampleAPI.Entities;
using ExampleAPI.Service.EntitiesService.OrderS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExampleAPI.Controllers;


[ApiController]
[Route("/[controller]")]
[Authorize]
public class OrderController: ControllerBase
{
    IOrderService _service;
    
    public OrderController(IOrderService service)
    {
        _service = service;
    }

    [HttpGet("GetAll")]
    [Authorize(Roles = "Менеджер")]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllAsync();
        return result.ToActionResult();
    }
    
    [HttpGet("GetBy/{id}")]
    [Authorize(Roles = "Менеджер")]
    public async Task<IActionResult> GetAll(int id)
    {
        var result = await _service.GetByIdAsync(id);
        return result.ToActionResult();
    }

    [HttpGet("AllMy")]
    public async Task<IActionResult> GetAllByUser()
    {
        if (!int.TryParse(User.FindFirst("sub")?.Value, out int userId))
            return Unauthorized();
        
        var result = await _service.GetByUserIdAsync(userId);
        return result.ToActionResult();
    }

    [HttpPost("CreateNew")]
    public async Task<IActionResult> CreateList([FromBody] List<PostOrderDto> Products)
    {
        if (!int.TryParse(User.FindFirst("sub")?.Value, out int userId))
            return Unauthorized();

        var result = await _service.PostListAsync(userId, Products);
        return  result.ToActionResult();
    }
}