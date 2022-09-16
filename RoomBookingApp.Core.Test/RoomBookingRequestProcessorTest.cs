using System;
using RoomBookingApp.Core.Models;
using RoomBookingApp.Core.Processors;
using Shouldly;

namespace RoomBookingApp.Core
{
    public class RoomBookingRequestProcessorTest
    {
        [Fact]
        public void ShouldReturnRoomBookingResponseWithRequestValues()
        {
            var bookingRequest = new RoomBookingRequest
            {
                FullName = "Test Name",
                Email = "test@test.com",
                Date = new DateTime(2022, 09, 16)
            };

            var processor = new RoomBookingRequestProcessor();

            // Act
            RoomBookingResult result = processor.BookRoom(bookingRequest);

            // Assert
            // Assert.NotNull(result);
            // Assert.Equal(bookingRequest.FullName, result.FullName);
            // Assert.Equal(bookingRequest.Email, result.Email);
            // Assert.Equal(bookingRequest.Date, result.Date);

            // Should Assertion
            result.ShouldNotBeNull(); // same as above
            result.FullName.ShouldBe(bookingRequest.FullName);
            result.Email.ShouldBe(bookingRequest.Email);
            result.Date.ShouldBe(bookingRequest.Date);

        }
    }
}

