using AdminDashboard.Application.Dtos;
using AdminDashboard.Application.Interfaces;
using AdminDashboard.Application.Services;
using AdminDashboard.Domain.Entities;
using FluentAssertions;
using Moq;

namespace AdminDashboard.Tests.Unit
{
    public class ProductServiceTests
    {
        private readonly Mock<IProductRepository> _repoMock;
        private readonly ProductService _service;

        // Constructeur
        public ProductServiceTests()
        {
            _repoMock = new Mock<IProductRepository>();
            _service = new ProductService(_repoMock.Object);
        }



        // ======== Test GetAll retourne liste mappée de produits
        [Fact]
        public async Task GetAllAsync_ShouldReturnMappedDtos()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { Id = Guid.NewGuid(), Name = "P1", Category = "Cat", Price = 10, Stock = 5 }
            };

            _repoMock.Setup(r => r.GetAllAsync())
                     .ReturnsAsync(products);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            result.Should().HaveCount(1);
            result.First().Name.Should().Be("P1");

            _repoMock.Verify(r => r.GetAllAsync(), Times.Once);
        }



        // ======== Test GetById retourne null si produit inexistant
        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenNotFound()
        {
            // Arrange
            _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                     .ReturnsAsync((Product?)null);

            // Act
            var result = await _service.GetByIdAsync(Guid.NewGuid());

            // Assert
            result.Should().BeNull();
        }



        // ======== Test GetById retourne DTO mappé si produit existant
        [Fact]
        public async Task GetByIdAsync_ShouldReturnMappedDto_WhenFound()
        {
            // Arrange
            var id = Guid.NewGuid();

            _repoMock.Setup(r => r.GetByIdAsync(id))
                     .ReturnsAsync(new Product
                     {
                         Id = id,
                         Name = "Test",
                         Category = "Cat",
                         Price = 50,
                         Stock = 3
                     });

            // Act
            var result = await _service.GetByIdAsync(id);

            // Assert
            result.Should().NotBeNull();
            result!.Id.Should().Be(id);
            result.Name.Should().Be("Test");
        }



        // ======== Test Create force stock à 0 si négatif
        [Fact]
        public async Task CreateAsync_ShouldSetStockToZero_WhenNegative()
        {
            // Arrange
            var dto = new ProductCreateDto
            {
                Name = "Test",
                Category = "Cat",
                Price = 20,
                Stock = -5
            };

            _repoMock.Setup(r => r.CreateAsync(It.IsAny<Product>()))
                     .ReturnsAsync((Product p) => p);

            // Act
            var result = await _service.CreateAsync(dto);

            // Assert
            result.Stock.Should().Be(0);
        }



        // ======== Test Create appelle le repository
        [Fact]
        public async Task CreateAsync_ShouldCallRepository()
        {
            // Arrange
            var dto = new ProductCreateDto
            {
                Name = "Test",
                Category = "Cat",
                Price = 20,
                Stock = 10
            };

            _repoMock.Setup(r => r.CreateAsync(It.IsAny<Product>()))
                     .ReturnsAsync((Product p) => p);

            // Act
            await _service.CreateAsync(dto);

            // Assert
            _repoMock.Verify(r => r.CreateAsync(It.IsAny<Product>()), Times.Once);
        }



        // ======== Test Update appelle le repository avec bon Id
        [Fact]
        public async Task UpdateAsync_ShouldCallRepository()
        {
            // Arrange
            var id = Guid.NewGuid();

            var dto = new ProductUpdateDto
            {
                Name = "Updated",
                Category = "NewCat",
                Price = 99,
                Stock = 15
            };

            _repoMock.Setup(r => r.UpdateAsync(It.IsAny<Product>()))
                     .ReturnsAsync((Product p) => p);

            // Act
            await _service.UpdateAsync(id, dto);

            // Assert
            _repoMock.Verify(r => r.UpdateAsync(It.Is<Product>(p => p.Id == id)), Times.Once);
        }



        // ======== Test Update force stock à 0 si négatif
        [Fact]
        public async Task UpdateAsync_ShouldSetStockToZero_WhenNegative()
        {
            // Arrange
            var id = Guid.NewGuid();

            var dto = new ProductUpdateDto
            {
                Name = "Updated",
                Category = "Cat",
                Price = 10,
                Stock = -1
            };

            _repoMock.Setup(r => r.UpdateAsync(It.IsAny<Product>()))
                     .ReturnsAsync((Product p) => p);

            // Act
            var result = await _service.UpdateAsync(id, dto);

            // Assert
            result.Stock.Should().Be(0);
        }



        // ======== Test Delete appelle le repository
        [Fact]
        public async Task DeleteAsync_ShouldCallRepository()
        {
            // Arrange
            var id = Guid.NewGuid();

            // Act
            await _service.DeleteAsync(id);

            // Assert
            _repoMock.Verify(r => r.DeleteAsync(id), Times.Once);
        }



        // ======== Test Exists retourne true si produit existe
        [Fact]
        public async Task ExistsAsync_ShouldReturnTrue_WhenExists()
        {
            // Arrange
            _repoMock.Setup(r => r.ExistsAsync(It.IsAny<Guid>()))
                     .ReturnsAsync(true);

            // Act
            var result = await _service.ExistsAsync(Guid.NewGuid());

            // Assert
            result.Should().BeTrue();
        }
    }
}
