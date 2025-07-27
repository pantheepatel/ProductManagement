namespace ProductManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;
    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    // GET: api/Category
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var categories = await _categoryService.GetAllCategoryAsync();
        return Ok(categories);
    }

    // GET: api/Category/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var category = await _categoryService.GetCategoryByIdAsync(id);
        if (category == null)
            return NotFound();
        return Ok(category);
    }

    // POST: api/Category
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CategoryCreateDto category)
    {
        var categoryResponse = await _categoryService.CreateCategoryAsync(category);
        return CreatedAtAction(nameof(GetById), new { id = categoryResponse.Id }, categoryResponse);
    }

    // PUT: api/Category/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] CategoryDto category)
    {
        var success = await _categoryService.UpdateCategoryAsync(id, category);
        if (!success)
            return NotFound();

        return NoContent();
    }

    // DELETE: api/Category/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _categoryService.DeleteCategoryAsync(id);
        if (!success)
            return NotFound();

        return NoContent();
    }
}
