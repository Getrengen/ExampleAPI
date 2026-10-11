using ExampleAPI.DTOs;
using ExampleAPI.Entities;
using ExampleAPI.Infrastructure;

namespace ExampleAPI.Service;

public class UserService
{
    private readonly MyDbContext _dbContext;

    public UserService(MyDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public List<AddGiveProduct> GetAllProducts()
    {
        return _dbContext.Products
            .Select(u => new AddGiveProduct(
                u.Name,
                u.Price,
                u.Description)).ToList();
    }

    public TakeInfoOfAppUser GetMe(int userID)
    {
        return _dbContext.AppUsers
            .Where(u => u.ID == userID)
            .Select(u => new TakeInfoOfAppUser(
                u.FullName,
                u.Login,
                u.Role.Name)).FirstOrDefault();
    }

    public List<GetOrder> GetUserOrders(int userID)
    {
        return _dbContext.Orders
            .Where(u => u.UserID == userID)
            .Select(o => new GetOrder(
                o.CreatedDate,
                o.OrderProducts
                    .Select(Op => new GetOrderProduct(
                        Op.Product.Name,
                        Op.Quantity,
                        Op.Product.Price)).ToList()
            )).ToList();
    }
    
    public bool PostNewOrder(List<ProductsInOrder> productsID, int userId)
    {
        if (!productsID.Any() || userId <= 0 ||
            !_dbContext.AppUsers.Any(u => u.ID == userId))
            return false;
        
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
                OrderID = NewOrder.ID
            });
        _dbContext.Orders.Add(NewOrder);
        _dbContext.SaveChanges();
        
        return true;
    }
    
}