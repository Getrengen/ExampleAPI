using ExampleAPI.DTOs.ProductDTOs;
using ExampleAPI.Entities;
using ExampleAPI.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace ExampleAPI.Service.EntitiesService.ProductS;

public class ProductService : IProductService
{

    private MyDbContext _dbContext;

    public ProductService(MyDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<List<ProductDto>>> GetAllAsync()
    {
        var products = await _dbContext.Products
            .Select(u => new ProductDto(
                u.ID,
                u.Name,
                u.Price,
                u.Description)).ToListAsync();

        return new Result<List<ProductDto>>
        {
            Status = ResultStatus.Success,
            Data = products
        };
    }

    public async Task<Result<ProductDto>> GetByIdAsync(int productID)
    {
        var product = await _dbContext.Products.Where(p => p.ID == productID)
            .Select(u => new ProductDto(
                u.ID,
                u.Name,
                u.Price,
                u.Description)).FirstOrDefaultAsync();

        if (product == null)
            return new Result<ProductDto>
            {
                Status = ResultStatus.NotFound,
                Message = "Продукт под этим ID не найден"
            };

        return new Result<ProductDto>
        {
            Status = ResultStatus.Success,
            Data = product
        };
    }

    public async Task<Result<List<ProductDto>>> GetAllByNameAsync(string name)
    {
        var products = await _dbContext.Products
            .Where(p => p.Name.Contains(name, StringComparison.CurrentCultureIgnoreCase))
            .Select(u => new ProductDto(
                u.ID,
                u.Name,
                u.Price,
                u.Description)).ToListAsync();

        return new Result<List<ProductDto>>
        {
            Status = ResultStatus.Success,
            Data = products
        };
    }

    public async Task<Result<int>> PostAsync(ProductCreateDto newProduct)
    {
        if (string.IsNullOrWhiteSpace(newProduct.Name))
            return new Result<int>
            {
                Status = ResultStatus.InvalidData,
                Message = "Имя продукта не может быть пустым"
            };
        
        if (newProduct.Price < 0)
            return new Result<int>
            {
                Status = ResultStatus.InvalidData,
                Message = "Цена не может быть отрицательной"
            };
        
        if (await _dbContext.Products.AnyAsync(p => p.Name == newProduct.Name))
            return new Result<int>
            {
                Status = ResultStatus.Conflict,
                Message = "Имя товара уже занято"
            };

        var newProd = new Product
        {
            Name = newProduct.Name,
            Price = newProduct.Price,
            Description = newProduct.Description
        };
        
        _dbContext.Products.Add(newProd);
        await _dbContext.SaveChangesAsync();
        
        return new Result<int>
        {
            Status = ResultStatus.Success,
            Data = newProd.ID
        };
    }

    public async Task<Result<ProductDto>> PutAsync(int productID, ProductCreateDto newProduct)
    {
        if (string.IsNullOrWhiteSpace(newProduct.Name))
            return new Result<ProductDto>
            {
                Status = ResultStatus.InvalidData,
                Message = "Имя продукта не может быть пустым"
            };
        
        if (newProduct.Price < 0)
            return new Result<ProductDto>
            {
                Status = ResultStatus.InvalidData,
                Message = "Цена не может быть отрицательной"
            };
        
        Product? prod = await _dbContext.Products.FirstOrDefaultAsync(p=>p.ID == productID);
        
        if (prod == null)
            return new Result<ProductDto>
            {
                Status = ResultStatus.NotFound,
                Message = "Товара под таким ID не существует"
            };
        
        if (await _dbContext.Products.AnyAsync(p => p.Name == newProduct.Name && p.ID != productID))
            return new Result<ProductDto>
            {
                Status = ResultStatus.Conflict,
                Message = "Название товара уже занято"
            };
        
        prod.Name = newProduct.Name;
        prod.Price = newProduct.Price;
        prod.Description = newProduct.Description;
        
        _dbContext.Products.Update(prod);
        await _dbContext.SaveChangesAsync();
        
        return new Result<ProductDto>
        {
            Status = ResultStatus.Success,
            Data = new ProductDto(prod.ID, prod.Name, prod.Price, prod.Description)
        };
    }

    public async Task<Result<ProductDto>> PatchAsync(int productID, ProductPatchDto patchProduct)
    {
        if (patchProduct.Price < 0)
            return new Result<ProductDto>
            {
                Status = ResultStatus.InvalidData,
                Message = "Цена не может быть отрицательной"
            };
        
        Product? prod = await _dbContext.Products.FirstOrDefaultAsync(p=>p.ID == productID);
        
        if (prod == null)
            return new Result<ProductDto>
            {
                Status = ResultStatus.NotFound,
                Message = "Товара под таким ID не существует"
            };
        
        if (await _dbContext.Products.AnyAsync(p => p.Name == patchProduct.Name))
            return new Result<ProductDto>
            {
                Status = ResultStatus.Conflict,
                Message = "Имя товара уже занято"
            };
        
        if (patchProduct.Name != null && string.IsNullOrEmpty(patchProduct.Name))
            prod.Name = patchProduct.Name;
        
        if (patchProduct.Price != null && patchProduct.Price <= 0)
            prod.Price = (int)patchProduct.Price;
        
        if (patchProduct.Description != null && string.IsNullOrEmpty(patchProduct.Description))
            prod.Description = patchProduct.Description;
        
        _dbContext.Products.Update(prod);
        await _dbContext.SaveChangesAsync();
        
        return new Result<ProductDto>
        {
            Status = ResultStatus.Success,
            Data = new ProductDto(prod.ID, prod.Name, prod.Price, prod.Description)
        };
    }
}