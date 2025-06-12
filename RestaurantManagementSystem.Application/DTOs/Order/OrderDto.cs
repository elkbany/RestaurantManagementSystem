using RestaurantManagementSystem.Application.DTOs.OrderItem;
using RestaurantManagementSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagementSystem.Application.DTOs.Order
{
    public class OrderDto
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public OrderType OrderType { get; set; }
        public OrderStatus Status { get; set; }
        public string DeliveryAddress { get; set; }
        public decimal Total { get; set; }
        public DateTime OrderDate { get; set; }
        public List<OrderItemDto> OrderItems { get; set; } 
    }
}

