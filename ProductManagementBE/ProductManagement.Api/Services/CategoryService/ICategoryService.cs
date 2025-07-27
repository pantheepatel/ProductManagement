namespace ProductManagement.Api.Services.CategoryService;

public interface ICategoryService
{
    Task<IEnumerable<Category>> GetAllCategoryAsync();
    Task<CategoryDto> CreateCategoryAsync(CategoryCreateDto dto);
    Task<CategoryDto?> GetCategoryByIdAsync(int id);
    Task<bool> UpdateCategoryAsync(int id, CategoryDto categoryDto);
    Task<bool> DeleteCategoryAsync(int id);
}
