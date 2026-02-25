using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace AdminDashboard.Infrastructure.Persistence
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var host = Environment.GetEnvironmentVariable("POSTGRES_HOST") ?? "localhost";
            var port = Environment.GetEnvironmentVariable("POSTGRES_PORT") ?? "5432";
            var db = Environment.GetEnvironmentVariable("POSTGRES_DB") ?? "admindashboard";
            var user = Environment.GetEnvironmentVariable("POSTGRES_USER") ?? "postgres";
            var password = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD") ?? "postgres";

            // Chaîne de connexion "par défaut" pour design time
            var connectionString = $"Host={host};Port={port};Database={db};Username={user};Password={password}";

            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseNpgsql(connectionString);

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
