namespace OrderService.Tests.ApplicationTest
{
    using Moq;
    using OrderService.Application.Commands;
    using OrderService.Application.Handlers;
    using OrderService.Domain;
    using OrderService.Domain.Entities;
    using OrderService.Domain.Repositories;

    public class UpdateOrderStatusHandlerTests
    {
        [Fact]
        public async Task Handle_ShouldUpdateStatus()
        {
            // Arrange
            var repoMock = new Mock<IOrderRepository>();
            var uowMock = new Mock<IUnitOfWork>();

            var order = new Order("123");
            repoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(order);

            var handler = new UpdateOrderStatusHandler(repoMock.Object, uowMock.Object);

            // Act
            await handler.Handle(new UpdateOrderStatusCommand(0, OrderStatus.Confirmed), default);

            // Assert
            Assert.Equal(OrderStatus.Confirmed, order.Status);
            uowMock.Verify(u => u.SaveChangesAsync(default), Times.Once);
        }
    }
}
