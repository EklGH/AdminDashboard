using AdminDashboard.Application.Interfaces;
using AdminDashboard.Domain.Entities;
using AdminDashboard.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AdminDashboard.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _db;

        // Constructeur
        public ProductRepository(AppDbContext db)
        {
            _db = db;
        }



        // ======== GetAll
        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _db.Products.ToListAsync();
        }



        // ======== GetPaginated
        public async Task<(IEnumerable<Product> Items, int TotalItems)> GetPaginatedAsync(int page, int pageSize)
        {
            var totalItems = await _db.Products.CountAsync();
            var items = await _db.Products
                                 .OrderBy(p => p.Name)
                                 .Skip((page - 1) * pageSize)
                                 .Take(pageSize)
                                 .ToListAsync();

            return (items, totalItems);
        }



        // ======== GetById
        public async Task<Product?> GetByIdAsync(Guid id)
        {
            return await _db.Products.FindAsync(id);
        }



        // ======== Create
        public async Task<Product> CreateAsync(Product product)
        {
            product.Id = Guid.NewGuid();
            _db.Products.Add(product);
            await _db.SaveChangesAsync();
            return product;
        }



        // ======== Update
        public async Task<Product> UpdateAsync(Product product)
        {
            var existing = await _db.Products.FindAsync(product.Id);
            if (existing == null) throw new KeyNotFoundException("Product not found");

            existing.Name = product.Name;
            existing.Category = product.Category;
            existing.Price = product.Price;
            existing.Stock = product.Stock;

            await _db.SaveChangesAsync();
            return existing;
        }



        // ======== Delete
        public async Task DeleteAsync(Guid id)
        {
            var product = await _db.Products.FindAsync(id);
            if (product == null) throw new KeyNotFoundException("Product not found");

            _db.Products.Remove(product);
            await _db.SaveChangesAsync();
        }



        // ======== Exists
        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _db.Products.AnyAsync(p => p.Id == id);
        }
    }
}
