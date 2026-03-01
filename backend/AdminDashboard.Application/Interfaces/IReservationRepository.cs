using AdminDashboard.Domain.Entities;

namespace AdminDashboard.Application.Interfaces
{
    public interface IReservationRepository
    {
        Task<IEnumerable<Reservation>> GetAllAsync(int? page = null, int? pageSize = null);
        Task<Reservation?> GetByIdAsync(Guid id);
        Task<Reservation> CreateAsync(Reservation reservation);
        Task<Reservation?> UpdateAsync(Reservation reservation);
        Task<bool> DeleteAsync(Guid id);
        Task<int> CountAsync();
    }
}
