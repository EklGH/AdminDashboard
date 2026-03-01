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




        // ======== AUTH ========

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




        // ======== PRODUCTS ========

        // ======== Test créer un produit
        [Fact]
        public async Task CreateAsync_ShouldPersistProduct()
        {
            // Arrange
            using var context = DbContextTestFactory.Create(nameof(CreateAsync_ShouldPersistProduct));
            var repo = new ProductRepository(context);

            var product = new Product
            {
                Name = "Laptop",
                Category = "Electronics",
                Price = 1500,
                Stock = 10
            };

            // Act
            var created = await repo.CreateAsync(product);

            // Assert
            var dbProduct = await context.Products.FindAsync(created.Id);
            Assert.NotNull(dbProduct);
            Assert.Equal("Laptop", dbProduct!.Name);
            Assert.Equal(10, dbProduct.Stock);
        }



        // ======== Test pagination produits
        [Fact]
        public async Task GetPaginatedAsync_ShouldReturnCorrectPage()
        {
            // Arrange
            using var context = DbContextTestFactory.Create(nameof(GetPaginatedAsync_ShouldReturnCorrectPage));
            var repo = new ProductRepository(context);

            for (int i = 1; i <= 10; i++)
            {
                context.Products.Add(new Product
                {
                    Id = Guid.NewGuid(),
                    Name = $"Product {i}",
                    Category = "Category",
                    Price = 10,
                    Stock = 5
                });
            }

            await context.SaveChangesAsync();

            // Act
            var (items, total) = await repo.GetPaginatedAsync(2, 3);

            // Assert
            Assert.Equal(10, total);
            Assert.Equal(3, items.Count());
        }



        // ======== Test update produit
        [Fact]
        public async Task UpdateAsync_ShouldUpdateProductFields()
        {
            // Arrange
            using var context = DbContextTestFactory.Create(nameof(UpdateAsync_ShouldUpdateProductFields));
            var repo = new ProductRepository(context);

            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = "Old Name",
                Category = "Old Category",
                Price = 10,
                Stock = 1
            };

            context.Products.Add(product);
            await context.SaveChangesAsync();

            product.Name = "New Name";
            product.Stock = 99;

            // Act
            var updated = await repo.UpdateAsync(product);

            // Assert
            Assert.Equal("New Name", updated.Name);
            Assert.Equal(99, updated.Stock);
        }



        // ======== Test suppression produit
        [Fact]
        public async Task DeleteAsync_ShouldRemoveProduct()
        {
            // Arrange
            using var context = DbContextTestFactory.Create(nameof(DeleteAsync_ShouldRemoveProduct));
            var repo = new ProductRepository(context);

            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = "ToDelete",
                Category = "Category",
                Price = 10,
                Stock = 5
            };

            context.Products.Add(product);
            await context.SaveChangesAsync();

            // Act
            await repo.DeleteAsync(product.Id);

            // Assert
            Assert.False(await repo.ExistsAsync(product.Id));
        }




        // ======== RESERVATION ========

        // ======== Test créer une réservation avec relations
        [Fact]
        public async Task CreateAsync_ShouldPersistReservation_WithProductAndUser()
        {
            // Arrange
            using var context = DbContextTestFactory.Create(nameof(CreateAsync_ShouldPersistReservation_WithProductAndUser));
            var repo = new ReservationRepository(context);

            var role = CreateRole(context);
            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = "reservation@test.com",
                PasswordHash = "hash",
                RoleId = role.Id
            };

            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = "Phone",
                Category = "Electronics",
                Price = 800,
                Stock = 5
            };

            context.Users.Add(user);
            context.Products.Add(product);
            await context.SaveChangesAsync();

            var reservation = new Reservation
            {
                Customer = "John Doe",
                Date = DateTime.UtcNow,
                Status = "Confirmed",
                ProductId = product.Id,
                UserId = user.Id
            };

            // Act
            var created = await repo.CreateAsync(reservation);

            // Assert
            var dbReservation = await context.Reservations
                .Include(r => r.Product)
                .Include(r => r.User)
                .FirstAsync(r => r.Id == created.Id);

            Assert.Equal("Phone", dbReservation.Product.Name);
            Assert.Equal("reservation@test.com", dbReservation.User!.Email);
        }



        // ======== Test update réservation
        [Fact]
        public async Task UpdateAsync_ShouldUpdateReservationFields()
        {
            // Arrange
            using var context = DbContextTestFactory.Create(nameof(UpdateAsync_ShouldUpdateReservationFields));
            var repo = new ReservationRepository(context);

            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = "Tablet",
                Category = "Tech",
                Price = 300,
                Stock = 10
            };

            context.Products.Add(product);
            await context.SaveChangesAsync();

            var reservation = new Reservation
            {
                Id = Guid.NewGuid(),
                Customer = "Old Customer",
                Date = DateTime.UtcNow,
                Status = "Pending",
                ProductId = product.Id
            };

            context.Reservations.Add(reservation);
            await context.SaveChangesAsync();

            reservation.Status = "Completed";

            // Act
            var updated = await repo.UpdateAsync(reservation);

            // Assert
            Assert.NotNull(updated);
            Assert.Equal("Completed", updated!.Status);
        }



        // ======== Test suppression réservation
        [Fact]
        public async Task DeleteAsync_ShouldDeleteReservation()
        {
            // Arrange
            using var context = DbContextTestFactory.Create(nameof(DeleteAsync_ShouldDeleteReservation));
            var repo = new ReservationRepository(context);

            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = "Mouse",
                Category = "Tech",
                Price = 50,
                Stock = 20
            };

            context.Products.Add(product);
            await context.SaveChangesAsync();

            var reservation = new Reservation
            {
                Id = Guid.NewGuid(),
                Customer = "Delete",
                Date = DateTime.UtcNow,
                Status = "Pending",
                ProductId = product.Id
            };

            context.Reservations.Add(reservation);
            await context.SaveChangesAsync();

            // Act
            var result = await repo.DeleteAsync(reservation.Id);

            // Assert
            Assert.True(result);
            Assert.Empty(context.Reservations);
        }



        // ======== Test suppression produit cascade réservations
        [Fact]
        public async Task DeleteProduct_ShouldCascadeDeleteReservations()
        {
            // Arrange
            using var context = DbContextTestFactory.Create(nameof(DeleteProduct_ShouldCascadeDeleteReservations));

            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = "Cascade Product",
                Category = "Test",
                Price = 10,
                Stock = 5
            };

            var reservation = new Reservation
            {
                Id = Guid.NewGuid(),
                Customer = "Cascade User",
                Date = DateTime.UtcNow,
                Status = "Pending",
                ProductId = product.Id
            };

            product.Reservations.Add(reservation);
            context.Products.Add(product);
            await context.SaveChangesAsync();

            // Act
            context.Products.Remove(product);
            await context.SaveChangesAsync();

            // Assert
            Assert.Empty(context.Reservations);
        }
    }
}
