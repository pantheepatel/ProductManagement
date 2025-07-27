using ProductManagement.Api.UnitOfWork;

namespace ProductManagement.Tests.Services;

[TestFixture]
public class CustomerServiceTests
{
    private ProductDbContext _context;
    private UnitOfWork<ProductDbContext> _unitOfWork;
    private CustomerService _customerService;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ProductDbContext>()
            .UseInMemoryDatabase(databaseName: $"TestDb_{System.Guid.NewGuid()}")
            .Options;

        _context = new ProductDbContext(options);
        _unitOfWork = new UnitOfWork<ProductDbContext>(_context);
        _customerService = new CustomerService(_unitOfWork);
    }

    [TearDown]
    public void TearDown()
    {
        _unitOfWork.Dispose();
        _context.Dispose();
    }

    [Test]
    public async Task CreateCustomerAsync_Should_Add_New_Customer()
    {
        var dto = new CustomerCreateDto
        {
            Name = "John Doe",
            Email = "john@example.com"
        };

        var result = await _customerService.CreateCustomerAsync(dto);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.GreaterThan(0));
        Assert.That(result.Name, Is.EqualTo("John Doe"));
    }

    [Test]
    public async Task GetAllCustomersAsync_Should_Return_Customers()
    {
        _context.Customers.Add(new Customer { Name = "A", Email = "a@example.com" });
        _context.Customers.Add(new Customer { Name = "B", Email = "b@example.com" });
        await _context.SaveChangesAsync();

        var customers = await _customerService.GetAllCustomersAsync();

        Assert.That(customers.Count(), Is.EqualTo(2));
    }

    [Test]
    public async Task GetCustomerByIdAsync_Should_Return_Customer_When_Exists()
    {
        var customer = new Customer { Name = "Test", Email = "test@example.com" };
        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();

        var result = await _customerService.GetCustomerByIdAsync(customer.Id);

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Name, Is.EqualTo("Test"));
    }

    [Test]
    public async Task GetCustomerByIdAsync_Should_Return_Null_When_Not_Exists()
    {
        var result = await _customerService.GetCustomerByIdAsync(999);
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task UpdateCustomerAsync_Should_Update_Customer_When_Exists()
    {
        var customer = new Customer { Name = "Old", Email = "old@example.com" };
        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();

        var dto = new CustomerDto { Id = customer.Id, Name = "Updated", Email = "updated@example.com" };

        var success = await _customerService.UpdateCustomerAsync(customer.Id, dto);

        Assert.That(success, Is.True);
        var updatedCustomer = await _context.Customers.FindAsync(customer.Id);
        Assert.That(updatedCustomer!.Name, Is.EqualTo("Updated"));
    }

    [Test]
    public async Task UpdateCustomerAsync_Should_Return_False_When_Not_Exists()
    {
        var dto = new CustomerDto { Id = 999, Name = "Updated", Email = "updated@example.com" };

        var success = await _customerService.UpdateCustomerAsync(999, dto);

        Assert.That(success, Is.False);
    }

    [Test]
    public async Task DeleteCustomerAsync_Should_Remove_Customer_When_Exists()
    {
        var customer = new Customer { Name = "Delete", Email = "delete@example.com" };
        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();

        var success = await _customerService.DeleteCustomerAsync(customer.Id);

        Assert.That(success, Is.True);
        Assert.That(await _context.Customers.FindAsync(customer.Id), Is.Null);
    }

    [Test]
    public async Task DeleteCustomerAsync_Should_Return_False_When_Not_Exists()
    {
        var success = await _customerService.DeleteCustomerAsync(999);
        Assert.That(success, Is.False);
    }
}