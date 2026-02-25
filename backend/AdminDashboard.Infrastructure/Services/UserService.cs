using AdminDashboard.Application.Dtos;
using AdminDashboard.Domain.Entities;
using AdminDashboard.Application.Interfaces;

namespace AdminDashboard.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepo;
        private readonly IJwtService _jwtService;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly IPasswordHasher _passwordHasher;

        // Constructeur
        public UserService(
            IUserRepository userRepo,
            IJwtService jwtService,
            IRefreshTokenService refreshTokenService,
            IPasswordHasher passwordHasher)
        {
            _userRepo = userRepo;
            _jwtService = jwtService;
            _refreshTokenService = refreshTokenService;
            _passwordHasher = passwordHasher;
        }



        // ======== Inscription
        public async Task<UserDto> RegisterAsync(RegisterRequestDto dto, Role role)
        {
            // Vérifie si l'email est déjà utilisé
            if (await _userRepo.EmailExistsAsync(dto.Email))
                throw new Exception("Email déjà utilisé");

            // Récupère le rôle par défaut
            var defaultRole = await _userRepo.GetDefaultRoleAsync();
            if (defaultRole == null)
                throw new Exception("Rôle par défaut introuvable");

            // Création d'un nouvel utilisateur avec hash mdp
            var user = new User
            {
                Email = dto.Email,
                PasswordHash = _passwordHasher.HashPassword(dto.Password),
                RoleId = role.Id
            };

            // Ajout en base
            var createdUser = await _userRepo.CreateAsync(user);

            return new UserDto
            {
                Email = createdUser.Email
            };
        }



        // ======== Login
        public async Task<AuthResponseDto> LoginAsync(LoginRequestDto dto)
        {
            var user = await _userRepo.GetByEmailAsync(dto.Email);

            // Vérifie le mdp
            if (user == null || !_passwordHasher.VerifyPassword(dto.Password, user.PasswordHash))
                throw new Exception("Identifiants invalides");

            // Génère un JWT et un refresh token
            var accessToken = _jwtService.GenerateToken(user);
            var refreshToken = await _refreshTokenService.GenerateAsync(user);

            return new AuthResponseDto
            {
                User = new UserDto { Email = user.Email },
                Token = accessToken,
                RefreshToken = refreshToken.Token
            };
        }



        // ======== Refresh Jwt
        public async Task<AuthResponseDto> RefreshAsync(RefreshTokenRequestDto dto)
        {
            var user = await _refreshTokenService.ValidateTokenAsync(dto.RefreshToken);
            if (user == null)
                throw new Exception("Refresh token invalide ou expiré");

            // Supprime l’ancien token
            await _refreshTokenService.RevokeTokenAsync(dto.RefreshToken);

            // Génère un refresh token et un JWT
            var newRefresh = await _refreshTokenService.GenerateAsync(user);
            var newAccess = _jwtService.GenerateToken(user);

            return new AuthResponseDto
            {
                User = new UserDto { Email = user.Email },
                Token = newAccess,
                RefreshToken = newRefresh.Token
            };
        }



        // ======== Logout
        public async Task LogoutAsync(RefreshTokenRequestDto dto)
        {
            await _refreshTokenService.RevokeTokenAsync(dto.RefreshToken);
        }



        // ======== GetById User
        public async Task<UserDto?> GetByIdAsync(Guid id)
        {
            var user = await _userRepo.GetByIdAsync(id);
            if (user == null) return null;

            return new UserDto { Email = user.Email };
        }



        // ======== GetAll Users
        public async Task<List<UserDto>> GetAllAsync()
        {
            var users = await _userRepo.GetAllAsync();
            return users.Select(u => new UserDto { Email = u.Email }).ToList();
        }



        // ======== Update User
        public async Task UpdateAsync(Guid id, RegisterRequestDto dto)
        {
            var user = await _userRepo.GetByIdAsync(id);
            if (user == null) throw new Exception("Utilisateur introuvable");

            user.Email = dto.Email;
            if (!string.IsNullOrEmpty(dto.Password))
                user.PasswordHash = _passwordHasher.HashPassword(dto.Password);

            await _userRepo.UpdateAsync(user);
        }



        // ======== Delete User
        public async Task DeleteAsync(Guid id)
        {
            await _userRepo.DeleteAsync(id);
            await _refreshTokenService.RevokeAllForUserAsync(id);
        }
    }
}
