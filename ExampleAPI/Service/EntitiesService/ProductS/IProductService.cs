using ExampleAPI.DTOs.ProductDTOs;
using ExampleAPI.Entities;

namespace ExampleAPI.Service.EntitiesService.ProductS;

public interface IProductService
{
    Task<Result<List<ProductDto>>> GetAllAsync();
    Task<Result<ProductDto>> GetByIdAsync(int productID);
    Task<Result<List<ProductDto>>> GetAllByNameAsync(string name);
    
    Task<Result<int>> PostAsync(ProductCreateDto newProduct);
    
    Task<Result<ProductDto>> PutAsync(int productID, ProductCreateDto newProduct);
    Task<Result<ProductDto>> PatchAsync(int productID, ProductPatchDto patchProduct);
}


