namespace OrderService.Domain.Entities
{
    public sealed class OrderItem
    {
        public int Id { get; private set; }          
        public int ProductId { get; private set; }
        public int Quantity { get; private set; }
        public decimal UnitPrice { get;  set; }

        public decimal Amount => Quantity * UnitPrice;

        // EF-required ctor
        private OrderItem() { }

        public OrderItem(int productId, int quantity, decimal unitPrice)
        {
            if (productId <= 0) throw new ArgumentException("ProductId must be positive.", nameof(productId));
            if (quantity <= 0) throw new ArgumentException("Quantity must be greater than zero.", nameof(quantity));
            if (unitPrice < 0) throw new ArgumentException("UnitPrice must be non-negative.", nameof(unitPrice));

            ProductId = productId;
            Quantity = quantity;
            UnitPrice = unitPrice;
        }

        public void UpdateQuantity(int quantity)
        {
            if (quantity <= 0) throw new ArgumentException("Quantity must be greater than zero.", nameof(quantity));
            Quantity = quantity;
        }

        public void UpdateUnitPrice(decimal unitPrice)
        {
            if (unitPrice < 0) throw new ArgumentException("UnitPrice must be non-negative.", nameof(unitPrice));
            UnitPrice = unitPrice;
        }
    }
}
