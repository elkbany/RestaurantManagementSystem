using System.ComponentModel.DataAnnotations;

namespace RestaurantManagementSystem.Domain.Entities
{
    public class MenuItem
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } 
        public decimal Price { get; set; } 
        public bool IsAvailable { get; set; } 
        public int CategoryId { get; set; }     
        public Category Category { get; set; }
        public int DailyOrderCount { get; set; }
        public int PreparationTime { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
        /// <summary>
        /// Concurrency token for optimistic concurrency control.
        /// </summary>
        public byte[] RowVersion { get; set; }

    }  
}
