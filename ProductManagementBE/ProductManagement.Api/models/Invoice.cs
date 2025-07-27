namespace ProductManagement.Api.models;

public class Invoice
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public decimal PriceTotal { get; set; }
    public decimal TaxTotal { get; set; }
    public decimal GrandTotal { get; set; }
    public int TotalItems { get; set; }

    // Foreign key
    public int CustomerId { get; set; }
    public Customer? Customer { get; set; }

    // Navigation property
    public ICollection<InvoiceDetail> InvoiceDetails { get; set; } = new List<InvoiceDetail>();
}
