using AdminDashboard.Application.Dtos;
using AdminDashboard.Domain.Entities;

namespace AdminDashboard.Application.Interfaces
{
    public interface IReservationService
    {
        Task<IEnumerable<ReservationResponseDto>> GetAllAsync(int? page = null, int? pageSize = null);
        Task<PaginatedReservationResponseDto> GetPaginatedAsync(int page, int pageSize);
        Task<ReservationResponseDto?> GetByIdAsync(Guid id);
        Task<ReservationResponseDto> CreateAsync(Dtos.ReservationCreateDto dto);
        Task<ReservationResponseDto?> UpdateAsync(Guid id, Dtos.ReservationUpdateDto dto);
        Task<bool> DeleteAsync(Guid id);
        Task<int> CountAsync();
        Task<bool> ExistsAsync(Guid id);
    }
}
