using AdminDashboard.Domain.Entities;
using AdminDashboard.Infrastructure.Persistence;
using AdminDashboard.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AdminDashboard.Tests.Unit
{
    public class EFCoreInMemoryTests
    {
        // DbContext Factory
        private static class DbContextTestFactory
        {
            public static AppDbContext Create(string dbName)
            {
                var options = new DbContextOptionsBuilder<AppDbContext>()
                    .UseInMemoryDatabase(dbName)
                    .EnableSensitiveDataLogging()
                    .Options;

                return new AppDbContext(options);
            }
        }


        // Créer un rôle (helper pour tests)
        private static Role CreateRole(AppDbContext context, string name = "User")
        {
            var role = new Role
            {
                Id = Guid.NewGuid(),
                Name = name
            };

            context.Roles.Add(role);
            context.SaveChanges();

            return role;
        }



        // ======== Test créer utilisateur avec rôle
        [Fact]
        public async Task CreateAsync_ShouldPersistUser_WithRole()
        {
            // Arrange
            using var context = DbContextTestFactory.Create(nameof(CreateAsync_ShouldPersistUser_WithRole));
            var repo = new UserRepository(context);

            var role = CreateRole(context, "Admin");

            var user = new User
            {
                Email = "test@test.com",
                PasswordHash = "hashed",
                RoleId = role.Id
            };

            // Act
            await repo.CreateAsync(user);

            // Assert
            var dbUser = await context.Users
                .Include(u => u.Role)
                .SingleAsync();

            Assert.Equal("test@test.com", dbUser.Email);
            Assert.Equal("Admin", dbUser.Role.Name);
        }



        // ======== Test vérifier existence email
        [Fact]
        public async Task EmailExistsAsync_ShouldReturnTrue_WhenEmailExists()
        {
            // Arrange
            using var context = DbContextTestFactory.Create(nameof(EmailExistsAsync_ShouldReturnTrue_WhenEmailExists));
            var repo = new UserRepository(context);

            var role = CreateRole(context);

            context.Users.Add(new User
            {
                Id = Guid.NewGuid(),
                Email = "exists@test.com",
                PasswordHash = "hash",
                RoleId = role.Id
            });

            await context.SaveChangesAsync();

            // Act
            var exists = await repo.EmailExistsAsync("exists@test.com");

            // Assert
            Assert.True(exists);
        }



        // ======== Test récupérer utilisateur par email avec rôle
        [Fact]
        public async Task GetByEmailAsync_ShouldReturnUser_WithRole()
        {
            // Arrange
            using var context = DbContextTestFactory.Create(nameof(GetByEmailAsync_ShouldReturnUser_WithRole));
            var repo = new UserRepository(context);

            var role = CreateRole(context);

            context.Users.Add(new User
            {
                Id = Guid.NewGuid(),
                Email = "user@test.com",
                PasswordHash = "hash",
                RoleId = role.Id
            });

            await context.SaveChangesAsync();

            // Act
            var user = await repo.GetByEmailAsync("user@test.com");

            // Assert
            Assert.NotNull(user);
            Assert.NotNull(user!.Role);
            Assert.Equal("User", user.Role.Name);
        }



        // ======== Test supprimer utilisateur existant
        [Fact]
        public async Task DeleteAsync_ShouldRemoveUser_WhenUserExists()
        {
            // Arrange
            using var context = DbContextTestFactory.Create(nameof(DeleteAsync_ShouldRemoveUser_WhenUserExists));
            var repo = new UserRepository(context);

            var role = CreateRole(context);
            var userId = Guid.NewGuid();

            context.Users.Add(new User
            {
                Id = userId,
                Email = "delete@test.com",
                PasswordHash = "hash",
                RoleId = role.Id
            });

            await context.SaveChangesAsync();

            // Act
            await repo.DeleteAsync(userId);

            // Assert
            Assert.Null(await context.Users.FindAsync(userId));
        }



        // ======== Test supprimer utilisateur inexistant ne plante pas
        [Fact]
        public async Task DeleteAsync_ShouldNotThrow_WhenUserDoesNotExist()
        {
            // Arrange
            using var context = DbContextTestFactory.Create(nameof(DeleteAsync_ShouldNotThrow_WhenUserDoesNotExist));
            var repo = new UserRepository(context);

            // Act / Assert
            var exception = await Record.ExceptionAsync(() =>
                repo.DeleteAsync(Guid.NewGuid())
            );

            Assert.Null(exception);
        }



        // ======== Test créer rôle par défaut si absent
        [Fact]
        public async Task GetDefaultRoleAsync_ShouldCreateRole_WhenNotExists()
        {
            // Arrange
            using var context = DbContextTestFactory.Create(nameof(GetDefaultRoleAsync_ShouldCreateRole_WhenNotExists));
            var repo = new UserRepository(context);

            // Act
            var role = await repo.GetDefaultRoleAsync();

            // Assert
            Assert.NotNull(role);
            Assert.Equal("User", role!.Name);
            Assert.Single(context.Roles);
        }



        // ======== Test retourner rôle par défaut existant
        [Fact]
        public async Task GetDefaultRoleAsync_ShouldReturnExistingRole_WhenExists()
        {
            // Arrange
            using var context = DbContextTestFactory.Create(nameof(GetDefaultRoleAsync_ShouldReturnExistingRole_WhenExists));
            var repo = new UserRepository(context);

            CreateRole(context, "User");

            // Act
            var role = await repo.GetDefaultRoleAsync();

            // Assert
            Assert.Single(context.Roles);
            Assert.Equal("User", role!.Name);
        }



        // ======== Test supprimer tokens en cascade lors suppression utilisateur
        [Fact]
        public async Task RefreshTokens_ShouldCascadeDelete_WhenUserDeleted()
        {
            // Arrange
            using var context = DbContextTestFactory.Create(nameof(RefreshTokens_ShouldCascadeDelete_WhenUserDeleted));

            var role = CreateRole(context);

            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = "token@test.com",
                PasswordHash = "hash",
                RoleId = role.Id
            };

            user.RefreshTokens.Add(new RefreshToken
            {
                Id = Guid.NewGuid(),
                Token = "refresh-token",
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(1)
            });

            context.Users.Add(user);
            await context.SaveChangesAsync();

            // Act
            context.Users.Remove(user);
            await context.SaveChangesAsync();

            // Assert
            Assert.Empty(context.RefreshTokens);
        }
    }
}
