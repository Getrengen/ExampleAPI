using ExampleAPI.DTOs;
using ExampleAPI.DTOs.OrderDTOs;
using ExampleAPI.Entities;
using ExampleAPI.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace ExampleAPI.Service.EntitiesService.OrderS;

public class OrderService  : IOrderService
{
    
    private MyDbContext _dbContext;

    public OrderService(MyDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<Result<List<OrderDto>>> GetAllAsync()
    {
        List<OrderDto> orders = await _dbContext.Orders
            .Select(o => new OrderDto(
                    o.ID,
                    o.UserID,
                    o.CreatedDate,
                    o.OrderProducts.Select(Op => new OrderProductDto(
                        Op.ProductID,
                        Op.Product.Name,
                        Op.Quantity,
                        Op.Product.Price)).ToList()
                )
            ).ToListAsync();
        return new Result<List<OrderDto>>
        {
            Status = ResultStatus.Success,
            Data = orders
        };
    }

    public async Task<Result<OrderDto>> GetByIdAsync(int OrderID)
    {
        OrderDto? order = await _dbContext.Orders.Where(u => u.ID == OrderID)
            .Select(o => new OrderDto(
                    o.ID,
                    o.UserID,
                    o.CreatedDate,
                    o.OrderProducts.Select(Op => new OrderProductDto(
                        Op.ProductID,
                        Op.Product.Name,
                        Op.Quantity,
                        Op.Product.Price)).ToList()
                )
            ).FirstOrDefaultAsync();

        if (order == null)
            return new Result<OrderDto>
            {
                Status = ResultStatus.NotFound,
                Message = "Заказ по ID не найден"
            };
        
        return new Result<OrderDto>
        {
            Status = ResultStatus.Success,
            Data = order
        };
    }

    public async Task<Result<List<OrderDto>>> GetByUserIdAsync(int userID)
    {
        List<OrderDto> orders = await _dbContext.Orders.Where(u => u.UserID == userID)
            .Select(o => new OrderDto(
                o.ID,
                o.UserID,
                o.CreatedDate,
                o.OrderProducts.Select(Op => new OrderProductDto(
                        Op.ProductID,
                        Op.Product.Name,
                        Op.Quantity,
                        Op.Product.Price)).ToList()
                )
            ).ToListAsync();

        return new Result<List<OrderDto>>
        {
            Status = ResultStatus.Success,
            Data = orders
        };
    }

    public async Task<Result<int>> PostListAsync(int userID, List<PostOrderDto> Products)
    {
        if (Products == null || Products.Count == 0)
            return new Result<int>
            {
                Status = ResultStatus.InvalidData,
                Message = "Заказ пустой"
            };

        if (userID <= 0 ||  !await _dbContext.AppUsers.AnyAsync(u => u.ID == userID))
            return new Result<int>
            {
                Status = ResultStatus.NotFound,
                Message = "Пользователь не найден"
            };

        if (Products.Any(p => p.Quantity <= 0))
            return new Result<int>
            {
                Status = ResultStatus.InvalidData,
                Message = "Количество товара должно быть больше нуля"
            };
        
        var productIds = Products.Select(p => p.ProductID).ToList();
        
        if (await _dbContext.Products.CountAsync(p => productIds.Contains(p.ID)) != productIds.Count)
            return new Result<int>
            {
                Status = ResultStatus.NotFound,
                Message = "Один или несколько товаров не найдены"
            };
        
        Order NewOrder = new Order()
        {
            UserID = userID,
            OrderProducts = new List<OrderProduct>(),
            CreatedDate = DateTime.Now
        };
        foreach (var productsInOrder in Products)
            NewOrder.OrderProducts.Add(new OrderProduct
            {
                ProductID = productsInOrder.ProductID,
                Quantity = productsInOrder.Quantity,
            });
        
        _dbContext.Orders.Add(NewOrder);
        
        await _dbContext.SaveChangesAsync();
        
        return new Result<int>()
        {
            Status = ResultStatus.Success,
            Data = NewOrder.ID
        };
        
    }
}