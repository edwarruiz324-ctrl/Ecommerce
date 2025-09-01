namespace OrderService.Application.Dtos
{
    using OrderService.Domain;

    public class UpdateOrderStatusRequest
    {
        public int OrderId { get; set; }
        public OrderStatus NewStatus { get; set; }
    }
}
