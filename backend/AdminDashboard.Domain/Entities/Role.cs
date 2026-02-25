namespace AdminDashboard.Domain.Entities
{
    public class Role
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;


        // Relation liste utilisateurs
        public ICollection<User> Users { get; set; } = new List<User>();
    }
}