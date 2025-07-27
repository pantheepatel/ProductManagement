using ProductManagement.Api.Services.InvoiceService;

namespace ProductManagement.Tests.Services;

[TestFixture]
public class InvoiceServiceTests
{
    private ProductDbContext _context;
    private UnitOfWork<ProductDbContext> _unitOfWork;
    private InvoiceService _service;

    [SetUp]
    public void Setup()
    {
        _context = TestDbContextFactory.CreateContext(Guid.NewGuid().ToString());
        _unitOfWork = new UnitOfWork<ProductDbContext>(_context);
        _service = new InvoiceService(_unitOfWork);

        SeedProducts();
        SeedInvoices();
    }

    [TearDown]
    public void TearDown()
    {
        _unitOfWork.Dispose();
        _context.Dispose();
    }

    private void SeedProducts()
    {
        var products = new List<Product>
            {
                new Product
                {
                    Name = "Product A",
                    Description = "Desc A",
                    Tax = 10,
                    BasePrice = 100,
                    CategoryId = 1,
                    Prices = new List<ProductPrice>
                    {
                        new ProductPrice
                        {
                            SeasonalPrice = 80,
                            StartDate = DateTime.UtcNow.AddDays(-5),
                            EndDate = DateTime.UtcNow.AddDays(5)
                        }
                    }
                },
                new Product
                {
                    Name = "Product B",
                    Description = "Desc B",
                    Tax = 5,
                    BasePrice = 50,
                    CategoryId = 2
                }
            };
        _context.Products.AddRange(products);
        _context.SaveChanges();
    }

    private void SeedInvoices()
    {
        var invoice = new Invoice
        {
            CustomerId = 1,
            Date = DateTime.UtcNow,
            PriceTotal = 100,
            TaxTotal = 10,
            GrandTotal = 110,
            TotalItems = 1,
            InvoiceDetails = new List<InvoiceDetail>
                {
                    new InvoiceDetail
                    {
                        ProductId = _context.Products.First().Id,
                        Quantity = 1,
                        UnitPrice = 100,
                        SubTotal = 100,
                        Tax = 10,
                        TaxTotal = 10,
                        GrandTotal = 110
                    }
                }
        };
        _context.Invoices.Add(invoice);
        _context.SaveChanges();
    }

    [Test]
    public async Task GetAllInvoicesAsync_ShouldReturnInvoices()
    {
        var result = await _service.GetAllInvoicesAsync();

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count(), Is.GreaterThan(0));
        Assert.That(result.First().Products.First().ProductName, Is.EqualTo("Product A"));
    }

    [Test]
    public async Task GetInvoiceByIdAsync_ValidId_ReturnsInvoice()
    {
        var invoice = _context.Invoices.First();
        var result = await _service.GetInvoiceByIdAsync(invoice.Id);

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Id, Is.EqualTo(invoice.Id));
        Assert.That(result.Products.First().ProductName, Is.EqualTo("Product A"));
    }

    [Test]
    public async Task GetInvoiceByIdAsync_InvalidId_ReturnsNull()
    {
        var result = await _service.GetInvoiceByIdAsync(9999);
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task CreateInvoiceAsync_ShouldCreateInvoiceWithCorrectTotals()
    {
        var product = _context.Products.First();

        var dto = new InvoiceCreateDto
        {
            CustomerId = 2,
            Date = DateTime.UtcNow,
            Products = new List<InvoiceProductDto>
                {
                    new InvoiceProductDto { ProductId = product.Id, Quantity = 2 }
                }
        };

        var createdInvoice = await _service.CreateInvoiceAsync(dto);

        var invoiceInDb = await _context.Invoices
            .Include(i => i.InvoiceDetails)
            .FirstOrDefaultAsync(i => i.Id == createdInvoice.Id);

        Assert.That(invoiceInDb, Is.Not.Null);
        Assert.That(invoiceInDb!.TotalItems, Is.EqualTo(2));
        Assert.That(invoiceInDb.PriceTotal, Is.GreaterThan(0));
        Assert.That(invoiceInDb.GrandTotal, Is.GreaterThan(invoiceInDb.PriceTotal));
    }

    [Test]
    public async Task UpdateInvoiceAsync_ValidId_ReturnsTrue()
    {
        var invoice = _context.Invoices.Include(i => i.InvoiceDetails).First();

        var dto = new InvoiceDto
        {
            Id = invoice.Id,
            Date = DateTime.UtcNow,
            PriceTotal = 200,
            TaxTotal = 20,
            GrandTotal = 220,
            TotalItems = 2,
            CustomerId = 1,
            Products = new List<InvoiceProductDto>
                {
                    new InvoiceProductDto { ProductId = _context.Products.Last().Id, Quantity = 2 }
                }
        };

        var result = await _service.UpdateInvoiceAsync(invoice.Id, dto);

        Assert.That(result, Is.True);
        var updated = await _service.GetInvoiceByIdAsync(invoice.Id);
        Assert.That(updated!.PriceTotal, Is.EqualTo(200));
    }

    [Test]
    public async Task UpdateInvoiceAsync_InvalidId_ReturnsFalse()
    {
        var dto = new InvoiceDto
        {
            Id = 9999,
            Date = DateTime.UtcNow,
            PriceTotal = 100,
            TaxTotal = 10,
            GrandTotal = 110,
            TotalItems = 1,
            CustomerId = 1,
            Products = new List<InvoiceProductDto>()
        };

        var result = await _service.UpdateInvoiceAsync(9999, dto);
        Assert.That(result, Is.False);
    }

    [Test]
    public async Task DeleteInvoiceAsync_ValidId_RemovesInvoice()
    {
        var invoice = _context.Invoices.First();
        var result = await _service.DeleteInvoiceAsync(invoice.Id);

        Assert.That(result, Is.True);
        Assert.That(_context.Invoices.Any(i => i.Id == invoice.Id), Is.False);
    }

    [Test]
    public async Task DeleteInvoiceAsync_InvalidId_ReturnsFalse()
    {
        var result = await _service.DeleteInvoiceAsync(9999);
        Assert.That(result, Is.False);
    }

    [Test]
    public void CreateInvoiceAsync_Should_Throw_When_ProductNotFound()
    {
        // Arrange
        var dto = new InvoiceCreateDto
        {
            CustomerId = 1,
            Date = DateTime.UtcNow,
            Products = new List<InvoiceProductDto>
        {
            new InvoiceProductDto { ProductId = 999, Quantity = 1 }
        }
        };

        // Act & Assert
        Assert.ThrowsAsync<InvalidOperationException>(() => _service.CreateInvoiceAsync(dto));
    }
}
