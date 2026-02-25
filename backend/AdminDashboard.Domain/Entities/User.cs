namespace AdminDashboard.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = default!;
        public string PasswordHash { get; set; } = default!;
        public Guid RoleId { get; set; }
        public Role Role { get; set; } = default!;


        // Relation liste des réservations et refreshtokens
        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    }
}