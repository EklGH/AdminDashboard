using System.ComponentModel.DataAnnotations;

namespace AdminDashboard.Application.Dtos
{
    // ======== DTOs Request

    // Create
    public class ReservationCreateDto
    {
        [Required]
        [StringLength(200, ErrorMessage = "Le nom du client ne peut dépasser 200 caractères.")]
        public string Customer { get; set; } = default!;

        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required]
        [RegularExpression("Confirmed|Pending|Cancelled", ErrorMessage = "Status doit être Confirmed, Pending ou Cancelled.")]
        public string Status { get; set; } = default!;

        [Required]
        public Guid ProductId { get; set; }

        public Guid? UserId { get; set; }
    }

    // Update
    public class ReservationUpdateDto
    {
        [Required]
        [StringLength(200, ErrorMessage = "Le nom du client ne peut dépasser 200 caractères.")]
        public string Customer { get; set; } = default!;

        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required]
        [RegularExpression("Confirmed|Pending|Cancelled", ErrorMessage = "Status doit être Confirmed, Pending ou Cancelled.")]
        public string Status { get; set; } = default!;

        [Required]
        public Guid ProductId { get; set; }

        public Guid? UserId { get; set; }
    }



    // ======== DTOs Response

    // Reservation
    public class ReservationResponseDto
    {
        public Guid Id { get; set; }
        public string Customer { get; set; } = default!;
        public string Date { get; set; } = default!; // (YYYY-MM-DD)
        public string Status { get; set; } = default!;
        public Guid ProductId { get; set; }
        public Guid? UserId { get; set; }
    }

    // Pagination
    public class PaginatedReservationResponseDto
    {
        public IEnumerable<ReservationResponseDto> Items { get; set; } = new List<ReservationResponseDto>();
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
    }
}
