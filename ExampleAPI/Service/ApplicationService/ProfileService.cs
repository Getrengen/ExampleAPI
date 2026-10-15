using ExampleAPI.DTOs;
using ExampleAPI.Entities;
using ExampleAPI.Service.EntitiesService.AppUserS;
using ExampleAPI.Service.EntitiesService.OrderS;

namespace ExampleAPI.Service.ApplicationService;

/// <summary>
/// Сервис-оркестратор, что объединяющий два сервиса
/// для выполнения операций, связанных с профилем пользователя
/// </summary>
public class ProfileService
{
    private readonly IAppUserService _userService;
    private readonly IOrderService _orderService;

    public ProfileService(IAppUserService userService, IOrderService orderService)
    {
        _userService = userService;
        _orderService = orderService;
    }
    
    public async Task<Result<ProfileDto>> GetProfileAsync(int userID)
    {
        
        
        
        var user = await _userService.GetByIdAsync(userID);

        if (user.Status != ResultStatus.Success || user.Data == null)
            return new Result<ProfileDto>
            {
                Status = user.Status,
                Message = user.Message
            };

        var orders = await _orderService.GetByUserIdAsync(userID);

        if (orders.Status != ResultStatus.Success || orders.Data == null)
            return new Result<ProfileDto>
            {
                Status = orders.Status,
                Message = orders.Message
            };

        return new Result<ProfileDto>
        {
            Status = ResultStatus.Success,
            Data = new ProfileDto(user.Data, orders.Data)
        };
    }
    
}

