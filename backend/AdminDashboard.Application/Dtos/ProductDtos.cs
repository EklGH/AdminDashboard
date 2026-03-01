using System.ComponentModel.DataAnnotations;

namespace AdminDashboard.Application.Dtos
{
    // ======== DTOs Request

    // Create
    public class ProductCreateDto
    {
        [Required(ErrorMessage = "Le nom est obligatoire")]
        [StringLength(200, ErrorMessage = "Le nom ne peut pas dépasser 200 caractères")]
        public string Name { get; set; } = default!;

        [Required(ErrorMessage = "La catégorie est obligatoire")]
        [StringLength(100, ErrorMessage = "La catégorie ne peut pas dépasser 100 caractères")]
        public string Category { get; set; } = default!;

        [Range(0, double.MaxValue, ErrorMessage = "Le prix doit être supérieur ou égal à 0")]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Le stock doit être supérieur ou égal à 0")]
        public int Stock { get; set; }
    }

    // Update
    public class ProductUpdateDto
    {
        [Required(ErrorMessage = "Le nom est obligatoire")]
        [StringLength(200, ErrorMessage = "Le nom ne peut pas dépasser 200 caractères")]
        public string Name { get; set; } = default!;

        [Required(ErrorMessage = "La catégorie est obligatoire")]
        [StringLength(100, ErrorMessage = "La catégorie ne peut pas dépasser 100 caractères")]
        public string Category { get; set; } = default!;

        [Range(0, double.MaxValue, ErrorMessage = "Le prix doit être supérieur ou égal à 0")]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Le stock doit être supérieur ou égal à 0")]
        public int Stock { get; set; }
    }



    // ======== DTOs Response

    // Product
    public class ProductResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string Category { get; set; } = default!;
        public decimal Price { get; set; }
        public int Stock { get; set; }
    }

    // Pagination
    public class PaginatedProductResponseDto
    {
        public IEnumerable<ProductResponseDto> Items { get; set; } = new List<ProductResponseDto>();
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
    }
}
