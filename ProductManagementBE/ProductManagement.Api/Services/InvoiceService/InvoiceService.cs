namespace ProductManagement.Api.Services.InvoiceService;

public class InvoiceService : IInvoiceService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<Invoice> _invoiceRepo;
    private readonly IRepository<InvoiceDetail> _invoiceDetailRepo;

    public InvoiceService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _invoiceRepo = _unitOfWork.GetRepository<Invoice>();
        _invoiceDetailRepo = _unitOfWork.GetRepository<InvoiceDetail>();
    }

    public async Task<IEnumerable<InvoiceDto>> GetAllInvoicesAsync()
    {
        var invoices = await _invoiceRepo
            .Query()
            .Include(i => i.InvoiceDetails)
            .ThenInclude(d => d.Product)
            .ToListAsync();

        return invoices.Select(i => new InvoiceDto
        {
            Id = i.Id,
            Date = i.Date,
            PriceTotal = i.PriceTotal,
            TaxTotal = i.TaxTotal,
            GrandTotal = i.GrandTotal,
            TotalItems = i.TotalItems,
            CustomerId = i.CustomerId,
            Products = i.InvoiceDetails.Select(d => new InvoiceProductDto
            {
                ProductId = d.ProductId,
                Quantity = d.Quantity,
                ProductName = d.Product?.Name ?? string.Empty
            }).ToList()
        });
    }

    public async Task<InvoiceDto?> GetInvoiceByIdAsync(int id)
    {
        var invoice = await _invoiceRepo
            .Query()
            .Include(i => i.InvoiceDetails)
            .ThenInclude(d => d.Product)
            .FirstOrDefaultAsync(i => i.Id == id);

        if (invoice == null)
            return null;

        return new InvoiceDto
        {
            Id = invoice.Id,
            Date = invoice.Date,
            PriceTotal = invoice.PriceTotal,
            TaxTotal = invoice.TaxTotal,
            GrandTotal = invoice.GrandTotal,
            TotalItems = invoice.TotalItems,
            CustomerId = invoice.CustomerId,
            Products = invoice.InvoiceDetails.Select(d => new InvoiceProductDto
            {
                ProductId = d.ProductId,
                Quantity = d.Quantity,
                ProductName = d.Product?.Name ?? string.Empty,
                UnitPrice = d.UnitPrice,
                Tax = d.Tax,
                Total = d.GrandTotal
            }).ToList()
        };
    }

    public async Task<InvoiceDto> CreateInvoiceAsync(InvoiceCreateDto dto)
    {
        // Load products that are being invoiced
        var productIds = dto.Products.Select(p => p.ProductId).ToList();

        var products = await _unitOfWork.GetRepository<Product>()
            .Query()
            .Include(p => p.Prices)
            .Where(p => productIds.Contains(p.Id))
            .ToListAsync();

        if (products.Count != productIds.Count)
        {
            throw new InvalidOperationException("One or more products not found.");
        }

        var invoice = new Invoice
        {
            CustomerId = dto.CustomerId,
            Date = dto.Date,
            InvoiceDetails = new List<InvoiceDetail>()
        };

        decimal priceTotal = 0, taxTotal = 0;

        var today = DateTime.UtcNow;

        foreach (var item in dto.Products)
        {
            var product = products.First(p => p.Id == item.ProductId);

            // Compute current price (seasonal or base)
            var activePrice = product.Prices
                .FirstOrDefault(price => price.StartDate <= today && price.EndDate >= today);

            var basePrice = activePrice?.SeasonalPrice ?? product.BasePrice;
            var priceWithTax = basePrice * (1 + product.Tax / 100);

            var subTotal = basePrice * item.Quantity;
            var taxAmount = subTotal * (product.Tax / 100);
            var grandTotal = subTotal + taxAmount;

            priceTotal += subTotal;
            taxTotal += taxAmount;

            invoice.InvoiceDetails.Add(new InvoiceDetail
            {
                ProductId = product.Id,
                Quantity = item.Quantity,
                UnitPrice = basePrice,
                SubTotal = subTotal,
                Tax = product.Tax,
                TaxTotal = taxAmount,
                GrandTotal = grandTotal
            });
        }

        invoice.TotalItems = invoice.InvoiceDetails.Sum(d => d.Quantity);
        invoice.PriceTotal = priceTotal;
        invoice.TaxTotal = taxTotal;
        invoice.GrandTotal = priceTotal + taxTotal;

        await _invoiceRepo.AddAsync(invoice);
        await _unitOfWork.CompleteAsync();

        // Return DTO
        return new InvoiceDto
        {
            Id = invoice.Id,
            Date = invoice.Date,
            PriceTotal = invoice.PriceTotal,
            TaxTotal = invoice.TaxTotal,
            GrandTotal = invoice.GrandTotal,
            TotalItems = invoice.TotalItems,
            CustomerId = invoice.CustomerId,
            Products = invoice.InvoiceDetails.Select(d => new InvoiceProductDto
            {
                ProductId = d.ProductId,
                Quantity = d.Quantity,
                ProductName = products.First(p => p.Id == d.ProductId).Name
            }).ToList()
        };
    }

    public async Task<bool> UpdateInvoiceAsync(int id, InvoiceDto dto)
    {
        var invoice = await _invoiceRepo.Query()
            .Include(i => i.InvoiceDetails)
            .FirstOrDefaultAsync(i => i.Id == id);

        if (invoice == null)
            return false;

        invoice.Date = dto.Date;
        invoice.CustomerId = dto.CustomerId;
        invoice.TotalItems = dto.TotalItems;
        invoice.PriceTotal = dto.PriceTotal;
        invoice.TaxTotal = dto.TaxTotal;
        invoice.GrandTotal = dto.GrandTotal;

        // Update InvoiceDetails (optional: more complex merge logic)
        invoice.InvoiceDetails.Clear();
        foreach (var p in dto.Products)
        {
            invoice.InvoiceDetails.Add(new InvoiceDetail
            {
                ProductId = p.ProductId,
                Quantity = p.Quantity
            });
        }

        _invoiceRepo.Update(invoice);
        await _unitOfWork.CompleteAsync();
        return true;
    }

    public async Task<bool> DeleteInvoiceAsync(int id)
    {
        var invoice = await _invoiceRepo.GetByIdAsync(id);
        if (invoice == null)
            return false;

        _invoiceRepo.Remove(invoice);
        await _unitOfWork.CompleteAsync();
        return true;
    }
}