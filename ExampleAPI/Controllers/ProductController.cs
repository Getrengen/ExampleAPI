using ExampleAPI.DTOs.ProductDTOs;
using ExampleAPI.Entities;
using ExampleAPI.Service.EntitiesService.ProductS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExampleAPI.Controllers;


[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProductController: ControllerBase
{
    private readonly IProductService _productService;
    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        var result = await _productService.GetAllAsync();
        return result.ToActionResult();
    }
    
    [HttpGet("{id:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var result = await _productService.GetByIdAsync(id);
        return result.ToActionResult();
    }
    
    [HttpGet("Search")]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllByName([FromQuery] string name)
    {
        var result = await _productService.GetAllByNameAsync(name);
        return result.ToActionResult();
    }


    [HttpPost]
    [Authorize(Roles = "Менеджер")]
    public async Task<IActionResult> Create([FromBody] ProductCreateDto newProduct)
    {
        var result = await _productService.PostAsync(newProduct);
        return result.ToActionResult();
    }

    [HttpPut("{prodId:int}")]
    [Authorize(Roles = "Менеджер")]
    public async Task<IActionResult> Put([FromRoute] int prodId, [FromBody] ProductCreateDto newProduct)
    {
        var result = await _productService.PutAsync(prodId, newProduct);
        return result.ToActionResult();
    }
    
    [HttpPatch("{prodId:int}")]
    [Authorize(Roles = "Менеджер")]
    public async Task<IActionResult> Patch([FromRoute] int prodId, [FromBody] ProductPatchDto newProduct)
    {
        var result = await _productService.PatchAsync(prodId, newProduct);
        return result.ToActionResult();
    }
}