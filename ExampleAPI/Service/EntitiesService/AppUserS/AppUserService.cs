using ExampleAPI.DTOs;
using ExampleAPI.DTOs.AppUserDTOs;
using ExampleAPI.Entities;
using ExampleAPI.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace ExampleAPI.Service.EntitiesService.AppUserS;

public class AppUserService : IAppUserService
{
    private MyDbContext _dbContext;

    public AppUserService(MyDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<List<AppUserDto>>> GetAllAsync()
    {
        List<AppUserDto> listUsers = await _dbContext.AppUsers
            .Select(u => new AppUserDto(
                u.ID,
                u.Login,
                u.FullName,
                u.Role.Name)).ToListAsync();

        return new Result<List<AppUserDto>>()
        {
            Status = ResultStatus.Success,
            Data = listUsers
        };
    }

    public async Task<Result<AppUserDto>> GetByIdAsync(int userID)
    {
        var findUser = await _dbContext.AppUsers
            .Where(u => u.ID == userID)
            .Select(u => new AppUserDto(
                u.ID,
                u.Login,
                u.FullName,
                u.Role.Name)).FirstOrDefaultAsync();

        if (findUser == null)
            return new Result<AppUserDto>()
            {
                Status = ResultStatus.NotFound,
                Message = "Пользователь не найден"
            };

        return new Result<AppUserDto>()
        {
            Status = ResultStatus.Success,
            Data = findUser
        };
    }

    public async Task<Result<int>> CreateAsync(CreateAppUserDto user)
    {
        if (string.IsNullOrWhiteSpace(user.Login)
            || string.IsNullOrWhiteSpace(user.Password)
            || string.IsNullOrWhiteSpace(user.FullName))
            return new Result<int>()
            {
                Status = ResultStatus.InvalidData,
                Message = "Что-то не так с логином, паролем или именем"
            };

        if (await _dbContext.AppUsers.AnyAsync(ap => ap.Login == user.Login))
            return new Result<int>()
            {
                Status = ResultStatus.Conflict,
                Message = "Логин уже есть"
            };

        AppUser? newUser = new()
            { Login = user.Login, PassHash = user.Password, FullName = user.FullName, RoleID = 2 };

        if (newUser == null)
            return new Result<int>()
            {
                Status = ResultStatus.Conflict,
                Message = "Техническая ошибка"
            };

        newUser = HashingService.HashPassword(newUser, user.Password);

        _dbContext.AppUsers.Add(newUser);
        await _dbContext.SaveChangesAsync();

        return new Result<int>()
        {
            Status = ResultStatus.Success,
            Data = newUser.ID
        };
    }

    public async Task<Result<AppUserDto>> UpdateAsync(int userID, UpdateAppUserDto user)
    {
        if (string.IsNullOrWhiteSpace(user.Login) || string.IsNullOrWhiteSpace(user.FullName))
            return new Result<AppUserDto>
            {
                Status = ResultStatus.InvalidData,
                Message = "Логин и ФИО обязательны"
            };

        if (await _dbContext.AppUsers.AnyAsync(u => u.Login == user.Login && u.ID != userID))
            return new Result<AppUserDto>
            {
                Status = ResultStatus.Conflict,
                Message = "Логин уже используется"
            };

        Role? role = await _dbContext.Roles.FirstOrDefaultAsync(r => r.ID == user.RoleID);

        if (role == null)
            return new Result<AppUserDto>
            {
                Status = ResultStatus.NotFound,
                Message = "Роль не найдена"
            };

        AppUser? putUser = _dbContext.AppUsers.FirstOrDefault(u => u.ID == userID);

        if (putUser == null)
            return new Result<AppUserDto>
            {
                Status = ResultStatus.NotFound,
                Message = "Пользователь под таким ID не найден"
            };

        putUser.FullName = user.FullName;
        putUser.Login = user.Login;
        putUser.RoleID = user.RoleID;

        await _dbContext.SaveChangesAsync();

        return new Result<AppUserDto>
        {
            Status = ResultStatus.Success,
            Data = new AppUserDto(putUser.ID, putUser.Login, putUser.FullName, putUser.Role.Name)
        };
    }

    public async Task<Result<AppUserDto>> ChangeRoleAsync(int userID, int newRoleID)
    {
        AppUser? patchUser = await _dbContext.AppUsers.FirstOrDefaultAsync(u => u.ID == userID);

        if (patchUser == null || !_dbContext.Roles.Any(r => r.ID == newRoleID))
            return new Result<AppUserDto>()
            {
                Status = ResultStatus.NotFound,
                Message = "Пользователь под таким ID не найден"
            };
        Role? role = await _dbContext.Roles
            .FirstOrDefaultAsync(r => r.ID == newRoleID);

        if (role == null)
            return new Result<AppUserDto>
            {
                Status = ResultStatus.NotFound,
                Message = "Роль не найдена"
            };

        patchUser.Role = role;
        await _dbContext.SaveChangesAsync();

        return new Result<AppUserDto>()
        {
            Status = ResultStatus.Success,
            Data = new AppUserDto(patchUser.ID, patchUser.Login, patchUser.FullName, role.Name)
        };
    }

    public async Task<Result<int>> PatchUserPasswordAsync(int userID, string oldPassword, string newPassword)
    {
        AppUser? patchUser = await _dbContext.AppUsers.FirstOrDefaultAsync(u => u.ID == userID);

        if (patchUser == null )
            return new Result<int>()
            {
                Status = ResultStatus.NotFound,
                Message = "Пользователь под таким ID не найден"
            };
        
        
        
        patchUser = HashingService.HashPassword(patchUser, newPassword);
        
        await _dbContext.SaveChangesAsync();

        return new Result<int>()
        {
            Status = ResultStatus.Success,
            Data = patchUser.ID
        };
    }
    
}