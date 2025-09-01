namespace OrderService.Infrastructure
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using OrderService.Application.Contracts;
    using OrderService.Domain.Repositories;
    using OrderService.Infrastructure.Common;
    using OrderService.Infrastructure.HttpClients;
    using OrderService.Infrastructure.Repositories;
    using Polly;
    using Polly.CircuitBreaker;
    using Polly.Retry;
    using System;

    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Configuración de DbContext
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Falta la configuracion dela cadena de conexión");

            services.AddDbContext<OrderDbContext>(options => 
                                                  options.UseSqlServer(
                                                    connectionString,
                                                    b => b.MigrationsAssembly(typeof(OrderDbContext).Assembly.FullName)
                                                  ));

            // Repositorios + UoW
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IProductClient, ProductClient>();


            // Configuración del cliente HTTP con Polly 8 + Resilience
            var productServiceBaseUrl = configuration["ProductService:BaseUrl"]
                ?? throw new InvalidOperationException("Falta la configuracion ProductService:BaseUrl");

            services.AddHttpClient<IProductClient, ProductClient>(client =>
            {
                client.BaseAddress = new Uri(productServiceBaseUrl);
                client.Timeout = TimeSpan.FromSeconds(20);
            })
            .AddResilienceHandler("ProductServicePipeline", builder =>
            {
                // Retry con backoff exponencial
                builder.AddRetry(new RetryStrategyOptions<HttpResponseMessage>
                {
                    MaxRetryAttempts = 1,
                    Delay = TimeSpan.FromSeconds(2),
                    BackoffType = DelayBackoffType.Exponential,
                    ShouldHandle = new PredicateBuilder<HttpResponseMessage>()
                        .Handle<HttpRequestException>()
                        .HandleResult(r => !r.IsSuccessStatusCode)
                });

                // Circuit Breaker
                builder.AddCircuitBreaker(new CircuitBreakerStrategyOptions<HttpResponseMessage>
                {
                    FailureRatio = 0.5,              // 50% de fallos en ventana
                    MinimumThroughput = 10,          // al menos 10 requests evaluados
                    SamplingDuration = TimeSpan.FromSeconds(30),
                    BreakDuration = TimeSpan.FromSeconds(15),
                    ShouldHandle = new PredicateBuilder<HttpResponseMessage>()
                        .Handle<HttpRequestException>()
                        .HandleResult(r => !r.IsSuccessStatusCode)
                });
            });


            return services;
        }
    }
}
