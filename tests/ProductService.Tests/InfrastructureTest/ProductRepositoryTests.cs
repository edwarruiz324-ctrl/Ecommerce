namespace ProductsService.InfrastructureTest
{
    using Microsoft.Extensions.DependencyInjection;
    using ProductService.Domain.Entities;
    using ProductService.Domain.Repositories;
    using ProductService.Infrastructure.Repositories;

    public class ProductRepositoryTests
    {

        ////// Repo Tests for basic CRUD operations

        [Fact]
        public async Task AddAsync_ShouldPersistProduct()
        {
            // Arrange
            var db = TestDbContextFactory.Create("AddAsyncDb");
            var repo = new ProductRepository(db.Context);
            var product = new Product("Laptop", "Gaming Laptop", 1200, 5);

            // Act
            await repo.AddAsync(product);
            await db.UnitOfWork.SaveChangesAsync();

            var result = await repo.GetByIdAsync(product.Id);
            
            // Assert
            Assert.NotNull(result);
            Assert.Equal("Laptop", result!.Name);
        }

        [Fact]
        public async Task UpdateAsync_ShouldModifyProduct()
        {
            var db = TestDbContextFactory.Create("UpdateAsyncDb");
            var repo = new ProductRepository(db.Context);
            var product = new Product("Phone", "Smartphone", 500, 10);

            await repo.AddAsync(product);
            await db.UnitOfWork.SaveChangesAsync();

            // Act
            product.Update("Phone X", "Updated Smartphone", 700, 8);
            await repo.UpdateAsync(product);
            await db.UnitOfWork.SaveChangesAsync();

            var result = await repo.GetByIdAsync(product.Id);
            await db.UnitOfWork.SaveChangesAsync();

            Assert.Equal("Phone X", result!.Name);
            Assert.Equal(700, result.Price);
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveProduct()
        {
            var db = TestDbContextFactory.Create("DeleteAsyncDb");
            var repo = new ProductRepository(db.Context);
            var product = new Product("Tablet", "Android Tablet", 300, 15);

            await repo.AddAsync(product);
            await db.UnitOfWork.SaveChangesAsync();

            // Act
            await repo.DeleteAsync(product);
            await db.UnitOfWork.SaveChangesAsync();

            var result = await repo.GetByIdAsync(product.Id);

            Assert.Null(result);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllProducts()
        {
            var db = TestDbContextFactory.Create("GetAllAsyncDb");
            var repo = new ProductRepository(db.Context);
            
            await repo.AddAsync(new Product("P1", "Desc1", 100, 2));
            await repo.AddAsync(new Product("P2", "Desc2", 200, 3));
            await db.UnitOfWork.SaveChangesAsync();

            // Act
            var products = await repo.GetAllAsync();

            // Assert
            Assert.Equal(2, products.Count());
        }
    }
}
