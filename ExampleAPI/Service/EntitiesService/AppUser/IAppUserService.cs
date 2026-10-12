using ExampleAPI.DTOs;
using ExampleAPI.Entities;

namespace ExampleAPI.Service.EntitiesService;

public interface IAppUserService
{
    Task<Result<List<TakeInfoOfAppUser>>> GetAllUser();
    Task<Result<TakeInfoOfAppUser>> PatchUserRole(int userID, int newRoleID);
    Task<Result<TakeInfoOfAppUser>> PutUser(int userID, PutAppUser user);
    
}