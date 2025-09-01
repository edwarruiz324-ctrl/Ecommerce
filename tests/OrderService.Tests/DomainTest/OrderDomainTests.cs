namespace OrderService.Tests.DomainTest
{
    using Common.Exceptions;
    using OrderService.Domain;
    using OrderService.Domain.Entities;

    public class OrderDomainTests
    {
        [Fact]
        public void CreateOrder_ShouldInitializeCorrectly()
        {
            var order = new Order("customer-1");
            Assert.Equal("customer-1", order.CustomerId);
            Assert.Equal(OrderStatus.Pending, order.Status);
            Assert.NotEqual(default, order.OrderDate);
            Assert.Empty(order.Items);
            Assert.Equal(0m, order.TotalAmount);
        }

        [Fact]
        public void AddItem_ShouldIncreaseTotal()
        {
            var order = new Order("c1");
            order.AddItem(101, 2, 10.5m); // 21.0
            order.AddItem(102, 1, 5m);    // 5.0

            Assert.Equal(2, order.Items.Count);
            Assert.Equal(26.00m, order.TotalAmount);
        }

        [Fact]
        public void AddItem_WithInvalidQuantity_ShouldThrow()
        {
            var order = new Order("c1");

            Assert.Throws<ArgumentException>(() => order.AddItem(100, 0, 5m));
        }

        [Fact]
        public void UpdateStatus_AllowedTransition_ShouldWork()
        {
            var order = new Order("c1");
            order.UpdateStatus(OrderStatus.Confirmed);

            Assert.Equal(OrderStatus.Confirmed, order.Status);

            order.UpdateStatus(OrderStatus.Processing);
            Assert.Equal(OrderStatus.Processing, order.Status);
        }

        [Fact]
        public void UpdateStatus_DisallowedTransition_ShouldThrow()
        {
            var order = new Order("c1");

            order.UpdateStatus(OrderStatus.Confirmed);

            Assert.Throws<CustomException>(() => order.UpdateStatus(OrderStatus.Delivered));
        }
    }
}
