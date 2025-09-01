namespace OrderService.Application.Commands
{
    using MediatR;
    using OrderService.Application.Dtos;

    public record CreateOrderCommand(string CustomerId, List<CreateOrderItemDto> Items) : IRequest<OrderService.Application.Dtos.OrderDto>;
}
