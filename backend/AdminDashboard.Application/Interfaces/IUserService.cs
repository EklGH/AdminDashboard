using AdminDashboard.Application.Dtos;
using AdminDashboard.Domain.Entities;

namespace AdminDashboard.Application.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> RegisterAsync(RegisterRequestDto dto, Role role);
        Task<AuthResponseDto> LoginAsync(LoginRequestDto dto);
        Task<AuthResponseDto> RefreshAsync(RefreshTokenRequestDto dto);
        Task LogoutAsync(RefreshTokenRequestDto dto);
        Task<UserDto?> GetByIdAsync(Guid id);
        Task<List<UserDto>> GetAllAsync();
        Task UpdateAsync(Guid id, RegisterRequestDto dto);
        Task DeleteAsync(Guid id);
    }
}
