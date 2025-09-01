namespace ProductService.Tests.ControllerTests
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using ProductService.Application.Commands;
    using ProductService.Application.Dtos;
    using ProductService.Application.Queries;
    using ProductsService.Api.Controllers;
    using Xunit;

    public class ProductsControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ProductsController _controller;

        public ProductsControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new ProductsController(_mediatorMock.Object);
        }

        [Fact]
        public async Task Create_ShouldReturnCreatedAt_WithProductDto()
        {
            // Arrange
            var request = new CreateProductRequest
            {
                Name = "Test Product",
                Description = "Description",
                Price = 10m,
                Stock = 5
            };

            var expected = new ProductDto(1, request.Name, request.Description, request.Price, request.Stock, DateTime.Now, DateTime.Now);

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<CreateProductCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expected);

            // Act
            var result = await _controller.Create(request);

            // Assert
            var createdAt = Assert.IsType<CreatedAtActionResult>(result.Result);
            var dto = Assert.IsType<ProductDto>(createdAt.Value);
            Assert.Equal(expected.Id, dto.Id);
        }

        [Fact]
        public async Task GetById_ShouldReturnOk_WhenProductExists()
        {
            // Arrange
            var expected = new ProductDto(1, "Name", "Desc", 20m, 10, DateTime.Now, DateTime.Now);
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetProductByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expected);

            // Act
            var result = await _controller.GetById(1);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result.Result);
            var dto = Assert.IsType<ProductDto>(ok.Value);
            Assert.Equal(expected.Id, dto.Id);
        }

        [Fact]
        public async Task GetById_ShouldReturnNotFound_WhenProductDoesNotExist()
        {
            // Arrange
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetProductByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((ProductDto?)null);

            // Act
            var result = await _controller.GetById(99);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetAll_ShouldReturnOk_WithListOfProducts()
        {
            // Arrange
            var expected = new List<ProductDto>
            {
                new ProductDto(1, "P1", "D1", 10m, 5, DateTime.Now, DateTime.Now),
                new ProductDto(2, "P2", "D2", 20m, 15, DateTime.Now, DateTime.Now),
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetAllProductsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expected);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result.Result);
            var list = Assert.IsAssignableFrom<IEnumerable<ProductDto>>(ok.Value);
            Assert.NotEmpty(list);
        }

        [Fact]
        public async Task Update_ShouldReturnOk_WhenProductExists()
        {
            // Arrange
            var request = new UpdateProductRequest
            {
                Id = 1,
                Name = "Updated",
                Description = "Updated",
                Price = 50m,
                Stock = 20
            };

            var expected = new ProductDto(request.Id, request.Name, request.Description, request.Price, request.Stock, DateTime.Now, DateTime.Now);

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<UpdateProductCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expected);

            // Act
            var result = await _controller.Update(1, request);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result.Result);
            var dto = Assert.IsType<ProductDto>(ok.Value);
            Assert.Equal(request.Id, dto.Id);
        }

        [Fact]
        public async Task Update_ShouldReturnBadRequest_WhenIdsMismatch()
        {
            // Arrange
            var request = new UpdateProductRequest { Id = 2 };

            // Act
            var result = await _controller.Update(1, request);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task Delete_ShouldReturnNoContent_WhenProductDeleted()
        {
            // Arrange
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<DeleteProductCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Delete_ShouldReturnNotFound_WhenProductDoesNotExist()
        {
            // Arrange
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<DeleteProductCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.Delete(99);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
