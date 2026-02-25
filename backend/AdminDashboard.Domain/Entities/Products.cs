namespace AdminDashboard.Domain.Entities
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string Category { get; set; } = default!;
        public decimal Price { get; set; }
        public int Stock { get; set; }


        // Relation réservation
        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}
