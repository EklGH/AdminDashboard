namespace AdminDashboard.Domain.Entities
{
    public class RefreshToken
    {
        public Guid Id { get; set; }
        public string Token { get; set; } = default!;
        public DateTime ExpiresAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsExpired => DateTime.UtcNow >= ExpiresAt;


        // Relations utilisateur
        public Guid UserId { get; set; }
        public User User { get; set; } = default!;
    }
}
