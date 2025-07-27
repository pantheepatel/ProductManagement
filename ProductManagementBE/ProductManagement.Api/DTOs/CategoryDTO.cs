namespace ProductManagement.Api.DTOs;

public class CategoryCreateDto
{
    public string Name { get; set; }
    public string Description { get; set; }
}

public class CategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}
