namespace ProductManagement.Api.Services.InvoiceService;

public interface IInvoiceService
{
    Task<IEnumerable<InvoiceDto>> GetAllInvoicesAsync();
    Task<InvoiceDto?> GetInvoiceByIdAsync(int id);
    Task<InvoiceDto> CreateInvoiceAsync(InvoiceCreateDto dto);
    Task<bool> UpdateInvoiceAsync(int id, InvoiceDto dto);
    Task<bool> DeleteInvoiceAsync(int id);
}
