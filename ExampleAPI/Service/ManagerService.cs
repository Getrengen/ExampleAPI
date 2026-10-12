using ExampleAPI.DTOs;
using ExampleAPI.Entities;
using ExampleAPI.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace ExampleAPI.Service;

public class ManagerService
{
    private readonly MyDbContext _dbContext;

    public ManagerService(MyDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<AddGiveProduct>> PutProduct(int productId, AddGiveProduct  product)
    {
        Product? productToPut = await _dbContext.Products.FirstOrDefaultAsync(p => p.ID == productId);
        
        if (productToPut == null) 
            return new Result<AddGiveProduct>()
            {
                Status = ResultStatus.NotFound,
                Message = "Продукта с таким ID не существует"
            };

        if(string.IsNullOrWhiteSpace(product.Description)
           || string.IsNullOrWhiteSpace(product.Name) || product.Price == null
           || product.Price <= 0) 
            return new Result<AddGiveProduct>()
            {
                Status = ResultStatus.InvalidData,
                Message = "Данные для изменения не должны быть пустыми",
            };
        
        productToPut.Name = product.Name;
        productToPut.Price = product.Price;
        productToPut.Description = product.Description;

        await _dbContext.SaveChangesAsync();

        return new Result<AddGiveProduct>()
        {
            Status = ResultStatus.Success,
            Data = new AddGiveProduct(productToPut.Name, (int)productToPut.Price, productToPut.Description)
        };
    }

    public async Task<Result<AddGiveProduct>> PatchProduct(int productId, PatchProductDTO product)
    {
        Product? productToPatch = await _dbContext.Products
            .FirstOrDefaultAsync(p => p.ID == productId);

        if (productToPatch == null)
            return new Result<AddGiveProduct>
            {
                Status = ResultStatus.NotFound,
                Message = "Товар не найден"
            };

        if (product.Name != null)
        {
            if (string.IsNullOrWhiteSpace(product.Name))
                return new Result<AddGiveProduct>
                {
                    Status = ResultStatus.InvalidData,
                    Message = "Название не может быть пустым"
                };

            productToPatch.Name = product.Name;
        }

        if (product.Price.HasValue)
        {
            if (product.Price.Value <= 0)
                return new Result<AddGiveProduct>
                {
                    Status = ResultStatus.InvalidData,
                    Message = "Цена должна быть больше нуля"
                };

            productToPatch.Price = product.Price.Value;
        }

        if (product.Description != null)
            productToPatch.Description = product.Description;

        await _dbContext.SaveChangesAsync();

        return new Result<AddGiveProduct>
        {
            Status = ResultStatus.Success,
            Data = new AddGiveProduct(
                productToPatch.Name,
                (int)productToPatch.Price,
                productToPatch.Description)
        };
    }
    
    public async Task<Result<int>> AddProduct(AddGiveProduct product)
    {
        if (string.IsNullOrWhiteSpace(product.Description)
            || string.IsNullOrWhiteSpace(product.Name) || product.Price == null
            || product.Price <= 0)
            return new Result<int>()
            {
                Status = ResultStatus.InvalidData,
                Message = "Все поля товара должны быть заполнены, а цена должна быть больше нуля",
            };
        
        Product newProduct = new Product()
        {
            Name = product.Name,
            Price = product.Price,
            Description = product.Description,
        };
        
        _dbContext.Products.Add(newProduct);
        await _dbContext.SaveChangesAsync();
        
        return new Result<int>()
        {
            Status = ResultStatus.Success,
            Data = newProduct.ID,
        };
    }
    
}