namespace ProductManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly IRepository<Category> _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CategoryController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _categoryRepository = _unitOfWork.GetRepository<Category>();
    }

    // GET: api/Category
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var categories = await _categoryRepository.GetAllAsync();
        return Ok(categories);
    }

    // GET: api/Category/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        if (category == null)
            return NotFound();
        return Ok(category);
    }

    // POST: api/Category
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Category category)
    {
        if (category == null)
            return BadRequest();

        await _categoryRepository.AddAsync(category);
        await _unitOfWork.CompleteAsync();

        return CreatedAtAction(nameof(GetById), new { id = category.Id }, category);
    }

    // PUT: api/Category/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Category category)
    {
        if (category == null || category.Id != id)
            return BadRequest();

        var existingCategory = await _categoryRepository.GetByIdAsync(id);
        if (existingCategory == null)
            return NotFound();

        existingCategory.Name = category.Name;
        existingCategory.Description = category.Description;

        _categoryRepository.Update(existingCategory);
        await _unitOfWork.CompleteAsync();

        return NoContent();
    }

    // DELETE: api/Category/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        if (category == null)
            return NotFound();

        _categoryRepository.Remove(category);
        await _unitOfWork.CompleteAsync();

        return NoContent();
    }
}
