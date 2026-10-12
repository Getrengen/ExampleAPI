using ExampleAPI.DTOs;
using ExampleAPI.Entities;
using ExampleAPI.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace ExampleAPI.Service;

public class UserService
{
    private readonly MyDbContext _dbContext;

    public UserService(MyDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<List<AddGiveProduct>>> GetAllProducts()
    {
        var allProducts = await _dbContext.Products
            .Select(p => new AddGiveProduct(
                p.Name,
                (int)p.Price,
                p.Description))
            .ToListAsync();

        return new Result<List<AddGiveProduct>>
        {
            Status = ResultStatus.Success,
            Data = allProducts
        };
    }

    public Result<TakeInfoOfAppUser> GetMe(int userID)
    {
        var UserInfo = _dbContext.AppUsers
            .Where(u => u.ID == userID)
            .Select(u => new TakeInfoOfAppUser(
                u.FullName,
                u.Login,
                u.Role.Name)).FirstOrDefault();
        
        if (UserInfo == null)
            return new Result<TakeInfoOfAppUser>()
            {
                Status = ResultStatus.NotFound,
                Message = "Пользователь не найден"
            };
        
        return new Result<TakeInfoOfAppUser>()
        {
            Status = ResultStatus.Success,
            Data = UserInfo
        };
    }

    public Result<List<GetOrder>> GetUserOrders(int userID)
    {
        List<GetOrder> orders = _dbContext.Orders
            .Where(u => u.UserID == userID)
            .Select(o => new GetOrder(
                o.CreatedDate,
                o.OrderProducts
                    .Select(Op => new GetOrderProduct(
                        Op.Product.Name,
                        Op.Quantity,
                        Op.Product.Price)).ToList()
            )).ToList();

        return new Result<List<GetOrder>>
        {
            Status = ResultStatus.Success,
            Data = orders
        };
    }
    
    public Result<int> PostNewOrder(List<ProductsInOrder> productsID, int userId)
    {
        if (productsID == null || productsID.Count == 0)
            return new Result<int>
            {
                Status = ResultStatus.InvalidData,
                Message = "Заказ пустой"
            };

        if (userId <= 0 || !_dbContext.AppUsers.Any(u => u.ID == userId))
            return new Result<int>
            {
                Status = ResultStatus.NotFound,
                Message = "Пользователь не найден"
            };

        if (productsID.Any(p => p.Quantity <= 0))
            return new Result<int>
            {
                Status = ResultStatus.InvalidData,
                Message = "Количество товара должно быть больше нуля"
            };
        
        var productIds = productsID.Select(p => p.ProductId).ToList();
        
        if (_dbContext.Products.Count(p => productIds.Contains(p.ID)) != productIds.Count)
            return new Result<int>
            {
                Status = ResultStatus.NotFound,
                Message = "Один или несколько товаров не найдены"
            };
        
        Order NewOrder = new Order()
        {
            UserID = userId,
            OrderProducts = new List<OrderProduct>(),
            CreatedDate = DateTime.Now
        };
        foreach (var productsInOrder in productsID)
            NewOrder.OrderProducts.Add(new OrderProduct
            {
                ProductID = productsInOrder.ProductId,
                Quantity = productsInOrder.Quantity,
            });
        
        _dbContext.Orders.Add(NewOrder);
        
        _dbContext.SaveChanges();
        
        return new Result<int>()
        {
            Status = ResultStatus.Success,
            Data = NewOrder.ID
        };
    }
    
}