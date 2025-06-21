using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagementSystem.Application.DTOs.Reservation
{
    public class ReservationDto
    {
        public int Id { get; set; }
        public int TableId { get; set; }
        public DateTime ReservationDateTime { get; set; }
        public string CustomerName { get; set; }
        public int PartySize { get; set; }
    }
}
