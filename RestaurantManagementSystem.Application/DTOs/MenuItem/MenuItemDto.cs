using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagementSystem.Application.DTOs.MenuItem
{
    public class MenuItemDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be positive.")]
        public decimal Price { get; set; }
        public string ImageUrl { get; set; } = string.Empty;

        public int DailyOrderCount { get; set; } = 0; 
        public bool IsAvailable { get; set; }
        public int PreparationTime { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public decimal? DiscountedPrice { get; set; }
    }
}
