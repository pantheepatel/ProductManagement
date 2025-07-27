namespace ProductManagement.Tests.Services;

[TestFixture]
public class CategoryServiceTests
{
    private ProductDbContext _context;
    private UnitOfWork<ProductDbContext> _unitOfWork;
    private CategoryService _service;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ProductDbContext>()
            .UseInMemoryDatabase(databaseName: $"TestDb_{System.Guid.NewGuid()}")
            .Options;

        _context = new ProductDbContext(options);
        _unitOfWork = new UnitOfWork<ProductDbContext>(_context);
        _service = new CategoryService(_unitOfWork);
    }

    [TearDown]
    public void TearDown()
    {
        _unitOfWork.Dispose();
        _context.Dispose();
    }

    [Test]
    public async Task CreateCategoryAsync_Should_Add_Category_And_Return_Dto()
    {
        var dto = new CategoryCreateDto { Name = "Books", Description = "All books" };

        var result = await _service.CreateCategoryAsync(dto);
        var categories = await _context.Categories.ToListAsync();

        Assert.That(result, Is.Not.Null);
        Assert.That(categories.Count, Is.EqualTo(1));
        Assert.That(result.Name, Is.EqualTo("Books"));
    }

    [Test]
    public async Task GetAllCategoryAsync_Should_Return_All_Categories()
    {
        _context.Categories.AddRange(
            new Category { Name = "A", Description = "DescA" },
            new Category { Name = "B", Description = "DescB" }
        );
        await _context.SaveChangesAsync();

        var result = await _service.GetAllCategoryAsync();

        Assert.That(result.Count(), Is.EqualTo(2));
    }

    [Test]
    public async Task GetCategoryByIdAsync_Should_Return_Correct_Category()
    {
        var category = new Category { Name = "Laptop", Description = "Electronics" };
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();

        var result = await _service.GetCategoryByIdAsync(category.Id);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Name, Is.EqualTo("Laptop"));
    }

    [Test]
    public async Task GetCategoryByIdAsync_Should_Return_Null_If_Not_Found()
    {
        var result = await _service.GetCategoryByIdAsync(999);

        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task UpdateCategoryAsync_Should_Update_Existing_Category()
    {
        var category = new Category { Name = "Old", Description = "OldDesc" };
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();

        var updateDto = new CategoryDto
        {
            Id = category.Id,
            Name = "New",
            Description = "NewDesc"
        };

        var success = await _service.UpdateCategoryAsync(category.Id, updateDto);
        var updatedCategory = await _context.Categories.FindAsync(category.Id);

        Assert.That(success, Is.True);
        Assert.That(updatedCategory.Name, Is.EqualTo("New"));
    }

    [Test]
    public async Task UpdateCategoryAsync_Should_Return_False_If_Not_Found()
    {
        var dto = new CategoryDto { Id = 999, Name = "Test", Description = "Desc" };

        var success = await _service.UpdateCategoryAsync(999, dto);

        Assert.That(success, Is.False);
    }

    [Test]
    public async Task DeleteCategoryAsync_Should_Remove_Category()
    {
        var category = new Category { Name = "ToDelete", Description = "Desc" };
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();

        var success = await _service.DeleteCategoryAsync(category.Id);
        var remaining = await _context.Categories.FindAsync(category.Id);

        Assert.That(success, Is.True);
        Assert.That(remaining, Is.Null);
    }

    [Test]
    public async Task DeleteCategoryAsync_Should_Return_False_If_Not_Found()
    {
        var success = await _service.DeleteCategoryAsync(999);

        Assert.That(success, Is.False);
    }
}