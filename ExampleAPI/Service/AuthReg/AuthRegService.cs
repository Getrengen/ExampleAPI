using ExampleAPI.Controllers;
using ExampleAPI.DTOs;
using ExampleAPI.DTOs.AppUserDTOs;
using ExampleAPI.Entities;
using ExampleAPI.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace ExampleAPI.Service.AuthReg;

public class AuthRegService : IAuthRegService
{
    private readonly MyDbContext _context;

    public AuthRegService(MyDbContext context)
    {
        _context = context;
       
    }

    public async Task<Result<JwtTokinDto>> LoginUser(LoginUserDto user)
    {
        if (string.IsNullOrWhiteSpace(user.Login) || string.IsNullOrWhiteSpace(user.Password))
            return new Result<JwtTokinDto>()
            {
                Status = ResultStatus.InvalidData,
                Message = "Врата распахнутся лишь для того, кто вознесёт логин и пароль, словно две части одной истины."
            };

        AppUser? logUser = await _context.AppUsers
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Login == user.Login);

        if (logUser == null ||
            !HashingService.CheckPassword(logUser, user.Password))
            return new Result<JwtTokinDto>
            {
                Status = ResultStatus.InvalidData,
                Message = "Неверный логин или пароль"
            };

        return new Result<JwtTokinDto>()
        {
            Status = ResultStatus.Success,
            Data = new JwtTokinDto(JWTService.CreateToken(logUser), "Bearer")
        };

    ;
    }

    public async Task<Result<int>> RegUser(CreateAppUserDto user)
    {
        if(string.IsNullOrWhiteSpace(user.Login)
           || string.IsNullOrWhiteSpace(user.Password)
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
            {Login = user.Login, PassHash = user.Password, FullName = user.FullName, RoleID = 2};
        
        if(newUser == null) 
            return new Result<int>()
            {
                Status = ResultStatus.Conflict,
                Message = "Техническая ошибка"
            };

        HashingService.HashPassword(newUser, user.Password);
        
        _context.AppUsers.Add(newUser);
        await _context.SaveChangesAsync();
        
        return new Result<int>()
        {
            Status = ResultStatus.Success,
            Data = newUser.ID
        };
    }

    public async Task<Result<int>> PatchUserPasswordAsync(int userID, string oldPassword, string newPassword)
    {

            AppUser? patchUser = await _context.AppUsers.FirstOrDefaultAsync(u => u.ID == userID);

            if (patchUser == null )
                return new Result<int>()
                {
                    Status = ResultStatus.NotFound,
                    Message = "Пользователь под таким ID не найден"
                };

            if (!HashingService.CheckPassword(patchUser, oldPassword))
                return new Result<int>()
                {
                    Status = ResultStatus.InvalidData,
                    Message = "Пароль не подходит"
                };
        
            HashingService.HashPassword(patchUser, newPassword);
        
            await _context.SaveChangesAsync();

            return new Result<int>()
            {
                Status = ResultStatus.Success,
                Data = patchUser.ID
            };
        
    }
}