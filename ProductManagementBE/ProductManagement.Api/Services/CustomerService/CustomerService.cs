namespace ProductManagement.Api.Services.CustomerService;

public class CustomerService : ICustomerService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<Customer> _customerRepo;

    public CustomerService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _customerRepo = _unitOfWork.GetRepository<Customer>();
    }

    public async Task<IEnumerable<CustomerDto>> GetAllCustomersAsync()
    {
        var customers = await _customerRepo.GetAllAsync();

        return customers.Select(c => new CustomerDto
        {
            Id = c.Id,
            Name = c.Name,
            Email = c.Email
        }).ToList();
    }

    public async Task<CustomerDto?> GetCustomerByIdAsync(int id)
    {
        var customer = await _customerRepo.GetByIdAsync(id);
        if (customer == null)
            return null;

        return new CustomerDto
        {
            Id = customer.Id,
            Name = customer.Name,
            Email = customer.Email
        };
    }

    public async Task<CustomerDto> CreateCustomerAsync(CustomerCreateDto dto)
    {
        var customer = new Customer
        {
            Name = dto.Name,
            Email = dto.Email
        };

        await _customerRepo.AddAsync(customer);
        await _unitOfWork.CompleteAsync();

        return new CustomerDto
        {
            Id = customer.Id,
            Name = customer.Name,
            Email = customer.Email
        };
    }

    public async Task<bool> UpdateCustomerAsync(int id, CustomerDto dto)
    {
        var customer = await _customerRepo.GetByIdAsync(id);
        if (customer == null)
            return false;

        customer.Name = dto.Name;
        customer.Email = dto.Email;

        _customerRepo.Update(customer);
        await _unitOfWork.CompleteAsync();
        return true;
    }

    public async Task<bool> DeleteCustomerAsync(int id)
    {
        var customer = await _customerRepo.GetByIdAsync(id);
        if (customer == null)
            return false;

        _customerRepo.Remove(customer);
        await _unitOfWork.CompleteAsync();
        return true;
    }
}