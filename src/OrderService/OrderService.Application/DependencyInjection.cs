namespace OrderService.Application
{
    using FluentValidation;
    using MediatR;
    using Microsoft.Extensions.DependencyInjection;
    using OrderService.Application.Validators;
    using System.Reflection;

    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();

            // Registrar MediatR con todos los handlers de Application
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));

            // Registrar todos los validators de Application
            services.AddValidatorsFromAssembly(assembly);

            // Pipeline Behavior de validación
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            return services;
        }
    }
}
