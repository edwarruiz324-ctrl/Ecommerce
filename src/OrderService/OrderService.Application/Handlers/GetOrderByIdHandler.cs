namespace OrderService.Application.Handlers
{
    using MediatR;
    using OrderService.Application.Dtos;
    using OrderService.Application.Queries;
    using OrderService.Domain.Repositories;

    public class GetOrderByIdHandler : IRequestHandler<GetOrderByIdQuery, OrderDto?>
    {
        private readonly IOrderRepository _orderRepo;

        public GetOrderByIdHandler(IOrderRepository orderRepo) => _orderRepo = orderRepo;

        public async Task<OrderDto?> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            var order = await _orderRepo.GetByIdAsync(request.Id, cancellationToken);
            if (order is null) return null;

            var items = order.Items.Select(i => new OrderItemDto(i.Id, i.ProductId, i.Quantity, i.UnitPrice, i.Amount));
            return new OrderDto(order.Id, order.CustomerId, order.Status.ToString(), order.TotalAmount, order.OrderDate, items);
        }
    }
}
