namespace ProductManagement.Api.models;

public class Customer
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }

    // Navigation property
    public ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
}
