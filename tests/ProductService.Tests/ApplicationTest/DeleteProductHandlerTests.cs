namespace ProductService.Tests.Application
{
    using Moq;
    using ProductService.Application.Commands;
    using ProductService.Domain.Entities;
    using ProductService.Domain.Repositories;

    public class DeleteProductHandlerTests
    {
        [Fact]
        public async Task Handle_ShouldDeleteProduct_WhenFound()
        {
            var product = new Product("Test", "Desc", 50, 3);

            var mockRepo = new Mock<IProductRepository>();
            var mockUow = new Mock<IUnitOfWork>();
            mockUow.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
            mockRepo.Setup(r => r.GetByIdAsync(product.Id, It.IsAny<CancellationToken>())).ReturnsAsync(product);
            mockRepo.Setup(r => r.DeleteAsync(product, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            var handler = new DeleteProductHandler(mockRepo.Object, mockUow.Object);
            var command = new DeleteProductCommand(product.Id);

            var result = await handler.Handle(command, default);

            Assert.True(result);
            mockRepo.Verify(r => r.DeleteAsync(product, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnFalse_WhenNotFound()
        {
            var mockRepo = new Mock<IProductRepository>();
            var mockUow = new Mock<IUnitOfWork>();

            mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync((Product?)null);

            var handler = new DeleteProductHandler(mockRepo.Object, mockUow.Object);
            var command = new DeleteProductCommand(99);

            var result = await handler.Handle(command, default);

            Assert.False(result);
        }
    }
}
