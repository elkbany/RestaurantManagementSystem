using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagementSystem.Domain.Entities
{
    public class Staff
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string Name { get; set; }

        [Required]
        public string Role { get; set; } // e.g., "Waiter", "Chef"

        public ICollection<Order> AssignedOrders { get; set; } = new List<Order>();
    }
}
