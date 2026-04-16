using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace AdminDashboard.Infrastructure.Persistence
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var host = Environment.GetEnvironmentVariable("Postgres__Host") ?? "localhost";
            var port = Environment.GetEnvironmentVariable("Postgres__Port") ?? "5432";
            var db = Environment.GetEnvironmentVariable("Postgres__Database") ?? "admindashboard";
            var user = Environment.GetEnvironmentVariable("Postgres__User") ?? "postgres";
            var password = Environment.GetEnvironmentVariable("Postgres__Password") ?? "postgres";

            // Chaîne de connexion "par défaut" pour design time
            var connectionString = $"Host={host};Port={port};Database={db};Username={user};Password={password}";

            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseNpgsql(connectionString);

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
