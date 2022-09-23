using RoomBookingApp.Core.Models;
using RoomBookingApp.Core.Processors;
using Shouldly;


namespace RoomBookingApp.Core
{
    public class RoomBookingRequestProcessorTest
    {
        private RoomBookingRequestProcessor _processor;

        public RoomBookingRequestProcessorTest()
        {
            // Arrange
            _processor = new RoomBookingRequestProcessor();

        }

        [Fact]
        public void Should_Return_Room_Booking_Request_With_Values()
        {
            // Arrange
            var request = new RoomBookingRequest
            {
                FullName = "Test Name",
                Email = "test@request.com",
                Date = new DateTime(2022, 9, 20)
            };


            // Act
            RoomBookingResult result = _processor.BookRoom(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(request.FullName, request.FullName);
            Assert.Equal(request.Email, request.Email);
            Assert.Equal(request.Date, request.Date);

            result.ShouldNotBeNull();
            result.FullName.ShouldBe(request.FullName);
            result.Email.ShouldBe(request.Email);
            result.Date.ShouldBe(request.Date);

        }

        [Fact]
        public void Should_Throw_Exception_For_Null_Request()
        {
            var exception = Should.Throw<ArgumentNullException>(() => _processor.BookRoom(null));

            exception.ParamName.ShouldBe("bookingRequest");
        }

    }
}