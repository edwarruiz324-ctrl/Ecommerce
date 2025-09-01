namespace ProductService.Domain.Entities
{
    public class Product
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public decimal Price { get; private set; }
        public int Stock { get; private set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Constructor protegido (para EF y serialización)
        protected Product() { }

        // Constructor para crear un nuevo producto
        public Product(string name, string description, decimal price, int stock)
        {
            Name = name;
            Description = description;
            Price = price;
            Stock = stock;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        // Métodos de dominio
        public void Update(string name, string description, decimal price, int stock)
        {
            Name = name;
            Description = description;
            Price = price;
            Stock = stock;
            UpdatedAt = DateTime.UtcNow;
        }

        public void ReduceStock(int quantity)
        {
            if (quantity > Stock)
                throw new InvalidOperationException("No existe Stock suficiente.");

            Stock -= quantity;
            UpdatedAt = DateTime.UtcNow;
        }

        public void IncreaseStock(int quantity)
        {
            Stock += quantity;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
