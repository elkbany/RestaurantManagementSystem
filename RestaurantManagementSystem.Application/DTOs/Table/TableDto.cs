using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagementSystem.Application.DTOs.Table
{
    public class TableDto
    {
        public int Id { get; set; }
        public string TableNumber { get; set; }
        public int Capacity { get; set; }
        public bool IsReserved { get; set; }
        public DateTime? ReservationTime { get; set; }
    }
}
