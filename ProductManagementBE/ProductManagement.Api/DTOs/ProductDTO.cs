namespace ProductManagement.Api.DTOs;

public class ProductCreateDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Tax { get; set; }
    public decimal BasePrice { get; set; }
    public int CategoryId { get; set; }
    public List<ProductPriceCreateDto> Prices { get; set; }
}

public class ProductDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Tax { get; set; }
    public decimal BasePrice { get; set; }
    public decimal CurrentPrice { get; set; }
    public int CategoryId { get; set; }
    public List<ProductPriceUpdateDto> Prices { get; set; }
}

public class ProductPriceCreateDto
{
    public decimal SeasonalPrice { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}

public class ProductPriceUpdateDto
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public decimal SeasonalPrice { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}
