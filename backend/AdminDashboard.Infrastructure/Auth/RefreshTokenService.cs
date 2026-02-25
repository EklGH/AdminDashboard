using AdminDashboard.Domain.Entities;
using AdminDashboard.Application.Interfaces;
using AdminDashboard.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace AdminDashboard.Infrastructure.Auth
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly AppDbContext _db;

        // Constructeur
        public RefreshTokenService(AppDbContext db)
        {
            _db = db;
        }



        // ======== Génère un refresh token pour un user
        public async Task<RefreshToken> GenerateAsync(User user, int daysValid = 7)
        {
            var refreshToken = new RefreshToken
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(daysValid)
            };

            await _db.RefreshTokens.AddAsync(refreshToken);
            await _db.SaveChangesAsync();

            return refreshToken;
        }



        // ======== Valide un token existant
        public async Task<User?> ValidateTokenAsync(string token)
        {
            var refreshToken = await _db.RefreshTokens
                .Include(t => t.User)
                .ThenInclude(u => u.Role)
                .FirstOrDefaultAsync(t => t.Token == token);

            if (refreshToken == null || refreshToken.IsExpired)
                return null;

            return refreshToken.User;
        }



        // ======== Supprime un token
        public async Task RevokeTokenAsync(string token)
        {
            var refreshToken = await _db.RefreshTokens.FirstOrDefaultAsync(t => t.Token == token);
            if (refreshToken == null) return;

            _db.RefreshTokens.Remove(refreshToken);
            await _db.SaveChangesAsync();
        }



        // ======== Supprime tous les tokens d'un utilisateur (logout global)
        public async Task RevokeAllForUserAsync(Guid userId)
        {
            var tokens = await _db.RefreshTokens
                .Where(t => t.UserId == userId)
                .ToListAsync();

            _db.RefreshTokens.RemoveRange(tokens);
            await _db.SaveChangesAsync();
        }
    }
}
