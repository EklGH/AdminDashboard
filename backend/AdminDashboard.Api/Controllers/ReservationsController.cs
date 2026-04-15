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
    public class ReservationsController : ControllerBase
    {
        private readonly IReservationService _service;

        // Constructeur
        public ReservationsController(IReservationService service)
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

            var reservations = await _service.GetAllAsync();
            return Ok(reservations);
        }



        // ======== GetById
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var reservation = await _service.GetByIdAsync(id);
            if (reservation == null) return NotFound();
            return Ok(reservation);
        }



        // ======== Create
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ReservationCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }



        // ======== Update
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] ReservationUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var updated = await _service.UpdateAsync(id, dto);
            if (updated == null) return NotFound();

            return Ok(updated);
        }



        // ======== Delete
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted) return NotFound();

            return NoContent();
        }
    }
}
