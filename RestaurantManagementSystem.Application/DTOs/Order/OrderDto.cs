using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using RestaurantManagementSystem.Domain.Enums;
using RestaurantManagementSystem.Application.DTOs.OrderItem;

namespace RestaurantManagementSystem.Application.DTOs.Order
{
    public class OrderDto
    {
        public int Id { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Customer Id must be positive.")]
        public int CustomerId { get; set; }

        [Required]
        public OrderType OrderType { get; set; }

        [Required]
        public OrderStatus Status { get; set; }

        [StringLength(200)]
        public string DeliveryAddress { get; set; }
        public DateTime? EstimatedDeliveryTime { get; set; } 
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Total must be non-negative.")]
        public decimal Total { get; set; }

        public DateTime OrderDate { get; set; }
        public List<OrderItemDto> OrderItems { get; set; } = new List<OrderItemDto>();
    }

}