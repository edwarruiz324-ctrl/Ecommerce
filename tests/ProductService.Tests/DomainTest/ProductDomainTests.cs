namespace ProductService.Tests.DomainTest;

using Common.Exceptions;
using ProductService.Domain.Entities;
using Xunit;

public class ProductDomainTests
{
    public class ProductTests
    {
        [Fact]
        public void CreateProduct_ShouldInitializeFieldsCorrectly()
        {
            // Arrange
            var name = "Laptop";
            var description = "Gaming Laptop";
            var price = 1500m;
            var stock = 10;

            // Act
            var product = new Product(name, description, price, stock);

            // Assert
            Assert.Equal(name, product.Name);
            Assert.Equal(description, product.Description);
            Assert.Equal(price, product.Price);
            Assert.Equal(stock, product.Stock);
            Assert.True(product.CreatedAt <= DateTime.UtcNow);
            Assert.True(product.UpdatedAt <= DateTime.UtcNow);
        }

        [Fact]
        public void Update_ShouldModifyFieldsAndUpdateTimestamp()
        {
            var product = new Product("Old", "Old Desc", 100, 5);
            var oldUpdatedAt = product.UpdatedAt;

            // Act
            product.Update("New", "New Desc", 200, 8);

            // Assert
            Assert.Equal("New", product.Name);
            Assert.Equal("New Desc", product.Description);
            Assert.Equal(200, product.Price);
            Assert.Equal(8, product.Stock);
            Assert.True(product.UpdatedAt > oldUpdatedAt);
        }

        [Fact]
        public void ReduceStock_ShouldDecreaseStock()
        {
            var product = new Product("Test", "Test Desc", 50, 10);

            // Act
            product.ReduceStock(3);

            // Assert
            Assert.Equal(7, product.Stock);
        }

        [Fact]
        public void ReduceStock_ShouldThrow_WhenQuantityExceedsStock()
        {
            var product = new Product("Test", "Test Desc", 50, 2);

            // Act & Assert
            Assert.Throws<CustomException>(() => product.ReduceStock(5));
        }

        [Fact]
        public void IncreaseStock_ShouldAddToStock()
        {
            var product = new Product("Test", "Test Desc", 50, 5);

            // Act
            product.IncreaseStock(10);

            // Assert
            Assert.Equal(15, product.Stock);
        }
    }
}
