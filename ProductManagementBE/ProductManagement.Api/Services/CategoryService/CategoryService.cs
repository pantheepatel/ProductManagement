namespace ProductManagement.Api.Services.CategoryService;

public class CategoryService : ICategoryService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<Category> _categoryRepo;

    public CategoryService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _categoryRepo = _unitOfWork.GetRepository<Category>();
    }

    public async Task<IEnumerable<Category>> GetAllCategoryAsync()
    {
        return await _categoryRepo.GetAllAsync();
    }

    public async Task<CategoryDto?> GetCategoryByIdAsync(int id)
    {
        var category = await _categoryRepo.GetByIdAsync(id);
        if (category == null)
        {
            return null;
        }
        return new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description
        };
    }

    public async Task<CategoryDto> CreateCategoryAsync(CategoryCreateDto dto)
    {
        var category = new Category
        {
            Name = dto.Name,
            Description = dto.Description
        };
        await _categoryRepo.AddAsync(category);
        await _unitOfWork.CompleteAsync();
        return new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description
        };
    }

    public async Task<bool> UpdateCategoryAsync(int id, CategoryDto categoryDto)
    {
        var category = await _categoryRepo.GetByIdAsync(id);
        if (category == null)
            return false;

        category.Name = categoryDto.Name;
        category.Description = categoryDto.Description;

        _categoryRepo.Update(category);
        await _unitOfWork.CompleteAsync();
        return true;
    }

    public async Task<bool> DeleteCategoryAsync(int id)
    {
        var category = await _categoryRepo.GetByIdAsync(id);
        if (category == null)
            return false;

        _categoryRepo.Remove(category);
        await _unitOfWork.CompleteAsync();
        return true;
    }
}
