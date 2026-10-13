using ExampleAPI.DTOs.AppUserDTOs;
using ExampleAPI.Entities;

namespace ExampleAPI.Service.EntitiesService.AppUserS;

public interface IAppUserService
{
    Task<Result<List<AppUserDto>>> GetAllAsync();
    Task<Result<AppUserDto>> GetByIdAsync(int userID);
    Task<Result<int>> CreateAsync(CreateAppUserDto dto);
    Task<Result<AppUserDto>> UpdateAsync(int userID, UpdateAppUserDto dto);
    Task<Result<AppUserDto>> ChangeRoleAsync(int userID, int dto);
    
    
    
    // В норм БД делать просто не активным
    // Task<Result<int>> DeleteAsync(int userId);
    
}
