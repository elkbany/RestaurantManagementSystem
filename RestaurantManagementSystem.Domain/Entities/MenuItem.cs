using System.ComponentModel.DataAnnotations;

namespace RestaurantManagementSystem.Domain.Entities
{
    public class MenuItem
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
        public decimal Price { get; set; }

        public bool IsAvailable { get; set; } = true; // Default to available
        public string ImageUrl { get; set; } = string.Empty; 

        public int CategoryId { get; set; }

        public Category Category { get; set; }

        public int DailyOrderCount { get; set; } = 0; // Default to zero orders

        [Range(0, int.MaxValue, ErrorMessage = "Preparation time must be non-negative.")]
        public int PreparationTime { get; set; } = 0; // Default to zero minutes

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>(); // Initialize collection

        /// <summary>
        /// Concurrency token for optimistic concurrency control.
        /// </summary>
        public byte[] RowVersion { get; set; }
    }
}