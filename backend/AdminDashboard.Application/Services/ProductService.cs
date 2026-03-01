using AdminDashboard.Application.Dtos;
using AdminDashboard.Application.Interfaces;
using AdminDashboard.Domain.Entities;

namespace AdminDashboard.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repo;

        // Constructeur
        public ProductService(IProductRepository repo)
        {
            _repo = repo;
        }



        // ======== GetAll Products
        public async Task<IEnumerable<ProductResponseDto>> GetAllAsync()
        {
            var products = await _repo.GetAllAsync();
            return products.Select(MapToDto);
        }



        // ======== GetPaginated Products
        public async Task<PaginatedProductResponseDto> GetPaginatedAsync(int page, int pageSize)
        {
            var (items, total) = await _repo.GetPaginatedAsync(page, pageSize);
            return new PaginatedProductResponseDto
            {
                Items = items.Select(MapToDto),
                Page = page,
                PageSize = pageSize,
                TotalItems = total
            };
        }



        // ======== GetById Product
        public async Task<ProductResponseDto?> GetByIdAsync(Guid id)
        {
            var product = await _repo.GetByIdAsync(id);
            return product == null ? null : MapToDto(product);
        }



        // ======== Create Product
        public async Task<ProductResponseDto> CreateAsync(ProductCreateDto dto)
        {
            var product = new Product
            {
                Name = dto.Name,
                Category = dto.Category,
                Price = dto.Price,
                Stock = dto.Stock < 0 ? 0 : dto.Stock
            };

            var created = await _repo.CreateAsync(product);
            return MapToDto(created);
        }



        // ======== Update Product
        public async Task<ProductResponseDto> UpdateAsync(Guid id, ProductUpdateDto dto)
        {
            var product = new Product
            {
                Id = id,
                Name = dto.Name,
                Category = dto.Category,
                Price = dto.Price,
                Stock = dto.Stock < 0 ? 0 : dto.Stock
            };

            var updated = await _repo.UpdateAsync(product);
            return MapToDto(updated);
        }



        // ======== Delete Product
        public async Task DeleteAsync(Guid id)
        {
            await _repo.DeleteAsync(id);
        }



        // ======== Exists Product
        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _repo.ExistsAsync(id);
        }



        // ======== Mapping Product -> ProductResponseDto
        private static ProductResponseDto MapToDto(Product p) => new ProductResponseDto
        {
            Id = p.Id,
            Name = p.Name,
            Category = p.Category,
            Price = p.Price,
            Stock = p.Stock
        };
    }
}
