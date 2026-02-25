using AdminDashboard.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AdminDashboard.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        // Constructeur
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        // Dbsets
        public DbSet<Product> Products => Set<Product>();
        public DbSet<Reservation> Reservations => Set<Reservation>();
        public DbSet<User> Users => Set<User>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();



        // ======== Configuration Entities & Relations
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Product
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(p => p.Id);

                entity.Property(p => p.Name)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(p => p.Category)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(p => p.Price)
                    .HasColumnType("decimal(18,2)");

                entity.Property(p => p.Stock)
                    .IsRequired();


                // Relation Reservations
                entity.HasMany(p => p.Reservations)
                      .WithOne(r => r.Product)
                      .HasForeignKey(r => r.ProductId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Reservation
            modelBuilder.Entity<Reservation>(entity =>
            {
                entity.HasKey(r => r.Id);

                entity.Property(r => r.Customer)
                      .IsRequired()
                      .HasMaxLength(200);

                entity.Property(r => r.Status)
                      .IsRequired()
                      .HasMaxLength(50);

                entity.Property(r => r.Date)
                      .IsRequired();


                // Relations Product et User
                entity.HasOne(r => r.Product)
                      .WithMany(p => p.Reservations)
                      .HasForeignKey(r => r.ProductId);

                entity.HasOne(r => r.User)
                      .WithMany(u => u.Reservations)
                      .HasForeignKey(r => r.UserId)
                      .OnDelete(DeleteBehavior.SetNull);
            });

            // User
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);

                entity.Property(u => u.Email)
                      .IsRequired()
                      .HasMaxLength(200);

                entity.HasIndex(u => u.Email)
                      .IsUnique();

                entity.Property(u => u.PasswordHash)
                      .IsRequired();


                // Relations Role et RefreshToken
                entity.HasOne(u => u.Role)
                      .WithMany(r => r.Users)
                      .HasForeignKey(u => u.RoleId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(u => u.RefreshTokens)
                      .WithOne(t => t.User)
                      .HasForeignKey(t => t.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Role
            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(r => r.Id);

                entity.Property(r => r.Name)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.HasIndex(r => r.Name)
                      .IsUnique();
            });

            // RefreshToken
            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.HasKey(t => t.Id);
                entity.Property(t => t.Token).IsRequired();
                entity.Property(t => t.CreatedAt).IsRequired();
                entity.Property(t => t.ExpiresAt).IsRequired();
            });
        }
    }
}
