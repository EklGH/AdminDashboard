using AdminDashboard.Application.Interfaces;

namespace AdminDashboard.Infrastructure.Auth
{
    public class PasswordHasher : IPasswordHasher
    {
        // ======== Génère un hash à partir d'un mdp
        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, workFactor: 12);
        }



        // ======== Vérifie qu'un mdp correspond à un hash existant
        public bool VerifyPassword(string password, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }
    }
}
