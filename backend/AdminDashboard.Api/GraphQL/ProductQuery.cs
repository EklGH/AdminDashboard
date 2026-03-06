using AdminDashboard.Application.Dtos;
using AdminDashboard.Application.Interfaces;
using HotChocolate.Authorization;

namespace AdminDashboard.Api.GraphQL
{
    [Authorize]
    public class ProductQuery
    {
        private readonly IProductService _service;


        // Constructeur
        public ProductQuery(IProductService service)
        {
            _service = service;
        }



        // ======== GetProducts
        [UsePaging(typeof(ProductType))]
        [UseFiltering]
        [UseSorting]
        public async Task<IEnumerable<ProductResponseDto>> GetProductsAsync()
        {
            var products = await _service.GetAllAsync();
            return products.ToList();
        }



        // ======== GetPaginatedProducts
        public async Task<PaginatedProductResponseDto> GetPaginatedProductsAsync(ProductQueryInputs input)
        {
            // Récupération de tous les produits
            var products = await _service.GetAllAsync();

            // Filtrage par catégorie si spécifié
            if (!string.IsNullOrEmpty(input.Category))
            {
                products = products
                    .Where(p => p.Category.Equals(input.Category, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            // Tri selon le paramètre OrderBy
            products = input.OrderBy switch
            {
                ProductOrderBy.NAME_ASC => products.OrderBy(p => p.Name).ToList(),
                ProductOrderBy.NAME_DESC => products.OrderByDescending(p => p.Name).ToList(),
                ProductOrderBy.PRICE_ASC => products.OrderBy(p => p.Price).ToList(),
                ProductOrderBy.PRICE_DESC => products.OrderByDescending(p => p.Price).ToList(),
                ProductOrderBy.STOCK_ASC => products.OrderBy(p => p.Stock).ToList(),
                ProductOrderBy.STOCK_DESC => products.OrderByDescending(p => p.Stock).ToList(),
                _ => products
            };

            // Pagination
            int skip = (input.Page - 1) * input.PageSize;
            var pagedItems = products.Skip(skip).Take(input.PageSize).ToList();

            return new PaginatedProductResponseDto
            {
                Items = pagedItems,
                Page = input.Page,
                PageSize = input.PageSize,
                TotalItems = products.Count()
            };
        }
    }
}
