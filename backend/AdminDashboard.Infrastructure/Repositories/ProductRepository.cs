using AdminDashboard.Application.Interfaces;
using AdminDashboard.Domain.Entities;
using AdminDashboard.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AdminDashboard.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IDbContextFactory<AppDbContext> _dbFactory;

        // Constructeur
        public ProductRepository(IDbContextFactory<AppDbContext> dbFactory)
        {
            _dbFactory = dbFactory;
        }



        // ======== GetAll
        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            await using var db = await _dbFactory.CreateDbContextAsync();
            return await db.Products.ToListAsync();
        }



        // ======== GetPaginated
        public async Task<(IEnumerable<Product> Items, int TotalItems)> GetPaginatedAsync(int page, int pageSize)
        {
            await using var db = await _dbFactory.CreateDbContextAsync();
            var totalItems = await db.Products.CountAsync();
            var items = await db.Products
                                 .OrderBy(p => p.Name)
                                 .Skip((page - 1) * pageSize)
                                 .Take(pageSize)
                                 .ToListAsync();

            return (items, totalItems);
        }



        // ======== GetById
        public async Task<Product?> GetByIdAsync(Guid id)
        {
            await using var db = await _dbFactory.CreateDbContextAsync();
            return await db.Products.FindAsync(id);
        }



        // ======== Create
        public async Task<Product> CreateAsync(Product product)
        {
            await using var db = await _dbFactory.CreateDbContextAsync();
            product.Id = Guid.NewGuid();
            db.Products.Add(product);
            await db.SaveChangesAsync();
            return product;
        }



        // ======== Update
        public async Task<Product> UpdateAsync(Product product)
        {
            await using var db = await _dbFactory.CreateDbContextAsync();
            var existing = await db.Products.FindAsync(product.Id);
            if (existing == null) throw new KeyNotFoundException("Produit inexistant");

            existing.Name = product.Name;
            existing.Category = product.Category;
            existing.Price = product.Price;
            existing.Stock = product.Stock;

            await db.SaveChangesAsync();
            return existing;
        }



        // ======== Delete
        public async Task DeleteAsync(Guid id)
        {
            await using var db = await _dbFactory.CreateDbContextAsync();
            var product = await db.Products.FindAsync(id);
            if (product == null) throw new KeyNotFoundException("Produit inexistant");

            db.Products.Remove(product);
            await db.SaveChangesAsync();
        }



        // ======== Exists
        public async Task<bool> ExistsAsync(Guid id)
        {
            await using var db = await _dbFactory.CreateDbContextAsync();
            return await db.Products.AnyAsync(p => p.Id == id);
        }
    }
}
