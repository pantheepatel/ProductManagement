using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ProductManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        // GET: api/Category
        [HttpGet]
        public IActionResult GetAll()
        {
            // TODO: Replace with actual data retrieval logic
            return Ok(new[] { "Category1", "Category2" });
        }

        // GET: api/Category/5
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            // TODO: Replace with actual data retrieval logic
            return Ok($"Category {id}");
        }

        // POST: api/Category
        [HttpPost]
        public IActionResult Create([FromBody] object category)
        {
            // TODO: Replace with actual create logic
            return CreatedAtAction(nameof(GetById), new { id = 1 }, category);
        }

        // PUT: api/Category/5
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] object category)
        {
            // TODO: Replace with actual update logic
            return NoContent();
        }

        // DELETE: api/Category/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            // TODO: Replace with actual delete logic
            return NoContent();
        }
    }
}
