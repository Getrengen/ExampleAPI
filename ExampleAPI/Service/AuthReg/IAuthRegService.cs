using ExampleAPI.Controllers;
using ExampleAPI.DTOs;
using ExampleAPI.DTOs.AppUserDTOs;
using ExampleAPI.Entities;

namespace ExampleAPI.Service.AuthReg;

public interface IAuthRegService
{
    Task<Result<JwtTokinDto>> LoginUser(LoginUserDto user);
    Task<Result<int>> RegUser(CreateAppUserDto user);
    
    
}