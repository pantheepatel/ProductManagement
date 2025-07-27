namespace ProductManagement.Api.models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Tax { get; set; }
    public decimal BasePrice { get; set; }
    // Foreign key
    public int CategoryId { get; set; }
    public Category? Category { get; set; }
    // Navigation property for dynamic prices
    public ICollection<ProductPrice> Prices { get; set; } = new List<ProductPrice>();
}
