namespace ProductService.Tests.Application
{
    using Moq;
    using ProductService.Application.Queries;
    using ProductService.Domain.Entities;
    using ProductService.Domain.Repositories;

    public class GetAllProductsHandlerTests
    {
        [Fact]
        public async Task Handle_ShouldReturnListOfProducts()
        {
            var products = new List<Product>
        {
            new Product("P1", "D1", 10, 1),
            new Product("P2", "D2", 20, 2)
        };

            var mockRepo = new Mock<IProductRepository>();
            var mockUow = new Mock<IUnitOfWork>();

            mockRepo.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(products);

            var handler = new GetAllProductsHandler(mockRepo.Object);
            var query = new GetAllProductsQuery();

            var result = await handler.Handle(query, default);

            Assert.Equal(2, result.Count());
            Assert.Contains(result, p => p.Name == "P1");
            Assert.Contains(result, p => p.Name == "P2");
        }
    }
}
