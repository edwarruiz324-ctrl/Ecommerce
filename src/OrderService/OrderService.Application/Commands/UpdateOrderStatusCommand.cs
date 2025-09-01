namespace OrderService.Application.Commands
{
    using MediatR;
    using OrderService.Domain;

    public record UpdateOrderStatusCommand(int OrderId, OrderStatus NewStatus) : IRequest<OrderService.Application.Dtos.OrderDto>;
}
