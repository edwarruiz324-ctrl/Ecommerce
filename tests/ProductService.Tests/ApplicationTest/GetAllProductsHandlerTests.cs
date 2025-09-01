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
            var filter1 = new Common.Models.PaginationFilter()
            {
                PageNumber = 1,
                PageSize = 13
            };

            var filter2 = new Common.Models.PaginationFilter()
            {
                PageNumber = 2,
                PageSize = 3
            };


            var productsPage1 = new List<Product>
            {
                new Product("P1", "D1", 10, 1),
                new Product("P2", "D2", 20, 2),
                new Product("P3", "D3", 30, 1)
            };

            var productsPage2 = new List<Product>
            {
                new Product("P4", "D4", 40, 2),
                new Product("P5", "D5", 50, 1)
            };

            var mockRepo = new Mock<IProductRepository>();
            var mockUow = new Mock<IUnitOfWork>();

            mockRepo.Setup(r => r.GetPaginateAsync(filter1, It.IsAny<CancellationToken>())).ReturnsAsync(productsPage1);
            mockRepo.Setup(r => r.GetPaginateAsync(filter2, It.IsAny<CancellationToken>())).ReturnsAsync(productsPage2);

            var handler = new GetAllProductsHandler(mockRepo.Object);
            var query1 = new GetAllProductsQuery(filter1);
            var query2 = new GetAllProductsQuery(filter2);

            var result1 = await handler.Handle(query1, default);
            var result2 = await handler.Handle(query2, default);

            Assert.Equal(3, result1!.Items.Count());
            Assert.Contains(result1!.Items, p => p.Name == "P1");
            Assert.Contains(result1!.Items, p => p.Name == "P2");

            Assert.Equal(2, result2!.Items.Count());
            Assert.Contains(result2!.Items, p => p.Name == "P4");
            Assert.Contains(result2!.Items, p => p.Name == "P5");
        }
    }
}
