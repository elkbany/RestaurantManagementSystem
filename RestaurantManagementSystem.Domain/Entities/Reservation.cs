namespace RestaurantManagementSystem.Domain.Entities
{
    public class Reservation
    {
        public int Id { get; set; }
        public int TableId { get; set; }
        public Table Table { get; set; }
        public DateTime ReservationDateTime { get; set; }
        public string CustomerName { get; set; }
        public int PartySize { get; set; }
    }
}