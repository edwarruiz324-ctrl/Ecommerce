namespace OrderService.Tests.ApplicationTest
{
    using Moq;
    using OrderService.Application.Handlers;
    using OrderService.Application.Queries;
    using OrderService.Domain.Entities;
    using OrderService.Domain.Repositories;

    public class GetAllOrdersHandlerTests
    {
        [Fact]
        public async Task Handle_ShouldReturnOrders()
        {
            // Arrange
            var repoMock = new Mock<IOrderRepository>();
            var mockUow = new Mock<IUnitOfWork>();

            repoMock.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(new List<Order> { new Order("123") });

            var handler = new GetAllOrdersHandler(repoMock.Object);

            // Act
            var result = await handler.Handle(new GetAllOrdersQuery(), default);

            // Assert
            Assert.Single(result);
        }
    }
}
