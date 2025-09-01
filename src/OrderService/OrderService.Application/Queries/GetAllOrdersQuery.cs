namespace OrderService.Application.Queries
{
    using MediatR;
    using OrderService.Application.Dtos;
    public record GetAllOrdersQuery() : IRequest<IEnumerable<OrderDto>>;
}
