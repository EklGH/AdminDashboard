using AdminDashboard.Domain.Entities;

namespace AdminDashboard.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(Guid id);
        Task<User?> GetByEmailAsync(string email);
        Task<bool> EmailExistsAsync(string email);
        Task<User> CreateAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(Guid id);
        Task<List<User>> GetAllAsync();
        Task<Role?> GetDefaultRoleAsync();
    }
}
