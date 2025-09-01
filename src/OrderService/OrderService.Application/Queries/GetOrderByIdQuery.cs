namespace OrderService.Application.Queries
{
    using MediatR;
    using OrderService.Application.Dtos;
    public record GetOrderByIdQuery(int Id) : IRequest<OrderDto?>;
}
