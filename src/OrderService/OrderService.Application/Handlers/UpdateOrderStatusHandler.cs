namespace OrderService.Application.Handlers
{
    using MediatR;
    using OrderService.Application.Commands;
    using OrderService.Domain.Repositories;

    public class UpdateOrderStatusHandler : IRequestHandler<UpdateOrderStatusCommand, OrderService.Application.Dtos.OrderDto>
    {
        private readonly IOrderRepository _orderRepo;
        private readonly IUnitOfWork _uow;

        public UpdateOrderStatusHandler(IOrderRepository orderRepo, IUnitOfWork uow)
        {
            _orderRepo = orderRepo;
            _uow = uow;
        }

        public async Task<OrderService.Application.Dtos.OrderDto> Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderRepo.GetByIdAsync(request.OrderId, cancellationToken);
            
            if (order is null) 
                throw new KeyNotFoundException($"Order {request.OrderId} no encontrada.");

            order.UpdateStatus(request.NewStatus);
            await _orderRepo.UpdateAsync(order, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            var items = order.Items.Select(i => new OrderService.Application.Dtos.OrderItemDto(i.Id, i.ProductId, i.Quantity, i.UnitPrice, i.Amount));
            return new OrderService.Application.Dtos.OrderDto(order.Id, order.CustomerId, order.Status.ToString(), order.TotalAmount, order.OrderDate, items);
        }
    }
}
