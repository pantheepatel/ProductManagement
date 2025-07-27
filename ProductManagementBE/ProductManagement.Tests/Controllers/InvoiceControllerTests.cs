using ProductManagement.Api.Services.InvoiceService;

namespace ProductManagement.Tests.Controllers;

[TestFixture]
public class InvoiceControllerTests
{
    private Mock<IInvoiceService> _invoiceServiceMock;
    private InvoiceController _controller;

    [SetUp]
    public void Setup()
    {
        _invoiceServiceMock = new Mock<IInvoiceService>();
        _controller = new InvoiceController(_invoiceServiceMock.Object);
    }

    [Test]
    public async Task GetAll_ReturnsOk_WithInvoices()
    {
        var invoices = new List<InvoiceDto>
            {
                new InvoiceDto { Id = 1, CustomerId = 101, PriceTotal = 100, GrandTotal = 118 },
                new InvoiceDto { Id = 2, CustomerId = 102, PriceTotal = 200, GrandTotal = 236 }
            };

        _invoiceServiceMock
            .Setup(s => s.GetAllInvoicesAsync())
            .ReturnsAsync(invoices);

        var result = await _controller.GetAll();

        var okResult = result as OkObjectResult;
        Assert.That(okResult, Is.Not.Null);

        var returnedInvoices = okResult.Value as IEnumerable<InvoiceDto>;
        Assert.That(returnedInvoices, Is.Not.Null);
        Assert.That(returnedInvoices.Count(), Is.EqualTo(2));
    }

    [Test]
    public async Task GetById_ReturnsOk_WhenFound()
    {
        var invoice = new InvoiceDto { Id = 1, CustomerId = 101 };

        _invoiceServiceMock
            .Setup(s => s.GetInvoiceByIdAsync(1))
            .ReturnsAsync(invoice);

        var result = await _controller.GetById(1);

        var okResult = result as OkObjectResult;
        Assert.That(okResult, Is.Not.Null);

        var returnedInvoice = okResult.Value as InvoiceDto;
        Assert.That(returnedInvoice.Id, Is.EqualTo(1));
    }

    [Test]
    public async Task GetById_ReturnsNotFound_WhenNull()
    {
        _invoiceServiceMock
            .Setup(s => s.GetInvoiceByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((InvoiceDto?)null);

        var result = await _controller.GetById(99);

        Assert.That(result, Is.TypeOf<NotFoundResult>());
    }

    [Test]
    public async Task Create_ReturnsCreatedAtAction()
    {
        var invoice = new InvoiceDto { Id = 10, CustomerId = 101, PriceTotal = 100 };

        _invoiceServiceMock
            .Setup(s => s.CreateInvoiceAsync(It.IsAny<InvoiceCreateDto>()))
            .ReturnsAsync(invoice);

        var dto = new InvoiceCreateDto { CustomerId = 101, Date = DateTime.UtcNow, Products = new List<InvoiceProductDto>() };

        var result = await _controller.Create(dto);

        var createdResult = result as CreatedAtActionResult;
        Assert.That(createdResult, Is.Not.Null);
        Assert.That(createdResult.RouteValues["id"], Is.EqualTo(10));
    }

    [Test]
    public async Task Update_ReturnsNoContent_WhenSuccess()
    {
        _invoiceServiceMock
            .Setup(s => s.UpdateInvoiceAsync(1, It.IsAny<InvoiceDto>()))
            .ReturnsAsync(true);

        var result = await _controller.Update(1, new InvoiceDto());

        Assert.That(result, Is.TypeOf<NoContentResult>());
    }

    [Test]
    public async Task Update_ReturnsNotFound_WhenFails()
    {
        _invoiceServiceMock
            .Setup(s => s.UpdateInvoiceAsync(It.IsAny<int>(), It.IsAny<InvoiceDto>()))
            .ReturnsAsync(false);

        var result = await _controller.Update(99, new InvoiceDto());

        Assert.That(result, Is.TypeOf<NotFoundResult>());
    }

    [Test]
    public async Task Delete_ReturnsNoContent_WhenSuccess()
    {
        _invoiceServiceMock
            .Setup(s => s.DeleteInvoiceAsync(1))
            .ReturnsAsync(true);

        var result = await _controller.Delete(1);

        Assert.That(result, Is.TypeOf<NoContentResult>());
    }

    [Test]
    public async Task Delete_ReturnsNotFound_WhenFails()
    {
        _invoiceServiceMock
            .Setup(s => s.DeleteInvoiceAsync(99))
            .ReturnsAsync(false);

        var result = await _controller.Delete(99);

        Assert.That(result, Is.TypeOf<NotFoundResult>());
    }
}