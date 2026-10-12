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

    [HttpPut("Product/{id}")]
    public async Task<IActionResult> UpdateProduct([FromRoute] int id, [FromBody] AddGiveProduct product)
    {
        var result = await _service.PutProduct(id, product);
        return result.ToActionResult();
    }

    [HttpPatch("Product/{id}")]
    public async Task<IActionResult> PatchProduct([FromRoute] int id, [FromBody] PatchProductDTO product)
    {
        var result = await _service.PatchProduct(id, product);

        return result.ToActionResult();
    }
    
    [HttpPost("AddProduct")]
    public async Task<IActionResult> AddProduct([FromBody] AddGiveProduct product)
    {
        var result = await _service.AddProduct(product);
        return result.ToActionResult();
    }
}
//Для теста есть менеджер Test
//Login: Manager
//Password: Test1