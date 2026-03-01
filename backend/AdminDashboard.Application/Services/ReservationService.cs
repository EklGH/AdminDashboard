using AdminDashboard.Application.Dtos;
using AdminDashboard.Application.Interfaces;
using AdminDashboard.Domain.Entities;

namespace AdminDashboard.Application.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _repo;

        // Constructeur
        public ReservationService(IReservationRepository repo)
        {
            _repo = repo;
        }



        // ======== GetAll Reservations
        public async Task<IEnumerable<ReservationResponseDto>> GetAllAsync(int? page = null, int? pageSize = null)
        {
            var reservations = await _repo.GetAllAsync(page, pageSize);
            return reservations.Select(MapToDto);
        }



        // ======== GetPaginated Reservations
        public async Task<PaginatedReservationResponseDto> GetPaginatedAsync(int page, int pageSize)
        {
            var items = await _repo.GetAllAsync(page, pageSize);
            var total = await _repo.CountAsync();

            return new PaginatedReservationResponseDto
            {
                Items = items.Select(MapToDto),
                Page = page,
                PageSize = pageSize,
                TotalItems = total
            };
        }



        // ======== GetById Reservation
        public async Task<ReservationResponseDto?> GetByIdAsync(Guid id)
        {
            var reservation = await _repo.GetByIdAsync(id);
            return reservation == null ? null : MapToDto(reservation);
        }



        // ======== Create Reservation
        public async Task<ReservationResponseDto> CreateAsync(ReservationCreateDto dto)
        {
            var reservation = new Reservation
            {
                Id = Guid.NewGuid(),
                Customer = dto.Customer,
                Date = dto.Date,
                Status = dto.Status,
                ProductId = dto.ProductId,
                UserId = dto.UserId
            };

            var created = await _repo.CreateAsync(reservation);
            return MapToDto(created);
        }



        // ======== Update Reservation
        public async Task<ReservationResponseDto?> UpdateAsync(Guid id, ReservationUpdateDto dto)
        {
            var reservation = new Reservation
            {
                Id = id,
                Customer = dto.Customer,
                Date = dto.Date,
                Status = dto.Status,
                ProductId = dto.ProductId,
                UserId = dto.UserId
            };

            var updated = await _repo.UpdateAsync(reservation);
            return updated == null ? null : MapToDto(updated);
        }



        // ======== Delete Reservation
        public async Task<bool> DeleteAsync(Guid id)
        {
            return await _repo.DeleteAsync(id);
        }



        // ======== Count
        public async Task<int> CountAsync()
        {
            return await _repo.CountAsync();
        }



        // ======== Exists Reservation
        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _repo.GetByIdAsync(id) != null;
        }



        // ======== Mapping Entity -> DTO
        private ReservationResponseDto MapToDto(Reservation r)
        {
            return new ReservationResponseDto
            {
                Id = r.Id,
                Customer = r.Customer,
                Date = r.Date.ToString("yyyy-MM-dd"),
                Status = r.Status,
                ProductId = r.ProductId,
                UserId = r.UserId
            };
        }
    }
}
