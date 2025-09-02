namespace OrderService.Tests.ApplicationTest
{
    using Moq;
    using OrderService.Application.Handlers;
    using OrderService.Application.Queries;
    using OrderService.Domain.Entities;
    using OrderService.Domain.Repositories;

    public class GetOrderByIdHandlerTests
    {
        [Fact]
        public async Task Handle_ShouldReturnOrder_WhenExists()
        {
            // Arrange
            var repoMock = new Mock<IOrderRepository>();
            repoMock.Setup(r => r.GetByIdAsync(0, It.IsAny<CancellationToken>())).ReturnsAsync(new Order("1234"));

            var handler = new GetOrderByIdHandler(repoMock.Object);

            // Act
            var result = await handler.Handle(new GetOrderByIdQuery(0), default);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(0, result!.Id);
        }

        [Fact]
        public async Task Handle_ShouldReturnNull_WhenNotFound()
        {
            var mockRepo = new Mock<IOrderRepository>();
            mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync((Order?)null);

            var handler = new GetOrderByIdHandler(mockRepo.Object);
            var query = new GetOrderByIdQuery(1);

            var result = await handler.Handle(query, default);

            Assert.Null(result);
        }
    }
}
