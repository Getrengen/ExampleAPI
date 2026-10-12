using ExampleAPI.Controllers;
using ExampleAPI.DTOs;
using ExampleAPI.Entities;
using ExampleAPI.Infrastructure;
using Microsoft.EntityFrameworkCore;
namespace ExampleAPI.Service;

public class AuthRegService
{
    private readonly MyDbContext _context;

    public AuthRegService(MyDbContext context)
    {
        _context = context;
       
    }

    public async Task<Result<JwtTokinDTO>> LoginUser(LoginAppUser user)
    {
        if (string.IsNullOrWhiteSpace(user.Login) || string.IsNullOrWhiteSpace(user.Password))
            return new Result<JwtTokinDTO>()
            {
                Status = ResultStatus.InvalidData,
                Message = "Invalid Login or Password"
            };

        AppUser? logUser = await _context.AppUsers
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Login == user.Login);

        if (logUser == null ||
            !HashingService.CheckPassword(logUser, user.Password))
            return new Result<JwtTokinDTO>
            {
                Status = ResultStatus.InvalidData,
                Message = "Неверный логин или пароль"
            };

        return new Result<JwtTokinDTO>()
        {
            Status = ResultStatus.Success,
            Data = new JwtTokinDTO(JWTService.CreateToken(logUser), "Bearer")
        };

    ;
    }

    public async Task<Result<int>> RegUser(RegAppUser user)
    {
        if(string.IsNullOrWhiteSpace(user.Login)
           || string.IsNullOrWhiteSpace(user.PassHash)
           ||string.IsNullOrWhiteSpace(user.FullName)) 
            return new Result<int>()
            {
                Status = ResultStatus.InvalidData,
                Message = "Что-то не так с логином, паролем или именем"
            };
        
        if(await _context.AppUsers.AnyAsync(ap => ap.Login == user.Login))
            return new Result<int>()
            {
                Status = ResultStatus.Conflict,
                Message = "Логин уже есть"
            };
        
        AppUser? newUser = new()
            {Login = user.Login, PassHash = user.PassHash, FullName = user.FullName, RoleID = 2};
        
        if(newUser == null) 
            return new Result<int>()
            {
                Status = ResultStatus.Conflict,
                Message = "Техническая ошибка"
            };

        newUser = HashingService.HashPassword(newUser, user.PassHash);
        
        _context.AppUsers.Add(newUser);
        await _context.SaveChangesAsync();
        
        return new Result<int>()
        {
            Status = ResultStatus.Success,
            Data = newUser.ID
        };
    } 
    
}