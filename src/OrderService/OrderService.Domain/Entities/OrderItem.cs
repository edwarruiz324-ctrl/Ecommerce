using Common.Constants;

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
            if (productId <= 0) throw new ArgumentException(ErrorMessages.ProductIdMustBePositive, nameof(productId));
            
            if (quantity <= 0) throw new ArgumentException(ErrorMessages.QuantityMustBeGreaterThanZero, nameof(quantity));
            
            if (unitPrice < 0) throw new ArgumentException(ErrorMessages.UnitPriceMustBeNonNegative, nameof(unitPrice));


            ProductId = productId;
            Quantity = quantity;
            UnitPrice = unitPrice;
        }

        public void UpdateQuantity(int quantity)
        {
            if (quantity <= 0) throw new ArgumentException(ErrorMessages.QuantityMustBeGreaterThanZero, nameof(quantity));
            Quantity = quantity;
        }

        public void UpdateUnitPrice(decimal unitPrice)
        {
            if (unitPrice < 0) throw new ArgumentException(ErrorMessages.UnitPriceMustBeNonNegative, nameof(unitPrice));
            UnitPrice = unitPrice;
        }
    }
}
