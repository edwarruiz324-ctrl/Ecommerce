namespace OrderService.Tests.ApplicationTest
{
    using Moq;
    using OrderService.Application.Commands;
    using OrderService.Application.Contracts;
    using OrderService.Application.Dtos;
    using OrderService.Application.Handlers;
    using OrderService.Domain.Entities;
    using OrderService.Domain.Repositories;

    public class CreateOrderHandlerTests
    {
        [Fact]
        public async Task Handle_ShouldCreateOrder_WhenProductsExistAndStockSufficient()
        {
            // Arrange
            var productClientMock = new Mock<IProductClient>();
            productClientMock.Setup(p => p.GetProductByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ProductInfoDto(1, "P1", Stock: 10, Price: 100m));
            productClientMock.Setup(p => p.GetProductByIdAsync(2, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ProductInfoDto(2, "P2", Stock: 5, Price: 50m));

            var repoMock = new Mock<IOrderRepository>();
            repoMock.Setup(r => r.AddAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            var uowMock = new Mock<IUnitOfWork>();
            uowMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var handler = new CreateOrderHandler(repoMock.Object, uowMock.Object, productClientMock.Object);

            var cmd = new CreateOrderCommand("customer-1",
                new List<CreateOrderItemDto>
                {
                new CreateOrderItemDto(1, 2, 100),
                new CreateOrderItemDto(2, 1, 50)
                });

            // Act
            var result = await handler.Handle(cmd, CancellationToken.None);

            // Assert
            repoMock.Verify(r => r.AddAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()), Times.Once);
            uowMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            Assert.Equal("customer-1", result.CustomerId);
            Assert.Equal(250m, result.TotalAmount);
        }

        [Fact]
        public async Task Handle_ShouldThrow_KeyNotFound_WhenProductNotFound()
        {
            var productClientMock = new Mock<IProductClient>();
            productClientMock.Setup(p => p.GetProductByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync((ProductInfoDto?)null);

            var repoMock = new Mock<IOrderRepository>();
            var uowMock = new Mock<IUnitOfWork>();

            var handler = new CreateOrderHandler(repoMock.Object, uowMock.Object, productClientMock.Object);

            var cmd = new CreateOrderCommand("c1", new List<CreateOrderItemDto> { new CreateOrderItemDto(1, 1, 10) });

            await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(cmd, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldThrow_WhenStockInsufficient()
        {
            var productClientMock = new Mock<IProductClient>();
            productClientMock.Setup(p => p.GetProductByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ProductInfoDto(1, "P1", Stock: 0, Price: 10m));

            var repoMock = new Mock<IOrderRepository>();
            var uowMock = new Mock<IUnitOfWork>();

            var handler = new CreateOrderHandler(repoMock.Object, uowMock.Object, productClientMock.Object);
            var cmd = new CreateOrderCommand("c1", new List<CreateOrderItemDto> { new CreateOrderItemDto(1, 1, 10) });

            await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(cmd, CancellationToken.None));
        }
    }
}
