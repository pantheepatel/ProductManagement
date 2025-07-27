namespace ProductManagement.Api.DTOs;

public class InvoiceCreateDto
{
    public int CustomerId { get; set; }
    public DateTime Date { get; set; }
    public List<InvoiceProductDto> Products { get; set; }
}

public class InvoiceProductDto
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; } = 0;
    public decimal Tax { get; set; } = 0;
    public decimal Total { get; set; } = 0;
}

public class InvoiceDto
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public decimal PriceTotal { get; set; }
    public decimal TaxTotal { get; set; }
    public decimal GrandTotal { get; set; }
    public int TotalItems { get; set; }
    public int CustomerId { get; set; }
    public List<InvoiceProductDto> Products { get; set; }
}

public class InvoiceDetailDto
{
    public int Id { get; set; }
    public int InvoiceId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal SubTotal { get; set; }
    public decimal Tax { get; set; }
    public decimal TaxTotal { get; set; }
    public decimal GrandTotal { get; set; }
}
