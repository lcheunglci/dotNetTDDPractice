namespace RoomBookingApp.Domain
{
    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<RoomBooking> RoomBookings { get; set; }
    }
}