namespace ProductService.Tests.InfrastructureTest
{
    using OrderService.Infrastructure.HttpClients;
    using System.Net;
    using System.Text;
    using System.Text.Json;

    public class ProductClientTests
    {
        private class FakeHandler : HttpMessageHandler
        {
            public HttpRequestMessage? LastRequest { get; private set; }
            private readonly HttpResponseMessage _response;

            public FakeHandler(object responseObject, HttpStatusCode code = HttpStatusCode.OK)
            {
                var json = JsonSerializer.Serialize(responseObject);
                _response = new HttpResponseMessage(code)
                {
                    Content = new StringContent(json, Encoding.UTF8, "application/json")
                };
            }

            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken ct)
            {
                LastRequest = request;
                return Task.FromResult(_response);
            }
        }

        private record ProductDto(int Id, string Name);

        [Fact]
        public async Task GetByIdAsync_Should_CallExpectedEndpoint_And_Deserialize()
        {
            // Arrange
            var expectedId = 1;
            var handler = new FakeHandler(new ProductDto(expectedId, "Laptop"));
            var http = new HttpClient(handler) { BaseAddress = new Uri("https://api.products.test/") };

            var client = new ProductClient(http);

            // Act
            // TODO: ajusta al nombre real (GetByIdAsync/GetAsync/GetProductAsync)
            var result = await client.GetProductByIdAsync(expectedId, new CancellationToken());

            // Assert request
            Assert.NotNull(((FakeHandler)handler).LastRequest);
            var req = ((FakeHandler)handler).LastRequest!;
            Assert.Equal(HttpMethod.Get, req.Method);
            Assert.Equal($"/api/products/{expectedId}", req.RequestUri!.AbsolutePath); // ajusta ruta

            // Assert response mapping
            Assert.NotNull(result);
            Assert.Equal(expectedId, result!.Id);
        }

        [Fact]
        public async Task GetByIdAsync_Should_Handle_NotFound_As_Null()
        {
            // Arrange
            var handler = new HttpStatusHandler(HttpStatusCode.NotFound);
            var http = new HttpClient(handler) { BaseAddress = new Uri("https://api.products.test/") };
            var client = new ProductClient(http);

            // Act
            var result = await client.GetProductByIdAsync(123, new CancellationToken());

            // Assert
            Assert.Null(result);
        }

        private sealed class HttpStatusHandler : HttpMessageHandler
        {
            private readonly HttpStatusCode _status;
            public HttpStatusHandler(HttpStatusCode status) => _status = status;

            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
                => Task.FromResult(new HttpResponseMessage(_status));
        }
    }
}
