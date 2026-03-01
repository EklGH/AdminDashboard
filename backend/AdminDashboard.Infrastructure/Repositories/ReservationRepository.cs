using AdminDashboard.Application.Interfaces;
using AdminDashboard.Domain.Entities;
using AdminDashboard.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AdminDashboard.Infrastructure.Repositories
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly AppDbContext _db;

        // Constructeur
        public ReservationRepository(AppDbContext db)
        {
            _db = db;
        }



        // ======== GetAll
        public async Task<IEnumerable<Reservation>> GetAllAsync(int? page = null, int? pageSize = null)
        {
            IQueryable<Reservation> query = _db.Reservations
                .Include(r => r.Product)
                .Include(r => r.User)
                .OrderBy(r => r.Date);

            if (page.HasValue && pageSize.HasValue)
            {
                query = query.Skip((page.Value - 1) * pageSize.Value)
                             .Take(pageSize.Value);
            }

            return await query.ToListAsync();
        }



        // ======== GetById
        public async Task<Reservation?> GetByIdAsync(Guid id)
        {
            return await _db.Reservations
                            .Include(r => r.Product)
                            .Include(r => r.User)
                            .FirstOrDefaultAsync(r => r.Id == id);
        }



        // ======== Create
        public async Task<Reservation> CreateAsync(Reservation reservation)
        {
            if (reservation.Id == Guid.Empty)
                reservation.Id = Guid.NewGuid();

            _db.Reservations.Add(reservation);
            await _db.SaveChangesAsync();
            return reservation;
        }



        // ======== Update
        public async Task<Reservation?> UpdateAsync(Reservation reservation)
        {
            var existing = await _db.Reservations.FindAsync(reservation.Id);
            if (existing == null) return null;

            existing.Customer = reservation.Customer;
            existing.Date = reservation.Date;
            existing.Status = reservation.Status;
            existing.ProductId = reservation.ProductId;
            existing.UserId = reservation.UserId;

            await _db.SaveChangesAsync();
            return existing;
        }



        // ======== Delete
        public async Task<bool> DeleteAsync(Guid id)
        {
            var existing = await _db.Reservations.FindAsync(id);
            if (existing == null) return false;

            _db.Reservations.Remove(existing);
            await _db.SaveChangesAsync();
            return true;
        }



        // ======== Count
        public async Task<int> CountAsync()
        {
            return await _db.Reservations.CountAsync();
        }
    }
}
