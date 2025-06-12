using Mapster;
using RestaurantManagementSystem.Application.DTOs.Category;
using RestaurantManagementSystem.Application.DTOs.MenuItem;
using RestaurantManagementSystem.Application.DTOs.Order;
using RestaurantManagementSystem.Application.DTOs.OrderItem;
using RestaurantManagementSystem.Domain.Entities; 

using System;

namespace RestaurantManagementSystem.Application.Mapping
{
    public class MappingConfig
    {
        public void Configure()
        {
            // Category Mapping
            TypeAdapterConfig<Category, CategoryDto>.NewConfig()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.Description, src => src.Description);

            // MenuItem Mapping
            TypeAdapterConfig<MenuItem, MenuItemDto>.NewConfig()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.Price, src => src.Price)
                .Map(dest => dest.IsAvailable, src => src.IsAvailable)
                .Map(dest => dest.PreparationTime, src => src.PreparationTime)
                .Map(dest => dest.CategoryId, src => src.CategoryId)
                .Map(dest => dest.CategoryName, src => src.Category != null ? src.Category.Name : null);
            // Order Mapping
            TypeAdapterConfig<Order, OrderDto>.NewConfig()
                .Map(dest => dest.Status, src => src.Status.ToString())
                .Map(dest => dest.OrderItems, src => src.OrderItems);

            // OrderItem Mapping
            TypeAdapterConfig<OrderItem, OrderItemDto>.NewConfig()
                .Map(dest => dest.OrderId, src => src.OrderId)
                .Map(dest => dest.MenuItemId, src => src.MenuItemId)
                .Map(dest => dest.Quantity, src => src.Quantity)
                .Map(dest => dest.Subtotal, src => src.Subtotal);
        }
    }
}