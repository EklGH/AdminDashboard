namespace AdminDashboard.Domain.Entities
{
    public class Reservation
    {
        public Guid Id { get; set; }
        public string Customer { get; set; } = default!;
        public DateTime Date { get; set; }
        public string Status { get; set; } = default!;


        // Relations produit et utilisateur
        public Guid ProductId { get; set; }
        public Product Product { get; set; } = default!;

        public Guid? UserId { get; set; }
        public User? User { get; set; } = default!;
    }
}
