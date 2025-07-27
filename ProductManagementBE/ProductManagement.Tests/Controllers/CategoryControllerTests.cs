namespace ProductManagement.Tests.Controllers;

[TestFixture]
public class CategoryControllerTests
{
    private Mock<ICategoryService> _categoryServiceMock;
    private CategoryController _controller;

    [SetUp]
    public void Setup()
    {
        _categoryServiceMock = new Mock<ICategoryService>();
        _controller = new CategoryController(_categoryServiceMock.Object);
    }

    [Test]
    public async Task GetAll_ReturnsOk_WithCategories()
    {
        var categories = new List<Category>
            {
                new Category { Id = 1, Name = "Electronics", Description = "Devices" },
                new Category { Id = 2, Name = "Books", Description = "Reading" }
            };

        _categoryServiceMock
            .Setup(s => s.GetAllCategoryAsync())
            .ReturnsAsync(categories);

        var result = await _controller.GetAll();

        var okResult = result as OkObjectResult;
        Assert.That(okResult, Is.Not.Null);

        var returnedCategories = okResult.Value as IEnumerable<Category>;
        Assert.That(returnedCategories.Count(), Is.EqualTo(2));
    }

    [Test]
    public async Task GetById_ReturnsOk_WhenFound()
    {
        var category = new CategoryDto { Id = 1, Name = "Electronics" };

        _categoryServiceMock
            .Setup(s => s.GetCategoryByIdAsync(1))
            .ReturnsAsync(category);

        var result = await _controller.GetById(1);

        var okResult = result as OkObjectResult;
        Assert.That(okResult, Is.Not.Null);

        var returnedCategory = okResult.Value as CategoryDto;
        Assert.That(returnedCategory.Id, Is.EqualTo(1));
    }

    [Test]
    public async Task GetById_ReturnsNotFound_WhenNull()
    {
        _categoryServiceMock
            .Setup(s => s.GetCategoryByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((CategoryDto)null);

        var result = await _controller.GetById(99);

        Assert.That(result, Is.TypeOf<NotFoundResult>());
    }

    [Test]
    public async Task Create_ReturnsCreatedAtAction()
    {
        var newCategory = new CategoryDto { Id = 5, Name = "Toys", Description = "Kids" };

        _categoryServiceMock
            .Setup(s => s.CreateCategoryAsync(It.IsAny<CategoryCreateDto>()))
            .ReturnsAsync(newCategory);

        var dto = new CategoryCreateDto { Name = "Toys", Description = "Kids" };

        var result = await _controller.Create(dto);

        var createdResult = result as CreatedAtActionResult;
        Assert.That(createdResult, Is.Not.Null);
        Assert.That(createdResult.RouteValues["id"], Is.EqualTo(5));
    }

    [Test]
    public async Task Update_ReturnsNoContent_WhenSuccess()
    {
        _categoryServiceMock
            .Setup(s => s.UpdateCategoryAsync(1, It.IsAny<CategoryDto>()))
            .ReturnsAsync(true);

        var result = await _controller.Update(1, new CategoryDto());

        Assert.That(result, Is.TypeOf<NoContentResult>());
    }

    [Test]
    public async Task Update_ReturnsNotFound_WhenFails()
    {
        _categoryServiceMock
            .Setup(s => s.UpdateCategoryAsync(It.IsAny<int>(), It.IsAny<CategoryDto>()))
            .ReturnsAsync(false);

        var result = await _controller.Update(99, new CategoryDto());

        Assert.That(result, Is.TypeOf<NotFoundResult>());
    }

    [Test]
    public async Task Delete_ReturnsNoContent_WhenSuccess()
    {
        _categoryServiceMock
            .Setup(s => s.DeleteCategoryAsync(1))
            .ReturnsAsync(true);

        var result = await _controller.Delete(1);

        Assert.That(result, Is.TypeOf<NoContentResult>());
    }

    [Test]
    public async Task Delete_ReturnsNotFound_WhenFails()
    {
        _categoryServiceMock
            .Setup(s => s.DeleteCategoryAsync(99))
            .ReturnsAsync(false);

        var result = await _controller.Delete(99);

        Assert.That(result, Is.TypeOf<NotFoundResult>());
    }
}