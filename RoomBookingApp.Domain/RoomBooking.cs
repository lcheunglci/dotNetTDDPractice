using RoomBookingApp.Domain.BaseModel;

namespace RoomBookingApp.Domain
{
    public class RoomBooking : RoomBookingBase
    {
        public int Id { get; set; }


        // this will add the FK
        public Room Room { get; set; }
        public int RoomId { get; set; }

    }
}