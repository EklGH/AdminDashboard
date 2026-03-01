using AdminDashboard.Application.Dtos;
using AdminDashboard.Application.Interfaces;
using AdminDashboard.Application.Services;
using AdminDashboard.Domain.Entities;
using FluentAssertions;
using Moq;

namespace AdminDashboard.Tests.Unit
{
    public class ReservationServiceTests
    {
        private readonly Mock<IReservationRepository> _repoMock;
        private readonly ReservationService _service;

        // Constructeur
        public ReservationServiceTests()
        {
            _repoMock = new Mock<IReservationRepository>();
            _service = new ReservationService(_repoMock.Object);
        }



        // ======== Test GetAll retourne liste mappée de réservations
        [Fact]
        public async Task GetAllAsync_ShouldReturnMappedDtos()
        {
            // Arrange
            var reservations = new List<Reservation>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Customer = "John Doe",
                    Date = new DateTime(2024, 1, 10),
                    Status = "Confirmed",
                    ProductId = Guid.NewGuid(),
                    UserId = Guid.NewGuid()
                }
            };

            _repoMock
                .Setup(r => r.GetAllAsync(null, null))
                .ReturnsAsync(reservations);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            result.Should().HaveCount(1);
            result.First().Customer.Should().Be("John Doe");
            result.First().Date.Should().Be("2024-01-10");
        }



        // ======== Test GetPaginated retourne résultat paginé correct
        [Fact]
        public async Task GetPaginatedAsync_ShouldReturnPaginatedResult()
        {
            // Arrange
            var reservations = new List<Reservation>
            {
                new() { Id = Guid.NewGuid(), Customer = "Alice", Date = DateTime.Today }
            };

            _repoMock
                .Setup(r => r.GetAllAsync(1, 10))
                .ReturnsAsync(reservations);

            _repoMock
                .Setup(r => r.CountAsync())
                .ReturnsAsync(1);

            // Act
            var result = await _service.GetPaginatedAsync(1, 10);

            // Assert
            result.Page.Should().Be(1);
            result.PageSize.Should().Be(10);
            result.TotalItems.Should().Be(1);
            result.Items.Should().HaveCount(1);
        }



        // ======== Test GetById retourne DTO si réservation existante
        [Fact]
        public async Task GetByIdAsync_ShouldReturnDto_WhenReservationExists()
        {
            // Arrange
            var id = Guid.NewGuid();
            var reservation = new Reservation
            {
                Id = id,
                Customer = "Client X",
                Date = DateTime.Today,
                Status = "Pending"
            };

            _repoMock
                .Setup(r => r.GetByIdAsync(id))
                .ReturnsAsync(reservation);

            // Act
            var result = await _service.GetByIdAsync(id);

            // Assert
            result.Should().NotBeNull();
            result!.Customer.Should().Be("Client X");
        }



        // ======== Test GetById retourne null si réservation inexistante 
        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenNotFound()
        {
            // Arrange
            _repoMock
                .Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Reservation?)null);

            // Act
            var result = await _service.GetByIdAsync(Guid.NewGuid());

            // Assert
            result.Should().BeNull();
        }



        // ======== Test Create crée réservation et retourne DTO mappé 
        [Fact]
        public async Task CreateAsync_ShouldCreateReservationAndReturnDto()
        {
            // Arrange
            var dto = new ReservationCreateDto
            {
                Customer = "New Client",
                Date = DateTime.Today,
                Status = "Confirmed",
                ProductId = Guid.NewGuid(),
                UserId = Guid.NewGuid()
            };

            _repoMock
                .Setup(r => r.CreateAsync(It.IsAny<Reservation>()))
                .ReturnsAsync((Reservation r) => r);

            // Act
            var result = await _service.CreateAsync(dto);

            // Assert
            result.Customer.Should().Be("New Client");
            result.Status.Should().Be("Confirmed");
        }



        // ======== Test Update retourne DTO update si réservation existante
        [Fact]
        public async Task UpdateAsync_ShouldReturnUpdatedDto_WhenExists()
        {
            // Arrange
            var id = Guid.NewGuid();
            var dto = new ReservationUpdateDto
            {
                Customer = "Updated Client",
                Date = DateTime.Today,
                Status = "Cancelled",
                ProductId = Guid.NewGuid(),
                UserId = Guid.NewGuid()
            };

            _repoMock
                .Setup(r => r.UpdateAsync(It.IsAny<Reservation>()))
                .ReturnsAsync((Reservation r) => r);

            // Act
            var result = await _service.UpdateAsync(id, dto);

            // Assert
            result.Should().NotBeNull();
            result!.Customer.Should().Be("Updated Client");
            result.Status.Should().Be("Cancelled");
        }



        // ======== Test Update retourne null si réservation inexistante
        [Fact]
        public async Task UpdateAsync_ShouldReturnNull_WhenNotFound()
        {
            // Arrange
            _repoMock
                .Setup(r => r.UpdateAsync(It.IsAny<Reservation>()))
                .ReturnsAsync((Reservation?)null);

            // Act
            var result = await _service.UpdateAsync(Guid.NewGuid(), new ReservationUpdateDto());

            // Assert
            result.Should().BeNull();
        }



        // ======== Test Delete retourne true si suppression réussie
        [Fact]
        public async Task DeleteAsync_ShouldReturnTrue_WhenDeleted()
        {
            // Arrange
            _repoMock
                .Setup(r => r.DeleteAsync(It.IsAny<Guid>()))
                .ReturnsAsync(true);

            // Act
            var result = await _service.DeleteAsync(Guid.NewGuid());

            // Assert
            result.Should().BeTrue();
        }



        // ======== Test Exists retourne true si réservation existe
        [Fact]
        public async Task ExistsAsync_ShouldReturnTrue_WhenReservationExists()
        {
            _repoMock
                .Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new Reservation());

            var exists = await _service.ExistsAsync(Guid.NewGuid());

            exists.Should().BeTrue();
        }



        // ======== Test Exists retourne false si réservation inexistante
        [Fact]
        public async Task ExistsAsync_ShouldReturnFalse_WhenReservationDoesNotExist()
        {
            _repoMock
                .Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Reservation?)null);

            var exists = await _service.ExistsAsync(Guid.NewGuid());

            exists.Should().BeFalse();
        }
    }
}
