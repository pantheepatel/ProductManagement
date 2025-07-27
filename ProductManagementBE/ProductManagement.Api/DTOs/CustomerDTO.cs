namespace ProductManagement.Api.DTOs;

public class CustomerCreateDto
{
    public string Name { get; set; }
    public string Email { get; set; }
}

public class CustomerDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
}
