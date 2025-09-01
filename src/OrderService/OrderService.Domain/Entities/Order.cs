namespace OrderService.Domain.Entities
{
    public sealed class Order
    {
        public int Id { get; set; }           
        public string CustomerId { get; private set; } = string.Empty;
        public OrderStatus Status { get; set; }
        public decimal TotalAmount { get; private set; }
        public DateTime OrderDate { get; set; }
        public List<OrderItem> Items { get; private set; } = new();

        // EF ctor
        private Order() { }

        public Order(string customerId)
        {
            if (string.IsNullOrWhiteSpace(customerId))
                throw new ArgumentException("CustomerId es requerido.", nameof(customerId));

            CustomerId = customerId;
            Status = OrderStatus.Pending;
            OrderDate = DateTime.UtcNow;
            TotalAmount = 0m;
            Items = new List<OrderItem>();
        }

        public Order(string customerId, List<OrderItem> items)
        {
            if (string.IsNullOrWhiteSpace(customerId))
                throw new ArgumentException("CustomerId es requerido.", nameof(customerId));

            CustomerId = customerId;
            Status = OrderStatus.Pending;
            OrderDate = DateTime.UtcNow;
            TotalAmount = 0m;
            Items = items;

            if (items.Any())
                RecalculateTotal();
        }

        public OrderItem AddItem(int productId, int quantity, decimal unitPrice)
        {
            var existing = Items.FirstOrDefault(i => i.ProductId == productId && i.UnitPrice == unitPrice);
            if (existing != null)
            {
                // aggregate adding to existing line (business choice)
                existing.UpdateQuantity(existing.Quantity + quantity);
            }
            else
            {
                var item = new OrderItem(productId, quantity, unitPrice);
                Items.Add(item);
            }

            RecalculateTotal();
            return Items.Last();
        }

        public void RemoveItem(int productId)
        {
            var item = Items.FirstOrDefault(i => i.ProductId == productId);
            if (item == null) 
                throw new InvalidOperationException("Item no encontrado.");
            
            Items.Remove(item);
            RecalculateTotal();
        }

        public void UpdateItem(int productId, int quantity, decimal unitPrice)
        {
            var item = Items.FirstOrDefault(i => i.ProductId == productId);
            if (item == null) 
                throw new InvalidOperationException("Item no encontrado.");

            item.UpdateQuantity(quantity);
            item.UpdateUnitPrice(unitPrice);
            RecalculateTotal();
        }

        private void RecalculateTotal()
        {
            decimal total = 0m;
            foreach (var it in Items)
            {
                total += it.Amount;
            }

            TotalAmount = decimal.Round(total, 2, MidpointRounding.AwayFromZero);
        }

        public void UpdateStatus(OrderStatus newStatus)
        {
            if (Status == newStatus) return;

            var allowed = GetAllowedTransitions(Status);
            if (!allowed.Contains(newStatus))
                throw new InvalidOperationException($"No se Puede cambiar el estado de {Status} a {newStatus}.");

            Status = newStatus;
        }

        private static HashSet<OrderStatus> GetAllowedTransitions(OrderStatus current)
        {
            return current switch
            {
                OrderStatus.Pending => new HashSet<OrderStatus> { OrderStatus.Confirmed, OrderStatus.Cancelled },
                OrderStatus.Confirmed => new HashSet<OrderStatus> { OrderStatus.Processing, OrderStatus.Cancelled },
                OrderStatus.Processing => new HashSet<OrderStatus> { OrderStatus.Shipped, OrderStatus.Cancelled },
                OrderStatus.Shipped => new HashSet<OrderStatus> { OrderStatus.Delivered },
                OrderStatus.Delivered => new HashSet<OrderStatus>(), 
                OrderStatus.Cancelled => new HashSet<OrderStatus>(), 
                _ => new HashSet<OrderStatus>()
            };
        }
    }
}

