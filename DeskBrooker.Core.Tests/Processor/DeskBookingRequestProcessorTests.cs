using DeskBooker.Core.Domain;
using System;
using Xunit;

namespace DeskBooker.Core.Processor
{
    public class DeskBookingRequestProcessorTests
    {
        private readonly DeskBookerRequestProcessor _processor;

        public DeskBookingRequestProcessorTests()
        {
            _processor = new DeskBookerRequestProcessor();
        }

        [Fact]
        public void ShouldReturnDeskBookingResultWithRequestValues()
        {
            // Arrange
            var request = new DeskBookingRequest
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "testing@test.com",
                Date = new DateTime(2022, 1, 1)
            };

            // Act
            DeskBookingResult result = _processor.BookDesk(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(request.FirstName, result.FirstName);
            Assert.Equal(request.LastName, result.LastName);
            Assert.Equal(request.Email, result.Email);
            Assert.Equal(request.Date, result.Date);
        }

        [Fact]
        public void ShouldThrowExceptionIfRequestIsNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => _processor.BookDesk(null));

            Assert.Equal("request", exception.ParamName);
        }
    }
}
