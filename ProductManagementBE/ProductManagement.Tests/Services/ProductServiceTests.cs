using ProductManagement.Api.Services.ProductService;

namespace ProductManagement.Tests.Services;

[TestFixture]
public class ProductServiceTests
{
    private ProductDbContext _context;
    private UnitOfWork<ProductDbContext> _unitOfWork;
    private ProductService _service;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ProductDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new ProductDbContext(options);
        _context.Database.EnsureCreated();

        _unitOfWork = new UnitOfWork<ProductDbContext>(_context);
        _service = new ProductService(_unitOfWork);
    }

    [TearDown]
    public void TearDown()
    {
        _unitOfWork.Dispose();
        _context.Dispose();
    }

    [Test]
    public async Task CreateProductAsync_Should_Add_Product_And_Return_Id()
    {
        var dto = new ProductCreateDto
        {
            Name = "Test Product",
            Description = "Description",
            Tax = 10,
            BasePrice = 100,
            CategoryId = 1,
            Prices = new List<ProductPriceCreateDto>
                {
                    new ProductPriceCreateDto
                    {
                        SeasonalPrice = 80,
                        StartDate = DateTime.UtcNow.AddDays(-1),
                        EndDate = DateTime.UtcNow.AddDays(1)
                    }
                }
        };

        var id = await _service.CreateProductAsync(dto);

        var saved = await _context.Products.Include(p => p.Prices).FirstOrDefaultAsync(p => p.Id == id);

        Assert.That(id, Is.GreaterThan(0));
        Assert.That(saved, Is.Not.Null);
        Assert.That(saved.Name, Is.EqualTo("Test Product"));
        Assert.That(saved.Prices.Count, Is.EqualTo(1));
    }

    [Test]
    public async Task GetAllProductsAsync_Should_Return_Products_With_Calculated_CurrentPrice()
    {
        // Arrange
        var product = new Product
        {
            Name = "Product1",
            Description = "Desc",
            Tax = 10,
            BasePrice = 100,
            CategoryId = 1,
            Prices = new List<ProductPrice>
                {
                    new ProductPrice
                    {
                        SeasonalPrice = 80,
                        StartDate = DateTime.UtcNow.AddDays(-1),
                        EndDate = DateTime.UtcNow.AddDays(1)
                    }
                }
        };
        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        // Act
        var result = await _service.GetAllProductsAsync();

        // Assert
        Assert.That(result.Count(), Is.EqualTo(1));
        var p = result.First();
        Assert.That(p.CurrentPrice, Is.EqualTo(80 * 1.1).Within(0.01));
    }

    [Test]
    public async Task GetProductByIdAsync_Should_Return_ProductDto_When_Found()
    {
        var product = new Product
        {
            Name = "Product2",
            Description = "Desc",
            Tax = 5,
            BasePrice = 200,
            CategoryId = 1
        };
        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        var result = await _service.GetProductByIdAsync(product.Id);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Name, Is.EqualTo("Product2"));
    }

    [Test]
    public async Task GetProductByIdAsync_Should_Return_Null_When_Not_Found()
    {
        var result = await _service.GetProductByIdAsync(999);
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task UpdateProductAsync_Should_Update_Product_When_Found()
    {
        var product = new Product
        {
            Name = "Old",
            Description = "Old",
            Tax = 5,
            BasePrice = 50,
            CategoryId = 1
        };
        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        var dto = new ProductDto
        {
            Id = product.Id,
            Name = "Updated",
            Description = "Updated",
            Tax = 8,
            BasePrice = 100,
            CategoryId = 2,
            Prices = new List<ProductPriceUpdateDto>()
        };

        var success = await _service.UpdateProductAsync(product.Id, dto);

        var updated = await _context.Products.FirstAsync(p => p.Id == product.Id);

        Assert.That(success, Is.True);
        Assert.That(updated.Name, Is.EqualTo("Updated"));
        Assert.That(updated.CategoryId, Is.EqualTo(2));
    }

    [Test]
    public async Task UpdateProductAsync_Should_Return_False_When_Not_Found()
    {
        var dto = new ProductDto
        {
            Id = 999,
            Name = "NotExists",
            Description = "x",
            Tax = 5,
            BasePrice = 100,
            CategoryId = 1,
            Prices = new List<ProductPriceUpdateDto>()
        };

        var result = await _service.UpdateProductAsync(999, dto);
        Assert.That(result, Is.False);
    }

    [Test]
    public async Task DeleteProductAsync_Should_Remove_Product_When_Found()
    {
        var product = new Product
        {
            Name = "ToDelete",
            Description = "Desc",
            Tax = 10,
            BasePrice = 100,
            CategoryId = 1
        };
        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        var result = await _service.DeleteProductAsync(product.Id);
        var exists = await _context.Products.FindAsync(product.Id);

        Assert.That(result, Is.True);
        Assert.That(exists, Is.Null);
    }

    [Test]
    public async Task DeleteProductAsync_Should_Return_False_When_Not_Found()
    {
        var result = await _service.DeleteProductAsync(999);
        Assert.That(result, Is.False);
    }

    [Test]
    public async Task GetProductByIdAsync_Should_Return_Null_When_Product_NotFound()
    {
        // Act
        var result = await _service.GetProductByIdAsync(999);

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task GetProductByIdAsync_Should_Return_Product_With_Prices()
    {
        // Arrange
        var product = new Product
        {
            Id = 1,
            Name = "Test Product",
            Description = "Desc",
            Tax = 10,
            BasePrice = 100,
            CategoryId = 1,
            Prices = new List<ProductPrice>
        {
            new ProductPrice
            {
                Id = 1,
                ProductId = 1,
                SeasonalPrice = 80,
                StartDate = DateTime.UtcNow.AddDays(-2),
                EndDate = DateTime.UtcNow.AddDays(2)
            }
        }
        };
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();

        // Act
        var result = await _service.GetProductByIdAsync(1);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Id, Is.EqualTo(1));
        Assert.That(result.Prices.Count, Is.EqualTo(1));
        Assert.That(result.Prices.First().SeasonalPrice, Is.EqualTo(80));
    }

    [Test]
    public async Task CreateProductAsync_Should_Handle_Null_Prices()
    {
        // Arrange
        var dto = new ProductCreateDto
        {
            Name = "Product without Prices",
            Description = "No prices",
            Tax = 5,
            BasePrice = 50,
            CategoryId = 1,
            Prices = null // branch that is missing
        };

        // Act
        var productId = await _service.CreateProductAsync(dto);

        // Assert
        var product = await _context.Products.FindAsync(productId);
        Assert.That(product, Is.Not.Null);
        Assert.That(product!.Prices.Count, Is.EqualTo(0)); // ensures branch covered
    }
}