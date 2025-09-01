namespace ProductService.Tests.Application
{
    using Moq;
    using ProductService.Application.Commands;
    using ProductService.Domain.Entities;
    using ProductService.Domain.Repositories;

    public class CreateProductHandlerTests
    {
        [Fact]
        public async Task Handle_ShouldCreateProduct()
        {
            // Arrange
            var mockRepo = new Mock<IProductRepository>();
            var mockUow = new Mock<IUnitOfWork>();
            mockUow.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
            mockRepo.Setup(r => r.AddAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            var handler = new CreateProductHandler(mockRepo.Object, mockUow.Object);
            var command = new CreateProductCommand("Phone", "Smartphone", 500, 10);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            mockRepo.Verify(r => r.AddAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()), Times.Once);
            mockUow.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            Assert.Equal("Phone", result.Name);
            Assert.Equal("Smartphone", result.Description);
            Assert.Equal(500, result.Price);
            Assert.Equal(10, result.Stock);
        }
    }
}
