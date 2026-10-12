using ExampleAPI.DTOs;
using ExampleAPI.Entities;
using ExampleAPI.Infrastructure;
using ExampleAPI.Service.EntitiesService;
using Microsoft.EntityFrameworkCore;

namespace ExampleAPI.Service;

public class AdminService
{
    private readonly MyDbContext _dbContext;

    public AdminService(MyDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    
    public async Task<Result<List<TakeInfoOfAppUser>>> GetAllUser()
    {
        List<TakeInfoOfAppUser> listUsers = await _dbContext.AppUsers
            .Select(u => new TakeInfoOfAppUser(
                u.FullName,
                u.Login,
                u.Role.Name)).ToListAsync();
        return new Result<List<TakeInfoOfAppUser>>()
        {
            Status = ResultStatus.Success,
            Data = listUsers
        };
    }

    public async Task<Result<TakeInfoOfAppUser>> PatchUserRole(int userID, int newRoleID)
    {
        AppUser? patchUser = await _dbContext.AppUsers.FirstOrDefaultAsync(u=>u.ID == userID);
       
        if (patchUser == null || !_dbContext.Roles.Any(r => r.ID == newRoleID))
            return new Result<TakeInfoOfAppUser>()
            {
                Status = ResultStatus.NotFound,
                Message = "Пользователь под таким ID не найден"
            };
        Role? role = await _dbContext.Roles
            .FirstOrDefaultAsync(r => r.ID == newRoleID);

        if (role == null)
            return new Result<TakeInfoOfAppUser>
            {
                Status = ResultStatus.NotFound,
                Message = "Роль не найдена"
            };
        
        patchUser.Role = role;
        await _dbContext.SaveChangesAsync();
        
        return new Result<TakeInfoOfAppUser>()
        {
            Status = ResultStatus.Success,
            Data = new TakeInfoOfAppUser(patchUser.FullName, patchUser.Login, role.Name)
        };
    }

    public async Task<Result<TakeInfoOfAppUser>> PutUser(int userID, PutAppUser user)
    {
        if (string.IsNullOrWhiteSpace(user.Login) || string.IsNullOrWhiteSpace(user.FullName))
            return new Result<TakeInfoOfAppUser>
            {
                Status = ResultStatus.InvalidData,
                Message = "Логин и ФИО обязательны"
            };
        
        if (await _dbContext.AppUsers.AnyAsync(u => u.Login == user.Login && u.ID != userID))
            return new Result<TakeInfoOfAppUser>
            {
                Status = ResultStatus.Conflict,
                Message = "Логин уже используется"
            };

        Role? role = await _dbContext.Roles.FirstOrDefaultAsync(r => r.ID == user.RoleID);

        if (role == null)
            return new Result<TakeInfoOfAppUser>
            {
                Status = ResultStatus.NotFound,
                Message = "Роль не найдена"
            };
        
        AppUser? putUser = _dbContext.AppUsers.FirstOrDefault(u=>u.ID == userID);
        
        if (putUser == null)
            return new Result<TakeInfoOfAppUser>()
            {
                Status = ResultStatus.NotFound,
                Message = "Пользователь под таким ID не найден"
            };
        
        putUser.FullName = user.FullName;
        putUser.Login = user.Login;
        putUser.RoleID = user.RoleID;
        
        await _dbContext.SaveChangesAsync();
        
        return new Result<TakeInfoOfAppUser>()
        {
            Status = ResultStatus.Success,
            Data = new TakeInfoOfAppUser(putUser.FullName, putUser.Login, putUser.Role.Name)
        };
    }
    
}