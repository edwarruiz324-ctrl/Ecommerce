namespace OrderService.Application.Handlers
{
    using MediatR;
    using OrderService.Application.Commands;
    using OrderService.Application.Contracts;
    using OrderService.Application.Dtos;
    using OrderService.Domain.Entities;
    using OrderService.Domain.Repositories;
    public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, OrderDto>
    {
        private readonly IOrderRepository _orderRepo;
        private readonly IUnitOfWork _uow;
        private readonly IProductClient _productClient;

        public CreateOrderHandler(IOrderRepository orderRepo, IUnitOfWork uow, IProductClient productClient)
        {
            _orderRepo = orderRepo;
            _uow = uow;
            _productClient = productClient;
        }

        public async Task<OrderDto> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var items = new List<OrderItem>();
            decimal totalAmount = 0;

            foreach (var itemDto in request.Items)
            {
                var prod = await _productClient.GetProductByIdAsync(itemDto.ProductId, cancellationToken);

                // 1.Validar que el producto existe
                if (prod is null)
                    throw new InvalidOperationException($"El producto {itemDto.ProductId} no existe.");

                // 2. Validar stock
                if (itemDto.Quantity > prod.Stock)
                    throw new InvalidOperationException($"Stock insuficiente para producto {itemDto.ProductId}.");

                // 3. Validar precio
                if (prod.Price <= 0)
                    throw new InvalidOperationException($"El precio del producto {itemDto.ProductId} no es valido.");

                // 4. Crear OrderItem
                var orderItem = new OrderItem(itemDto.ProductId, itemDto.Quantity, prod.Price);
                items.Add(orderItem);
            }

            // 2) Create domain entity and add items
            var order = new OrderService.Domain.Entities.Order(request.CustomerId, items);
            
            // 3) Persist
            await _orderRepo.AddAsync(order, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            // 4) Map to DTO
            var dto = MapToDto(order);
            
            return dto;
        }

        private static OrderDto MapToDto(OrderService.Domain.Entities.Order order)
        {
            var items = order.Items.Select(i =>
                new OrderItemDto(i.Id, i.ProductId, i.Quantity, i.UnitPrice, i.Amount)).ToList();

            return new OrderDto(order.Id, order.CustomerId, order.Status.ToString(), order.TotalAmount, order.OrderDate, items);
        }
    }
}
