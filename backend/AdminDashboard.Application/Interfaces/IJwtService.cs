using AdminDashboard.Domain.Entities;

namespace AdminDashboard.Application.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(User user);
    }
}
