using AdminDashboard.Application.Dtos;
using AdminDashboard.Application.Interfaces;
using AdminDashboard.Domain.Entities;
using AdminDashboard.Infrastructure.Services;
using FluentAssertions;
using Moq;

namespace AdminDashboard.Tests.Unit
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _userRepoMock;
        private readonly Mock<IJwtService> _jwtMock;
        private readonly Mock<IRefreshTokenService> _refreshMock;
        private readonly Mock<IPasswordHasher> _hasherMock;
        private readonly UserService _userService;


        // Constructeur
        public UserServiceTests()
        {
            _userRepoMock = new Mock<IUserRepository>();
            _jwtMock = new Mock<IJwtService>();
            _refreshMock = new Mock<IRefreshTokenService>();
            _hasherMock = new Mock<IPasswordHasher>();

            _userService = new UserService(
                _userRepoMock.Object,
                _jwtMock.Object,
                _refreshMock.Object,
                _hasherMock.Object
            );
        }



        // ======== Test création utilisateur si email inexistant
        [Fact]
        public async Task RegisterAsync_ShouldCreateUser_WhenEmailNotExists()
        {
            // Arrange
            var dto = new RegisterRequestDto { Email = "test@test.com", Password = "pwd123" };
            var role = new Role { Id = Guid.NewGuid(), Name = "User" };

            _userRepoMock.Setup(r => r.EmailExistsAsync(dto.Email)).ReturnsAsync(false);
            _userRepoMock.Setup(r => r.GetDefaultRoleAsync()).ReturnsAsync(role);
            _hasherMock.Setup(h => h.HashPassword(dto.Password)).Returns("hashedpwd");
            _userRepoMock.Setup(r => r.CreateAsync(It.IsAny<User>()))
                .ReturnsAsync((User u) => u);

            // Act
            var result = await _userService.RegisterAsync(dto, role);

            // Assert
            result.Should().NotBeNull();
            result.Email.Should().Be(dto.Email);
            _userRepoMock.Verify(r => r.CreateAsync(It.Is<User>(u => u.Email == dto.Email && u.PasswordHash == "hashedpwd")), Times.Once);
        }



        // ======== Test exception si email déjà utilisé
        [Fact]
        public async Task RegisterAsync_ShouldThrow_WhenEmailExists()
        {
            // Arrange
            var dto = new RegisterRequestDto { Email = "exists@test.com", Password = "pwd123" };
            var role = new Role { Id = Guid.NewGuid(), Name = "User" };

            _userRepoMock.Setup(r => r.EmailExistsAsync(dto.Email)).ReturnsAsync(true);

            // Act
            Func<Task> act = async () => await _userService.RegisterAsync(dto, role);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("Email déjà utilisé");
        }



        // ======== Test login retourne token si identifiants valides
        [Fact]
        public async Task LoginAsync_ShouldReturnToken_WhenCredentialsValid()
        {
            // Arrange
            var user = new User { Id = Guid.NewGuid(), Email = "login@test.com", PasswordHash = "hashedpwd", RoleId = Guid.NewGuid(), Role = new Role { Id = Guid.NewGuid(), Name = "User" } };
            var dto = new LoginRequestDto { Email = user.Email, Password = "pwd123" };

            _userRepoMock.Setup(r => r.GetByEmailAsync(dto.Email)).ReturnsAsync(user);
            _hasherMock.Setup(h => h.VerifyPassword(dto.Password, user.PasswordHash)).Returns(true);
            _jwtMock.Setup(j => j.GenerateToken(user)).Returns("fake-jwt-token");
            _refreshMock.Setup(r => r.GenerateAsync(It.IsAny<User>(), It.IsAny<int>())).ReturnsAsync(new RefreshToken());

            // Act
            var result = await _userService.LoginAsync(dto);

            // Assert
            result.Should().NotBeNull();
            result.User.Email.Should().Be(user.Email);
            result.Token.Should().Be("fake-jwt-token");
        }



        // ======== Test login lève exception si mdp invalide
        [Fact]
        public async Task LoginAsync_ShouldThrow_WhenPasswordInvalid()
        {
            // Arrange
            var user = new User { Email = "user@test.com", PasswordHash = "hashedpwd", RoleId = Guid.NewGuid(), Role = new Role { Id = Guid.NewGuid(), Name = "User" } };
            var dto = new LoginRequestDto { Email = user.Email, Password = "wrongpwd" };

            _userRepoMock.Setup(r => r.GetByEmailAsync(dto.Email)).ReturnsAsync(user);
            _hasherMock.Setup(h => h.VerifyPassword(dto.Password, user.PasswordHash)).Returns(false);

            // Act
            Func<Task> act = async () => await _userService.LoginAsync(dto);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("Identifiants invalides");
        }



        // ======== Test GetById retourne utilisateur existant
        [Fact]
        public async Task GetByIdAsync_ShouldReturnUser_WhenExists()
        {
            // Arrange
            var user = new User { Id = Guid.NewGuid(), Email = "get@test.com" };
            _userRepoMock.Setup(r => r.GetByIdAsync(user.Id)).ReturnsAsync(user);

            // Act
            var result = await _userService.GetByIdAsync(user.Id);

            // Assert
            result.Should().NotBeNull();
            result!.Email.Should().Be(user.Email);
        }



        // ======== Test GetAll retourne liste des utilisateurs
        [Fact]
        public async Task GetAllAsync_ShouldReturnListOfUsers()
        {
            // Arrange
            var users = new List<User>
            {
                new User { Email = "u1@test.com" },
                new User { Email = "u2@test.com" }
            };
            _userRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(users);

            // Act
            var result = await _userService.GetAllAsync();

            // Assert
            result.Should().HaveCount(2);
            result[0].Email.Should().Be("u1@test.com");
            result[1].Email.Should().Be("u2@test.com");
        }
    }
}
