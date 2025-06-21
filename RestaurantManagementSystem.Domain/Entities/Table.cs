using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagementSystem.Domain.Entities
{
    public class Table
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Table number cannot exceed 50 characters.")]
        public string TableNumber { get; set; } = string.Empty;

        public int Capacity { get; set; }

        public bool IsReserved { get; set; } = false;

        public DateTime? ReservationTime { get; set; }

        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}
