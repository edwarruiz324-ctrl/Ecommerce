namespace ProductService.Tests.Application
{
    using Moq;
    using ProductService.Application.Commands;
    using ProductService.Domain.Entities;
    using ProductService.Domain.Repositories;

    public class UpdateProductHandlerTests
    {
        [Fact]
        public async Task Handle_ShouldUpdateExistingProduct()
        {
            // Arrange
            var product = new Product("Old", "Old Desc", 100, 5);

            var mockRepo = new Mock<IProductRepository>();
            var mockUow = new Mock<IUnitOfWork>();
            mockRepo.Setup(r => r.GetByIdAsync(product.Id, It.IsAny<CancellationToken>())).ReturnsAsync(product);
            mockRepo.Setup(r => r.UpdateAsync(product, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            var handler = new UpdateProductHandler(mockRepo.Object, mockUow.Object);
            var command = new UpdateProductCommand(product.Id, "New", "New Desc", 200, 10);

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            Assert.Equal("New", result.Name);
            Assert.Equal("New Desc", result.Description);
            Assert.Equal(200, result.Price);
            Assert.Equal(10, result.Stock);

            mockRepo.Verify(r => r.UpdateAsync(product, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrow_WhenProductNotFound()
        {
            var mockRepo = new Mock<IProductRepository>();
            var mockUow = new Mock<IUnitOfWork>();
            mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync((Product?)null);

            var handler = new UpdateProductHandler(mockRepo.Object, mockUow.Object);
            var command = new UpdateProductCommand(999, "X", "Y", 10, 1);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => handler.Handle(command, default));
        }
    }
}
