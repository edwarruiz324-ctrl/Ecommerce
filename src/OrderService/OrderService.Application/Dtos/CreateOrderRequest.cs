namespace OrderService.Application.Dtos
{
    public class CreateOrderRequest
    {
        public string CustomerId { get; set; } = default!;
        public List<CreateOrderItemRequest> Items { get; set; } = new();
    }    
}
