using ExampleAPI.Entities;
using ExampleAPI.Service.ApplicationService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExampleAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProfileController : ControllerBase
{
    private readonly ProfileService _service;

    public ProfileController(ProfileService service)
    {
        _service = service;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetProfile()
    {
        if (!int.TryParse(User.FindFirst("sub")?.Value, out int userID))
            return Unauthorized();

        var result = await _service.GetProfileAsync(userID);

        return result.ToActionResult();
    }
}