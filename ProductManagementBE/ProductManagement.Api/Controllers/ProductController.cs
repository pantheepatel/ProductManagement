namespace ProductManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    // GET: api/Product
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var products = await _productService.GetAllProductsAsync();
        return Ok(products);
    }

    // GET: api/Product/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var product = await _productService.GetProductByIdAsync(id);
        if (product == null)
            return NotFound();

        return Ok(product);
    }

    // POST: api/Product
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ProductCreateDto productDto)
    {
        var createdProductId = await _productService.CreateProductAsync(productDto);
        return CreatedAtAction(nameof(GetById), new { id = createdProductId }, new { Id = createdProductId });
    }

    // PUT: api/Product/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] ProductDto productDto)
    {
        var success = await _productService.UpdateProductAsync(id, productDto);
        if (!success)
            return NotFound();

        return NoContent();
    }

    // DELETE: api/Product/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _productService.DeleteProductAsync(id);
        if (!success)
            return NotFound();

        return NoContent();
    }
}
