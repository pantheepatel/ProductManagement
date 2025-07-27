namespace ProductManagement.Api.Services.CustomerService;

public interface ICustomerService
{
    Task<IEnumerable<CustomerDto>> GetAllCustomersAsync();
    Task<CustomerDto?> GetCustomerByIdAsync(int id);
    Task<CustomerDto> CreateCustomerAsync(CustomerCreateDto dto);
    Task<bool> UpdateCustomerAsync(int id, CustomerDto dto);
    Task<bool> DeleteCustomerAsync(int id);
}
