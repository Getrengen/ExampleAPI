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

    public bool PatchUserRole(int userID, int newRoleID)
    {
        AppUser? g = _dbContext.AppUsers.FirstOrDefault(u=>u.ID == userID);
        if (g == null) return false;
        g.RoleID = newRoleID;
        _dbContext.SaveChanges();
        return true;
    }

    public bool PutUser(int userID, PutAppUser user)
    {
        AppUser? g = _dbContext.AppUsers.FirstOrDefault(u=>u.ID == userID);
        if (g == null) return false;
        g.FullName = user.FullName?? g.FullName;
        g.Login = user.Login?? g.Login;
        g.RoleID = user.RoleID ?? g.RoleID;
        _dbContext.SaveChanges();
        return true;
    }
    
}