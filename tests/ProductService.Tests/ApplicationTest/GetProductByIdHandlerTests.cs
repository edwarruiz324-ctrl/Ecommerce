namespace ProductService.Tests.Application
{
    using Moq;
    using ProductService.Application.Queries;
    using ProductService.Domain.Entities;
    using ProductService.Domain.Repositories;

    public class GetProductByIdHandlerTests
    {
        [Fact]
        public async Task Handle_ShouldReturnProduct_WhenExists()
        {
            var product = new Product("Laptop", "Gaming", 1000, 5);

            var mockRepo = new Mock<IProductRepository>();
            mockRepo.Setup(r => r.GetByIdAsync(product.Id, It.IsAny<CancellationToken>())).ReturnsAsync(product);

            var handler = new GetProductByIdHandler(mockRepo.Object);
            var query = new GetProductByIdQuery(product.Id);

            var result = await handler.Handle(query, default);

            Assert.NotNull(result);
            Assert.Equal("Laptop", result!.Name);
        }

        [Fact]
        public async Task Handle_ShouldReturnNull_WhenNotFound()
        {
            var mockRepo = new Mock<IProductRepository>();
            mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync((Product?)null);

            var handler = new GetProductByIdHandler(mockRepo.Object);
            var query = new GetProductByIdQuery(1);

            var result = await handler.Handle(query, default);

            Assert.Null(result);
        }
    }
}
