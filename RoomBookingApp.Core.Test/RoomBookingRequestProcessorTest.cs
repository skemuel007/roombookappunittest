using System;
using Moq;
using RoomBookingApp.Core.DataServices;
using RoomBookingApp.Core.Enums;
using RoomBookingApp.Core.Models;
using RoomBookingApp.Core.Processors;
using RoomBookingApp.Domain;
using Shouldly;

namespace RoomBookingApp.Core
{
    public class RoomBookingRequestProcessorTest
    {
        private RoomBookingRequestProcessor _processor;
        private RoomBookingRequest _bookingRequest;
        private Mock<IRoomBookingService> _roomBookingServiceMock;
        private List<Room> _availableRooms;

        public RoomBookingRequestProcessorTest()
        { 

            // arrange room booking request
            _bookingRequest = new RoomBookingRequest
            {
                FullName = "Test Name",
                Email = "test@test.com",
                Date = new DateTime(2022, 09, 16)
            };

            _availableRooms = new List<Room>()
            {
                new Room()
            };

            _roomBookingServiceMock = new Mock<IRoomBookingService>();

            _roomBookingServiceMock.Setup(x => x.GetAvailableRooms(_bookingRequest.Date))
                .Returns(_availableRooms);
            // arrange roombook request processor
            _processor = new RoomBookingRequestProcessor(_roomBookingServiceMock.Object);


        }

        [Fact]
        public void ShouldReturnRoomBookingResponseWithRequestValues()
        {
            

            // Act
            RoomBookingResult result = _processor.BookRoom(_bookingRequest);

            // Assert
            // Assert.NotNull(result);
            // Assert.Equal(bookingRequest.FullName, result.FullName);
            // Assert.Equal(bookingRequest.Email, result.Email);
            // Assert.Equal(bookingRequest.Date, result.Date);

            // Should Assertion
            result.ShouldNotBeNull(); // same as above
            result.FullName.ShouldBe(_bookingRequest.FullName);
            result.Email.ShouldBe(_bookingRequest.Email);
            result.Date.ShouldBe(_bookingRequest.Date);

        }

        [Fact]
        public void ShouldThrowExceptionForNullRequest()
        {
            // Arrange
            // var processor = new RoomBookingRequestProcessor();

            // Act
            // RoomBookingResult result = processor.BookRoom(null);

            // Assert
            var exception = Should.Throw<ArgumentNullException>(() => _processor.BookRoom(null));
            // Assert.Throws<ArgumentNullException>(() => processor.BookRoom(null));
            exception.ParamName.ShouldBe("bookingRequest");
        }

        [Fact]
        public void ShouldSaveRooomBookingRequest()
        {
            RoomBooking savedBook = null;

            // Act
            _roomBookingServiceMock.Setup(x => x.Save(It.IsAny<RoomBooking>()))
                .Callback<RoomBooking>(booking =>
                {
                    savedBook = booking;
                });
            _processor.BookRoom(_bookingRequest);

            // Assert
            _roomBookingServiceMock.Verify(q => q.Save(It.IsAny<RoomBooking>()), Times.Once);
            savedBook.ShouldNotBeNull();
            savedBook.Email.ShouldBe(_bookingRequest.Email);
            savedBook.Date.ShouldBe(_bookingRequest.Date);
            savedBook.FullName.ShouldBe(_bookingRequest.FullName);
            savedBook.RoomId.ShouldBe(_availableRooms.First().Id);
        }

        [Fact]
        public void ShouldNotSaveRoomBookingRequestIfNoneAvailable()
        {
            _availableRooms.Clear();
            _processor.BookRoom(_bookingRequest);
            _roomBookingServiceMock.Verify(q => q.Save(It.IsAny<RoomBooking>()), Times.Never);
        }

        [Theory]
        [InlineData(BookingResultFlag.Failure, false)]
        [InlineData(BookingResultFlag.Success, true)]
        public void ShouldReturnSuccessOrFailureFlagInResult(BookingResultFlag bookSuccessFlag, bool isAvailable)
        {
            if ( !isAvailable )
            {
                _availableRooms.Clear();
            }

            // act
            var result = _processor.BookRoom(_bookingRequest);

            // 
            bookSuccessFlag.ShouldBe(result.Flag);
        }

        [Theory]
        [InlineData(1, true)]
        [InlineData(null, false)]
        public void ShouldReturnRoomBookingIdInResult(int? roomBookingId, bool isAvailable)
        {
            if (!isAvailable)
            {
                _availableRooms.Clear();
            } else
            {

                // Act
                _ = _roomBookingServiceMock.Setup(x => x.Save(It.IsAny<RoomBooking>()))
                    .Callback<RoomBooking>(booking =>
                    {
                        booking.Id = roomBookingId.Value;
                    });
            }

            var result = _processor.BookRoom(_bookingRequest);
            result.RoomBookingId.ShouldBe(roomBookingId);



        }
    }
}

