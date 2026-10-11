using ExampleAPI.DTOs;
using ExampleAPI.Entities;
using ExampleAPI.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExampleAPI.Controllers;


[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Менеджер")]
public class ManagerController : ControllerBase
{
    private readonly ManagerService _service;
    
    public ManagerController(ManagerService service)
    {
        _service = service;
    }

    [HttpPatch("Product/{id}")]
    public async Task<IActionResult> UpdateProduct([FromRoute] int id, [FromBody] AddGiveProduct product)
    {
        if(_service.PatchProduct(id, product))
            return Ok();
        return BadRequest();
    }

    [HttpPost("AddProduct")]
    public async Task<IActionResult> AddProduct([FromBody] AddGiveProduct product)
    {
        if(_service.AddProduct(product))
            return Ok();
        return BadRequest();
    }
}