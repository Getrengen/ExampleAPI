using ExampleAPI.DTOs;
using ExampleAPI.Entities;
using ExampleAPI.Infrastructure;

namespace ExampleAPI.Service;

public class ManagerService
{
    private readonly MyDbContext _dbContext;

    public ManagerService(MyDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public bool PatchProduct(int productId, AddGiveProduct  product)
    {
        Product? productToPatch = _dbContext.Products.FirstOrDefault(p => p.ID == productId);
        
        if (productToPatch == null) return false;

        if(string.IsNullOrWhiteSpace(product.Description)
           || string.IsNullOrWhiteSpace(product.Name) 
           || product.Price <= 0) 
            return false;
        
        productToPatch.Name = product.Name ?? productToPatch.Name;
        productToPatch.Price = product.Price ?? productToPatch.Price;
        productToPatch.Description = product.Description ?? productToPatch.Description;

        _dbContext.SaveChanges();

        return true;
    }

    public bool AddProduct(AddGiveProduct product)
    {
        if (string.IsNullOrWhiteSpace(product.Description)
            ||string.IsNullOrWhiteSpace(product.Name) || product.Price == null
            || product.Price <= 0)
            return false;
        
        Product newProduct = new Product()
        {
            Name = product.Name,
            Price = product.Price,
            Description = product.Description,
        };
        _dbContext.Products.Add(newProduct);
        _dbContext.SaveChanges();
        return true;
    }
    
}