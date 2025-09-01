namespace OrderService.Domain.Exceptions
{
    public class OrderOperationException : DomainException
    {
        public OrderOperationException(string message) : base(message) { }
    }
}
