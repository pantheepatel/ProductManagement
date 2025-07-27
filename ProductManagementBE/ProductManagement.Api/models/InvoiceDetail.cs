namespace ProductManagement.Api.models;

public class InvoiceDetail
{
    public int Id { get; set; }

    // Foreign keys
    public int InvoiceId { get; set; }
    public Invoice? Invoice { get; set; }

    public int ProductId { get; set; }
    public Product? Product { get; set; }

    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal SubTotal { get; set; } // Total price for the product (Quantity * UnitPrice)

    public decimal Tax { get; set; } 
    public decimal TaxTotal { get; set; } // Total tax for the product (Quantity * Tax)

    public decimal GrandTotal { get; set; } // Total price including tax (SubTotal + TaxTotal)
}
