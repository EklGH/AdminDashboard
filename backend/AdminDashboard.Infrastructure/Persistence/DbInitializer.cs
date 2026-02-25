using AdminDashboard.Application.Dtos;
using AdminDashboard.Application.Interfaces;
using AdminDashboard.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AdminDashboard.Infrastructure.Persistence
{
    public static class DbInitializer
    {
        // ======== Création d'un Admin par défaut
        public static async Task InitializeAsync(AppDbContext context, IUserService userService)
        {
            // Assure que la DB est créée
            await context.Database.MigrateAsync();

            // Crée rôle Admin si inexistant
            var adminRole = await context.Roles.FirstOrDefaultAsync(r => r.Name == "Admin");
            if (adminRole == null)
            {
                adminRole = new Role
                {
                    Id = Guid.NewGuid(),
                    Name = "Admin"
                };
                await context.Roles.AddAsync(adminRole);
                await context.SaveChangesAsync();
            }

            // Crée utilisateur admin si inexistant
            var adminEmail = "admin@test.com"; // Email par défaut
            var existingAdmin = await context.Users.FirstOrDefaultAsync(u => u.Email == adminEmail);
            if (existingAdmin == null)
            {
                var registerDto = new RegisterRequestDto
                {
                    Email = adminEmail,
                    Password = "Admin123" // mot de passe par défaut
                };

                await userService.RegisterAsync(registerDto, adminRole);
                Console.WriteLine("Admin par défaut créé");
            }
            else
            {
                Console.WriteLine("Admin déjà présent");
            } 
        }
    }
}
