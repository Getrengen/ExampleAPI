using ExampleAPI.DTOs;
using ExampleAPI.Entities;
using ExampleAPI.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace ExampleAPI.Service;

public class AdminService
{
    private readonly MyDbContext _dbContext;

    public AdminService(MyDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public List<TakeInfoOfAppUser> GetAllUser()
    {
        return _dbContext.AppUsers
            .Select(u => new TakeInfoOfAppUser(
                u.FullName,
                u.Login,
                u.Role.Name))
            .ToList();
    }
    
}