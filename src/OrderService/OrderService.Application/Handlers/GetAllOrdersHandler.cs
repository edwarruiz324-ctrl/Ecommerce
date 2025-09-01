namespace OrderService.Application.Handlers
{
    using MediatR;
    using OrderService.Application.Dtos;
    using OrderService.Application.Queries;
    using OrderService.Domain.Repositories;

    public class GetAllOrdersHandler : IRequestHandler<GetAllOrdersQuery, IEnumerable<OrderDto>>
    {
        private readonly IOrderRepository _orderRepo;

        public GetAllOrdersHandler(IOrderRepository orderRepo) => _orderRepo = orderRepo;

        public async Task<IEnumerable<OrderDto>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
        {
            var orders = await _orderRepo.GetAllAsync(cancellationToken);
            return orders.Select(order =>
            {
                var items = order.Items.Select(i => new OrderItemDto(i.Id, i.ProductId, i.Quantity, i.UnitPrice, i.Amount));
                return new OrderDto(order.Id, order.CustomerId, order.Status.ToString(), order.TotalAmount, order.OrderDate, items);
            });
        }
    }
}
