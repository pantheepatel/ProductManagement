using ProductManagement.Api.Services.InvoiceService;

namespace ProductManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InvoiceController : ControllerBase
{
    private readonly IInvoiceService _invoiceService;

    public InvoiceController(IInvoiceService invoiceService)
    {
        _invoiceService = invoiceService;
    }

    // GET: api/Invoice
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var invoices = await _invoiceService.GetAllInvoicesAsync();
        return Ok(invoices);
    }

    // GET: api/Invoice/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var invoice = await _invoiceService.GetInvoiceByIdAsync(id);
        if (invoice == null)
            return NotFound();

        return Ok(invoice);
    }

    // POST: api/Invoice
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] InvoiceCreateDto invoiceDto)
    {
        var createdInvoice = await _invoiceService.CreateInvoiceAsync(invoiceDto);
        return CreatedAtAction(nameof(GetById), new { id = createdInvoice.Id }, createdInvoice);
    }

    // PUT: api/Invoice/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] InvoiceDto invoiceDto)
    {
        var success = await _invoiceService.UpdateInvoiceAsync(id, invoiceDto);
        if (!success)
            return NotFound();

        return NoContent();
    }

    // DELETE: api/Invoice/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _invoiceService.DeleteInvoiceAsync(id);
        if (!success)
            return NotFound();

        return NoContent();
    }
}
