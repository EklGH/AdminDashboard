using System.ComponentModel.DataAnnotations;

namespace AdminDashboard.Application.Dtos
{
    // ======== DTOs Request
    
    // Login
    public class LoginRequestDto
    {
        [Required(ErrorMessage = "Email requis")]
        [EmailAddress(ErrorMessage = "Format d'email invalide")]
        public string Email { get; set; } = default!;

        [Required(ErrorMessage = "Mot de passe requis")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Mot de passe entre 6 et 100 caractères")]
        public string Password { get; set; } = default!;
    }

    // Register
    public class RegisterRequestDto
    {
        [Required(ErrorMessage = "Email requis")]
        [EmailAddress(ErrorMessage = "Format d'email invalide")]
        public string Email { get; set; } = default!;

        [Required(ErrorMessage = "Mot de passe requis")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Mot de passe entre 6 et 100 caractères")]
        public string Password { get; set; } = default!;
    }

    // Refresh Token
    public class RefreshTokenRequestDto
    {
        [Required(ErrorMessage = "Refresh token requis")]
        public string RefreshToken { get; set; } = default!;
    }

    /*
    // Logout (future)
    public class LogoutRequestDto
    {
        [Required(ErrorMessage = "Refresh token requis")]
        public string RefreshToken { get; set; } = default!;
    }
    */


    // ======== DTOs Response

    // User
    public class UserDto
    {
        public string Email { get; set; } = default!;
    }

    // Auth (réponse login/refresh)
    public class AuthResponseDto
    {
        public UserDto User { get; set; } = default!;
        public string Token { get; set; } = default!;
        public string RefreshToken { get; set; } = default!;
    }


    /*
    // Lister les users (future)
    public class UserListItemDto
    {
        public string Email { get; set; } = default!;
    }

    // Détails sur un user (future)
    public class UserDetailsDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = default!;
        public string Role { get; set; } = default!;
    }
    */
}
