using AdminDashboard.Application.Dtos;
using AdminDashboard.Application.Interfaces;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdminDashboard.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _service;

        // Constructeur
        public ProductsController(IProductService service)
        {
            _service = service;
        }



        // ======== GetAll
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int? page, [FromQuery] int? pageSize)
        {
            if (page.HasValue && pageSize.HasValue)
            {
                var paginated = await _service.GetPaginatedAsync(page.Value, pageSize.Value);
                return Ok(paginated);
            }

            var products = await _service.GetAllAsync();
            return Ok(products);
        }



        // ======== GetById
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var product = await _service.GetByIdAsync(id);
            if (product == null) return NotFound();
            return Ok(product);
        }



        // ======== Create
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }



        // ======== Update
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] ProductUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (!await _service.ExistsAsync(id)) return NotFound();

            var updated = await _service.UpdateAsync(id, dto);
            return Ok(updated);
        }



        // ======== Delete
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (!await _service.ExistsAsync(id)) return NotFound();

            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
