using ExampleAPI.DTOs.OrderDTOs;
using ExampleAPI.Entities;

namespace ExampleAPI.Service.EntitiesService.OrderS;

public interface IOrderService
{
    Task<Result<List<OrderDto>>> GetAllAsync();
    Task<Result<OrderDto>> GetByIdAsync(int OrderID);
    Task<Result<List<OrderDto>>> GetByUserIdAsync(int userID);
    Task<Result<int>> PostListAsync(int userID, List<PostOrderDto> Products);
    
}

