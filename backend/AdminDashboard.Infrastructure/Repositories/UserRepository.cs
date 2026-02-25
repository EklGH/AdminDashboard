using AdminDashboard.Domain.Entities;
using AdminDashboard.Application.Interfaces;
using AdminDashboard.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AdminDashboard.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _db;

        // Constructeur
        public UserRepository(AppDbContext db)
        {
            _db = db;
        }



        // ======== GetById
        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await _db.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Id == id);
        }



        // ======== GetByEmail
        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _db.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email == email);
        }



        // ======== EmailExists
        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _db.Users
                .AnyAsync(u => u.Email == email);
        }



        // ======== Create
        public async Task<User> CreateAsync(User user)
        {
            user.Id = Guid.NewGuid();

            await _db.Users.AddAsync(user);
            await _db.SaveChangesAsync();

            return user;
        }



        // ======== Update
        public async Task UpdateAsync(User user)
        {
            _db.Users.Update(user);
            await _db.SaveChangesAsync();
        }



        // ======== Delete
        public async Task DeleteAsync(Guid id)
        {
            var user = await _db.Users.FindAsync(id);
            if (user == null)
                return;

            _db.Users.Remove(user);
            await _db.SaveChangesAsync();
        }



        // ======== GetAll
        public async Task<List<User>> GetAllAsync()
        {
            return await _db.Users
                .Include(u => u.Role)
                .ToListAsync();
        }



        // ======== GetDefaultRole
        public async Task<Role?> GetDefaultRoleAsync()
        {
            // Rôle par défaut "User"
            var role = await _db.Roles.FirstOrDefaultAsync(r => r.Name == "User");

            // Si inexistant, création du rôle
            if (role == null)
            {
                role = new Role
                {
                    Id = Guid.NewGuid(),
                    Name = "User"
                };
                await _db.Roles.AddAsync(role);
                await _db.SaveChangesAsync();
            }

            return role;
        }
    }
}
