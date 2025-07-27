namespace ProductManagement.Api.Services.ProductService;

public interface IProductService
{
    Task<IEnumerable<ProductDto>> GetAllProductsAsync();
    Task<ProductDto?> GetProductByIdAsync(int id);
    Task<int> CreateProductAsync(ProductCreateDto dto);
    Task<bool> UpdateProductAsync(int id, ProductDto dto);
    Task<bool> DeleteProductAsync(int id);
}
