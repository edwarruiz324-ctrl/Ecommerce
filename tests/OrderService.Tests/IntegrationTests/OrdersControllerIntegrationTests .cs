namespace OrderService.Tests.IntegrationTests
{
    using FluentAssertions;
    using OrderService.Application.Dtos;
    using OrderService.Domain;
    using System.Net;
    using System.Net.Http.Json;

    public class OrdersControllerTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public OrdersControllerTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Post_CreateOrder_ShouldReturn201AndOrder()
        {
            // Arrange
            var request = new CreateOrderRequest
            {
                CustomerId = "CUST-123",
                Items = new List<CreateOrderItemRequest>
            {
                new CreateOrderItemRequest { ProductId = 1, Quantity = 2 }
            }
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/orders", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            var order = await response.Content.ReadFromJsonAsync<OrderDto>();
            order.Should().NotBeNull();
            order!.CustomerId.Should().Be("CUST-123");
            order.Items.Should().HaveCount(1);
        }

        [Fact]
        public async Task Get_GetAllOrders_ShouldReturnList()
        {
            // Arrange: Crear un pedido primero
            var request = new CreateOrderRequest
            {
                CustomerId = "CUST-555",
                Items = new List<CreateOrderItemRequest>
            {
                new CreateOrderItemRequest { ProductId = 1, Quantity = 1}
            }
            };

            var order = await _client.PostAsJsonAsync("/api/orders", request);

            // Act
            var response = await _client.GetAsync("/api/orders");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var orders = await response.Content.ReadFromJsonAsync<List<OrderDto>>();
            orders.Should().NotBeEmpty();
        }

        [Fact]
        public async Task Put_UpdateOrderStatus_ShouldReturnNoContent()
        {
            // Arrange: Crear un pedido
            var request = new CreateOrderRequest
            {
                CustomerId = "CUST-777",
                Items = new List<CreateOrderItemRequest>
            {
                new CreateOrderItemRequest { ProductId = 1, Quantity = 1 }
            }
            };
            var postResponse = await _client.PostAsJsonAsync("/api/orders", request);
            
            var order = await postResponse.Content.ReadFromJsonAsync<OrderDto>();

            // Act: Actualizar estado
            var updateResponse = await _client.PutAsJsonAsync(
                $"/api/orders/{order!.Id}/status",
                new UpdateOrderStatusRequest {  OrderId = 1, NewStatus = OrderStatus.Confirmed });

            // Assert
            updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var getResponse = await _client.GetAsync($"/api/orders/{order.Id}");
            var updatedOrder = await getResponse.Content.ReadFromJsonAsync<OrderDto>();
            Assert.Equal(updatedOrder.Status, OrderStatus.Confirmed.ToString());
        }

        [Fact]
        public async Task CreateOrder_WithNegativeQuantity_ShouldReturnBadRequest()
        {
            // Arrange: Crear un pedido
            var request = new CreateOrderRequest
            {
                CustomerId = "CUST123",
                Items = new List<CreateOrderItemRequest>
            {
                new CreateOrderItemRequest { ProductId = 1, Quantity = -5 }
            }
            };

            var response = await _client.PostAsJsonAsync("/api/orders", request);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var body = await response.Content.ReadAsStringAsync();
            Assert.Contains("La cantidad debe ser mayor que 0", body);
        }

        [Fact]
        public async Task CreateOrder_WithNegativePrice_ShouldReturnBadRequest()
        {
            var command = new
            {
                CustomerId = "cust123",
                Items = new[]
                {
                new { ProductId = 3, Quantity = 2 }
            },
                TotalAmount = -200m
            };

            var response = await _client.PostAsJsonAsync("/api/orders", command);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var body = await response.Content.ReadAsStringAsync();
            Assert.Contains("El precio del producto 3 no es valido", body);
        }

        [Fact]
        public async Task CreateOrder_WithNoItems_ShouldReturnBadRequest()
        {
            var command = new
            {
                CustomerId = "cust123",
                Items = new object[] { }, // vacío
                TotalAmount = 0m
            };

            var response = await _client.PostAsJsonAsync("/api/orders", command);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var body = await response.Content.ReadAsStringAsync();
            Assert.Contains("El pedido debe contener al menos un producto", body);
        }

        [Fact]
        public async Task CreateOrder_WithNonExistingProduct_ShouldReturnBadRequest()
        {
            var command = new
            {
                CustomerId = "cust123",
                Items = new[]
                {
                new { ProductId = 999, Quantity = 1, UnitPrice = 50m }
            },
                TotalAmount = 50m
            };

            var response = await _client.PostAsJsonAsync("/api/orders", command);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var body = await response.Content.ReadAsStringAsync();
            Assert.Contains("no existe", body);
        }

        [Fact]
        public async Task CreateOrder_WithInsufficientStock_ShouldReturnBadRequest()
        {
            var command = new
            {
                CustomerId = "cust123",
                Items = new[]
                {
                new { ProductId = 1, Quantity = 5, UnitPrice = 100m }
            },
                TotalAmount = 500m
            };

            var response = await _client.PostAsJsonAsync("/api/orders", command);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var body = await response.Content.ReadAsStringAsync();
            Assert.Contains("Stock insuficiente", body);
        }
    }
}
