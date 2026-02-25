using AdminDashboard.Application.Dtos;
using AdminDashboard.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AdminDashboard.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IUserRepository _userRepo;

        // Constructeur
        public AuthController(IUserService userService, IUserRepository userRepo)
        {
            _userService = userService;
            _userRepo = userRepo;
        }



        // ======== Register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto dto)
        {
            try
            {
                // Récupère rôle par défaut
                var defaultRole = await _userRepo.GetDefaultRoleAsync();
                if (defaultRole == null)
                    return BadRequest(new { message = "Impossible de trouver ou créer le rôle par défaut" });

                var userDto = await _userService.RegisterAsync(dto, defaultRole);
                return Ok(userDto);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }



        // ======== Login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
        {
            try
            {
                var authResponse = await _userService.LoginAsync(dto);
                return Ok(authResponse);
            }
            catch (Exception ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }



        // ======== Refresh
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequestDto dto)
        {
            try
            {
                var authResponse = await _userService.RefreshAsync(dto);
                return Ok(authResponse);
            }
            catch (Exception ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }



        /*
        // ======== Logout
        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] RefreshTokenRequestDto dto)
        {
            try
            {
                await _userService.LogoutAsync(dto);
                return Ok(new { message = "Déconnecté avec succès" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        */
    }
}
