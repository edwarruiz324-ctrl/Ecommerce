namespace OrderService.Application.Dtos
{
    public record OrderDto(int Id, string CustomerId, string Status, decimal TotalAmount, DateTime OrderDate, IEnumerable<OrderItemDto> Items);
}
