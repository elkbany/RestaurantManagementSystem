using Mapster;
using RestaurantManagementSystem.Application.DTOs.Category;
using RestaurantManagementSystem.Application.DTOs.MenuItem;
using RestaurantManagementSystem.Application.DTOs.Order;
using RestaurantManagementSystem.Application.DTOs.OrderItem;
using RestaurantManagementSystem.Application.DTOs.Reservation;
using RestaurantManagementSystem.Application.DTOs.Staff;
using RestaurantManagementSystem.Application.DTOs.Table;
using RestaurantManagementSystem.Domain.Entities;

namespace RestaurantManagementSystem.Application.Mapping
{
    public class MappingConfig
    {
        private static int GetMenuItemCount(Category category)
        {
            return category?.MenuItems?.Count ?? 0;
        }

        public void Configure()
        {
            // Category Mapping
            TypeAdapterConfig<Category, CategoryDto>.NewConfig()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.MenuItemCount, src => GetMenuItemCount(src))
                .Map(dest => dest.IsActive, src => true); 

            // MenuItem Mapping
            TypeAdapterConfig<MenuItem, MenuItemDto>.NewConfig()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.Price, src => src.Price)
                .Map(dest => dest.IsAvailable, src => src.IsAvailable)
                .Map(dest => dest.PreparationTime, src => src.PreparationTime)
                .Map(dest => dest.CategoryId, src => src.CategoryId)
                .Map(dest => dest.CategoryName, src => src.Category != null ? src.Category.Name : null)
                .Map(dest => dest.ImageUrl, src => src.ImageUrl)
                .Map(dest => dest.DailyOrderCount, src => src.DailyOrderCount);

            // Order Mapping
            TypeAdapterConfig<Order, OrderDto>.NewConfig()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.CustomerId, src => src.CustomerId)
                .Map(dest => dest.OrderType, src => src.OrderType)
                .Map(dest => dest.Status, src => src.Status)
                .Map(dest => dest.DeliveryAddress, src => src.DeliveryAddress)
                .Map(dest => dest.Total, src => src.Total)
                .Map(dest => dest.OrderDate, src => src.OrderDate)
                .Map(dest => dest.OrderItems, src => src.OrderItems);

            // OrderItem Mapping
            TypeAdapterConfig<OrderItem, OrderItemDto>.NewConfig()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.OrderId, src => src.OrderId)
                .Map(dest => dest.MenuItemId, src => src.MenuItemId)
                .Map(dest => dest.Quantity, src => src.Quantity)
                .Map(dest => dest.Subtotal, src => src.Subtotal);

            TypeAdapterConfig<Table, TableDto>.NewConfig()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.TableNumber, src => src.TableNumber)
                .Map(dest => dest.Capacity, src => src.Capacity)
                .Map(dest => dest.IsReserved, src => src.IsReserved)
                .Map(dest => dest.ReservationTime, src => src.ReservationTime);

            TypeAdapterConfig<Reservation, ReservationDto>.NewConfig()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.TableId, src => src.TableId)
                .Map(dest => dest.ReservationDateTime, src => src.ReservationDateTime)
                .Map(dest => dest.CustomerName, src => src.CustomerName)
                .Map(dest => dest.PartySize, src => src.PartySize);

            TypeAdapterConfig<Staff, StaffDto>.NewConfig()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.Role, src => src.Role);

            // Reverse Mappings
            TypeAdapterConfig<CategoryDto, Category>.NewConfig();
            TypeAdapterConfig<MenuItemDto, MenuItem>.NewConfig();
            TypeAdapterConfig<OrderDto, Order>.NewConfig();
            TypeAdapterConfig<OrderItemDto, OrderItem>.NewConfig();
            TypeAdapterConfig<TableDto, Table>.NewConfig();
            TypeAdapterConfig<ReservationDto, Reservation>.NewConfig();
            TypeAdapterConfig<StaffDto, Staff>.NewConfig();
        }
    }
}