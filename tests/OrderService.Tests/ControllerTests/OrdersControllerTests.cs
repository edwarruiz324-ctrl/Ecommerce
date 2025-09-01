namespace OrderService.Tests.ControllerTests
{
    using Common.Constants;
    using Common.Exceptions;
    using FluentAssertions;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using OrderService.Api.Controllers;
    using OrderService.Application.Commands;
    using OrderService.Application.Dtos;
    using OrderService.Application.Queries;
    using OrderService.Domain;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Xunit;

    public class OrdersControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly OrdersController _controller;

        public OrdersControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new OrdersController(_mediatorMock.Object);
        }

        [Fact]
        public async Task GetAll_ShouldReturnOkWithOrders()
        {
            // Arrange
            var orders = new List<OrderDto> { new OrderDto( 1,  "C1", OrderStatus.Pending.ToString(), 30, DateTime.Now, new List<OrderItemDto>()) };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetAllOrdersQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(orders);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = result as OkObjectResult;            
            okResult.Should().NotBeNull();
            okResult!.Value.Should().BeEquivalentTo(orders);
        }

        [Fact]
        public async Task GetById_ShouldReturnOk_WhenOrderExists()
        {
            // Arrange
            var order = new OrderDto(2, "C2", OrderStatus.Pending.ToString(), 30, DateTime.Now, new List<OrderItemDto>());

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetOrderByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(order);

            // Act
            var result = await _controller.GetById(42);

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.Value.Should().BeEquivalentTo(order);
        }

        [Fact]
        public async Task GetById_ShouldReturnNotFound_WhenOrderDoesNotExist()
        {
            // Arrange
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetOrderByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((OrderDto?)null);

            // Act
            var result = await _controller.GetById(99);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task Create_ShouldReturnCreatedAtAction_WithOrderId()
        {
            // Arrange
            var order = new OrderDto(3, "C3", OrderStatus.Pending.ToString(), 30, DateTime.Now, new List<OrderItemDto>());
            
            var request = new CreateOrderRequest { CustomerId = "C3",  Items = new List<CreateOrderItemRequest>
            {
                new CreateOrderItemRequest{ ProductId = 1, Quantity = 2 }
            }
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<CreateOrderCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(order);

            // Act
            var result = await _controller.Create(request);

            // Assert
            var createdAt = Assert.IsType<CreatedAtActionResult>(result);
            var dto = Assert.IsType<OrderDto>(createdAt.Value);
            Assert.Equal(order.Id, dto.Id);
        }

        [Fact]
        public async Task UpdateStatus_ShouldReturnNoContent_WhenSuccess()
        {
            // Arrange
            var order = new OrderDto(2, "C2", OrderStatus.Pending.ToString(), 30, DateTime.Now, new List<OrderItemDto>());
            var request = new UpdateOrderStatusRequest { OrderId = 10, NewStatus = OrderStatus.Shipped };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<UpdateOrderStatusCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(order);

            // Act
            var result = await _controller.UpdateStatus(10, request);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            var dto = Assert.IsType<OrderDto>(ok.Value);
            Assert.Equal(order.Id, dto.Id);
        }

        [Fact]
        public async Task UpdateStatus_ShouldReturnBadRequest_WhenIdMismatch()
        {
            // Arrange
            var request = new UpdateOrderStatusRequest { OrderId = 5, NewStatus = OrderStatus.Shipped };

            // Act
            var result = await _controller.UpdateStatus(10, request);

            // Assert
            var badRequest = result as BadRequestObjectResult;
            badRequest.Should().NotBeNull();
            //badRequest!.Value.Should().Be(ErrorMessages.IdNotMatch);
            Assert.Contains(ErrorMessages.IdNotMatch, badRequest!.Value!.ToString());
        }
    }
}
