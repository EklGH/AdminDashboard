using AdminDashboard.Domain.Entities;

namespace AdminDashboard.Application.Interfaces
{
    public interface IRefreshTokenService
    {
        Task<RefreshToken> GenerateAsync(User user, int daysValid = 7);
        Task<User?> ValidateTokenAsync(string token);
        Task RevokeTokenAsync(string token);
        Task RevokeAllForUserAsync(Guid userId);
    }
}
