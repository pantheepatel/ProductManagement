namespace ProductManagement.Tests.Controllers;

[TestFixture]
public class CustomerControllerTests
{
    private Mock<ICustomerService> _customerServiceMock;
    private CustomerController _controller;

    [SetUp]
    public void Setup()
    {
        _customerServiceMock = new Mock<ICustomerService>();
        _controller = new CustomerController(_customerServiceMock.Object);
    }

    [Test]
    public async Task GetAll_ReturnsOk_WithCustomers()
    {
        var customers = new List<CustomerDto>
            {
                new CustomerDto { Id = 1, Name = "John Doe", Email = "john@example.com" },
                new CustomerDto { Id = 2, Name = "Jane Doe", Email = "jane@example.com" }
            };

        _customerServiceMock
            .Setup(s => s.GetAllCustomersAsync())
            .ReturnsAsync(customers);

        var result = await _controller.GetAll();

        var okResult = result as OkObjectResult;
        Assert.That(okResult, Is.Not.Null);

        var returnedCustomers = okResult.Value as IEnumerable<CustomerDto>;
        Assert.That(returnedCustomers.Count(), Is.EqualTo(2));
    }

    [Test]
    public async Task GetById_ReturnsOk_WhenFound()
    {
        var customer = new CustomerDto { Id = 1, Name = "John Doe", Email = "john@example.com" };

        _customerServiceMock
            .Setup(s => s.GetCustomerByIdAsync(1))
            .ReturnsAsync(customer);

        var result = await _controller.GetById(1);

        var okResult = result as OkObjectResult;
        Assert.That(okResult, Is.Not.Null);

        var returnedCustomer = okResult.Value as CustomerDto;
        Assert.That(returnedCustomer.Id, Is.EqualTo(1));
    }

    [Test]
    public async Task GetById_ReturnsNotFound_WhenNull()
    {
        _customerServiceMock
            .Setup(s => s.GetCustomerByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((CustomerDto)null);

        var result = await _controller.GetById(99);

        Assert.That(result, Is.TypeOf<NotFoundResult>());
    }

    [Test]
    public async Task Create_ReturnsCreatedAtAction()
    {
        var newCustomer = new CustomerDto { Id = 10, Name = "Alice", Email = "alice@example.com" };

        _customerServiceMock
            .Setup(s => s.CreateCustomerAsync(It.IsAny<CustomerCreateDto>()))
            .ReturnsAsync(newCustomer);

        var dto = new CustomerCreateDto { Name = "Alice", Email = "alice@example.com" };

        var result = await _controller.Create(dto);

        var createdResult = result as CreatedAtActionResult;
        Assert.That(createdResult, Is.Not.Null);
        Assert.That(createdResult.RouteValues["id"], Is.EqualTo(10));
    }

    [Test]
    public async Task Update_ReturnsNoContent_WhenSuccess()
    {
        _customerServiceMock
            .Setup(s => s.UpdateCustomerAsync(1, It.IsAny<CustomerDto>()))
            .ReturnsAsync(true);

        var result = await _controller.Update(1, new CustomerDto());

        Assert.That(result, Is.TypeOf<NoContentResult>());
    }

    [Test]
    public async Task Update_ReturnsNotFound_WhenFails()
    {
        _customerServiceMock
            .Setup(s => s.UpdateCustomerAsync(It.IsAny<int>(), It.IsAny<CustomerDto>()))
            .ReturnsAsync(false);

        var result = await _controller.Update(99, new CustomerDto());

        Assert.That(result, Is.TypeOf<NotFoundResult>());
    }

    [Test]
    public async Task Delete_ReturnsNoContent_WhenSuccess()
    {
        _customerServiceMock
            .Setup(s => s.DeleteCustomerAsync(1))
            .ReturnsAsync(true);

        var result = await _controller.Delete(1);

        Assert.That(result, Is.TypeOf<NoContentResult>());
    }

    [Test]
    public async Task Delete_ReturnsNotFound_WhenFails()
    {
        _customerServiceMock
            .Setup(s => s.DeleteCustomerAsync(99))
            .ReturnsAsync(false);

        var result = await _controller.Delete(99);

        Assert.That(result, Is.TypeOf<NotFoundResult>());
    }
}
