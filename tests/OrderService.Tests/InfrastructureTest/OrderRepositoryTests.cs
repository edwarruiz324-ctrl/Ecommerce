namespace OrderService.Tests.InfrastructureTest
{
    using OrderService.Domain;
    using OrderService.Domain.Entities;
    using OrderService.Infrastructure.Repositories;

    public class OrderRepositoryTests
    {
        [Fact]
        public async Task AddAsync_ShouldPersistOrder()
        {
            // Arrange
            var wrapper = TestDbContextFactory.Create("AddOrderDb");
            var repo = new OrderRepository(wrapper.Context);

            var order = new Order("C1", new List<OrderItem>
            {
                new OrderItem(1,2,100) 
            });

            // Act
            await repo.AddAsync(order);
            await wrapper.UnitOfWork.SaveChangesAsync();

            var result = await repo.GetByIdAsync(order.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("C1", result!.CustomerId);
            Assert.True(result!.Items.Any());
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnOrders()
        {
            // Arrange
            var wrapper = TestDbContextFactory.Create("GetAllOrdersDb");
            var repo = new OrderRepository(wrapper.Context);

            var order1 = new Order("C1");

            var order2 = new Order("C2");

            await repo.AddAsync(order1);
            await repo.AddAsync(order2);
            await wrapper.UnitOfWork.SaveChangesAsync();

            // Act
            var result = await repo.GetAllAsync();

            // Assert
            Assert.Equal(2, result!.Count());
        }

        [Fact]
        public async Task UpdateAsync_ShouldModifyOrder()
        {
            // Arrange
            var wrapper = TestDbContextFactory.Create("UpdateOrderDb");
            var repo = new OrderRepository(wrapper.Context);

            var order = new Order("C3");

            await repo.AddAsync(order);
            await wrapper.UnitOfWork.SaveChangesAsync();

            // Act
            order.Status = OrderStatus.Processing;
            await repo.UpdateAsync(order);
            await wrapper.UnitOfWork.SaveChangesAsync();


            var result = await repo.GetByIdAsync(order.Id);
            await wrapper.UnitOfWork.SaveChangesAsync();

            // Assert
            Assert.Equal(OrderStatus.Processing, result!.Status);
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveOrder()
        {
            // Arrange
            var wrapper = TestDbContextFactory.Create("DeleteOrderDb");
            var repo = new OrderRepository(wrapper.Context);

            var order = new Order("C4");

            await repo.AddAsync(order);
            await wrapper.UnitOfWork.SaveChangesAsync();

            // Act
            await repo.DeleteAsync(order);
            await wrapper.UnitOfWork.SaveChangesAsync();

            var result = await repo.GetByIdAsync(order.Id);

            // Assert
            Assert.Null(result);
        }
    }
}
