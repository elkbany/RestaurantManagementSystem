using Microsoft.EntityFrameworkCore;
using RestaurantManagementSystem.Domain.Entities;
using RestaurantManagementSystem.Domain.Enums;

namespace RestaurantManagementSystem.Infrastructure.Data.Seeder
{
    public class DataSeeder
    {
        public static void SeedData(ModelBuilder modelBuilder)
        {
            // Seed Categories
            var category1 = new Category { Id = 1, Name = "Main Dishes", Description = "Main course items" };
            var category2 = new Category { Id = 2, Name = "Desserts", Description = "Sweet treats" };
            modelBuilder.Entity<Category>().HasData(category1, category2);

            // Seed Menu Items
            var menuItem1 = new MenuItem
            {
                Id = 1,
                Name = "Pizza",
                Price = 10.00m,
                IsAvailable = true,
                CategoryId = 1,
                PreparationTime = 20,
                DailyOrderCount = 0
            };
            var menuItem2 = new MenuItem
            {
                Id = 2,
                Name = "Cake",
                Price = 5.00m,
                IsAvailable = true,
                CategoryId = 2,
                PreparationTime = 15,
                DailyOrderCount = 0
            };
            modelBuilder.Entity<MenuItem>().HasData(menuItem1, menuItem2);

            // Seed Order
            var order = new Order
            {
                Id = 1,
                CustomerId = 1,
                OrderType = OrderType.Delivery,
                Status = OrderStatus.Pending,
                DeliveryAddress = "123 Main St",
                Total = 25.00m,
                OrderDate = new DateTime(2025, 6, 14, 12, 0, 0) // Static date
            };
            modelBuilder.Entity<Order>().HasData(order);

            // Seed Order Items
            var orderItem1 = new OrderItem
            {
                Id = 1,
                OrderId = 1,
                MenuItemId = 1,
                Quantity = 2,
                Subtotal = 20.00m
            };
            var orderItem2 = new OrderItem
            {
                Id = 2,
                OrderId = 1,
                MenuItemId = 2,
                Quantity = 1,
                Subtotal = 5.00m
            };
            modelBuilder.Entity<OrderItem>().HasData(orderItem1, orderItem2);

            // Seed Tables
            var table1 = new Table { Id = 3, TableNumber = "T01", Capacity = 4, IsReserved = false };
            var table2 = new Table { Id = 4, TableNumber = "T02", Capacity = 6, IsReserved = false };
            modelBuilder.Entity<Table>().HasData(table1, table2);

            // Seed Reservations with static date
            var reservation1 = new Reservation
            {
                Id = 1,
                TableId = 3,
                ReservationDateTime = new DateTime(2025, 6, 21, 18, 0, 0), // Static date and time
                CustomerName = "Ali",
                PartySize = 3
            };
            modelBuilder.Entity<Reservation>().HasData(reservation1);

            // Seed Staff
            var staff1 = new Staff { Id = 1, Name = "Mohamed", Role = "Waiter" };
            var staff2 = new Staff { Id = 2, Name = "Ahmed", Role = "Chef" };
            modelBuilder.Entity<Staff>().HasData(staff1, staff2);
        }
    }
}