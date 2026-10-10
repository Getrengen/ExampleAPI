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

    public string? LoginUser(LoginAppUser user)
    {
        if(string.IsNullOrWhiteSpace(user.Login)|| string.IsNullOrWhiteSpace(user.Password)) 
            return null;
        
        AppUser? logUser = _context.AppUsers
            .Include(u => u.Role)
            .FirstOrDefault(u => u.Login == user.Login);
        
        if (logUser == null) 
            return null;
        
        if(!HashingService.CheckPassword(logUser, user.Password))
            return null;
        
        return JWTService.CreateToken(logUser);
    }

    public bool RegUser(RegAppUser user)
    {
        if(string.IsNullOrWhiteSpace(user.Login)
           || string.IsNullOrWhiteSpace(user.PassHash)
           ||string.IsNullOrWhiteSpace(user.FullName)) 
            return false;
        
        AppUser? newUser = new()
            {Login = user.Login, PassHash = user.PassHash, FullName = user.FullName, RoleID = 2};

        newUser = HashingService.HashPassword(newUser, user.PassHash);
        
        
        if(newUser == null) 
            return false;
        
        _context.AppUsers.Add(newUser);
        _context.SaveChanges();
        
        return true;
    } 
    
}