using System.ComponentModel.DataAnnotations;

namespace AdminDashboard.Api.GraphQL
{
    // Enum de tri pour Product
    public enum ProductOrderBy
    {
        NAME_ASC,
        NAME_DESC,
        PRICE_ASC,
        PRICE_DESC,
        STOCK_ASC,
        STOCK_DESC
    }



    // Input Product
    public class ProductQueryInputs
    {
        [Range(1, int.MaxValue, ErrorMessage = "La page doit être >= 1")]
        public int Page { get; set; } = 1;

        [Range(1, 100, ErrorMessage = "La taille de page doit être comprise entre 1 et 100")]
        public int PageSize { get; set; } = 10;

        [MaxLength(50, ErrorMessage = "La catégorie ne peut pas dépasser 50 caractères")]
        public string? Category { get; set; }

        public ProductOrderBy? OrderBy { get; set; }
    }
}
