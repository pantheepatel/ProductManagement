using ProductManagement.Api.Services.ProductService;

namespace ProductManagement.Tests.Controllers;

[TestFixture]
public class ProductControllerTests
{
    private Mock<IProductService> _productServiceMock;
    private ProductController _controller;

    [SetUp]
    public void Setup()
    {
        _productServiceMock = new Mock<IProductService>();
        _controller = new ProductController(_productServiceMock.Object);
    }

    [Test]
    public async Task GetAll_ReturnsOk_WithProducts()
    {
        var products = new List<ProductDto>
            {
                new ProductDto { Id = 1, Name = "Laptop" },
                new ProductDto { Id = 2, Name = "Phone" }
            };

        _productServiceMock
            .Setup(s => s.GetAllProductsAsync())
            .ReturnsAsync(products);

        var result = await _controller.GetAll();

        var okResult = result as OkObjectResult;
        Assert.That(okResult, Is.Not.Null);

        var returnedProducts = okResult.Value as IEnumerable<ProductDto>;
        Assert.That(returnedProducts, Is.Not.Null);
        Assert.That(returnedProducts.Count(), Is.EqualTo(2));
    }

    [Test]
    public async Task GetById_ReturnsOk_WhenFound()
    {
        var product = new ProductDto { Id = 1, Name = "Laptop" };

        _productServiceMock
            .Setup(s => s.GetProductByIdAsync(1))
            .ReturnsAsync(product);

        var result = await _controller.GetById(1);

        var okResult = result as OkObjectResult;
        Assert.That(okResult, Is.Not.Null);

        var returnedProduct = okResult.Value as ProductDto;
        Assert.That(returnedProduct.Id, Is.EqualTo(1));
    }

    [Test]
    public async Task GetById_ReturnsNotFound_WhenNotExists()
    {
        _productServiceMock
            .Setup(s => s.GetProductByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((ProductDto?)null);

        var result = await _controller.GetById(99);

        Assert.That(result, Is.TypeOf<NotFoundResult>());
    }

    [Test]
    public async Task Create_ReturnsCreatedAtAction()
    {
        _productServiceMock
            .Setup(s => s.CreateProductAsync(It.IsAny<ProductCreateDto>()))
            .ReturnsAsync(10);

        var dto = new ProductCreateDto { Name = "Tablet", BasePrice = 500 };

        var result = await _controller.Create(dto);

        var createdResult = result as CreatedAtActionResult;
        Assert.That(createdResult, Is.Not.Null);
        Assert.That(createdResult.RouteValues["id"], Is.EqualTo(10));
    }

    [Test]
    public async Task Update_ReturnsNoContent_WhenSuccess()
    {
        _productServiceMock
            .Setup(s => s.UpdateProductAsync(1, It.IsAny<ProductDto>()))
            .ReturnsAsync(true);

        var result = await _controller.Update(1, new ProductDto());

        Assert.That(result, Is.TypeOf<NoContentResult>());
    }

    [Test]
    public async Task Update_ReturnsNotFound_WhenFails()
    {
        _productServiceMock
            .Setup(s => s.UpdateProductAsync(It.IsAny<int>(), It.IsAny<ProductDto>()))
            .ReturnsAsync(false);

        var result = await _controller.Update(99, new ProductDto());

        Assert.That(result, Is.TypeOf<NotFoundResult>());
    }

    [Test]
    public async Task Delete_ReturnsNoContent_WhenSuccess()
    {
        _productServiceMock
            .Setup(s => s.DeleteProductAsync(1))
            .ReturnsAsync(true);

        var result = await _controller.Delete(1);

        Assert.That(result, Is.TypeOf<NoContentResult>());
    }

    [Test]
    public async Task Delete_ReturnsNotFound_WhenFails()
    {
        _productServiceMock
            .Setup(s => s.DeleteProductAsync(99))
            .ReturnsAsync(false);

        var result = await _controller.Delete(99);

        Assert.That(result, Is.TypeOf<NotFoundResult>());
    }
}
