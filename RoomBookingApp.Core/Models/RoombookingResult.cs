using RoomBookingApp.Core.Domain.Enums;

namespace RoomBookingApp.Core.Models
{
    public class RoomBookingResult : RoomBookingBase
    {
        public BookingSuccessFlag Flag { get; set; }
    }
}