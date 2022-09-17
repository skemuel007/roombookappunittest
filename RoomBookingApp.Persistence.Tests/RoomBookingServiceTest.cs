using System;
using Microsoft.EntityFrameworkCore;
using RoomBookingApp.Domain;
using RoomBookingApp.Persistence.Repositories;

namespace RoomBookingApp.Persistence.Tests
{
    public class RoomBookingServiceTest
    {
        public RoomBookingServiceTest()
        {
        }

        [Fact]
        public void ShouldReturnAvailableRoom()
        {
            var date = new DateTime(2022, 09, 17);

            // in memory db for testing
            var dbOptions = new DbContextOptionsBuilder<RoomBookingAppDbContext>().UseInMemoryDatabase("AvailableRoomTest").Options;

            // create a scope
            var context = new RoomBookingAppDbContext(dbOptions);

            // seed with temp data for the lifetime of this test
            context.Add(new Room
            {
                Id = 1,
                Name = "Room 1"
            });
            context.Add(new Room
            {
                Id = 2,
                Name = "Room 2"
            });
            context.Add(new Room
            {
                Id = 3,
                Name = "Room 3"
            });

            context.Add(new RoomBooking { RoomId = 1, Date = date });
            context.Add(new RoomBooking { RoomId = 2, Date = date.AddDays(-1) });

            context.SaveChanges();

            var roomBookingService = new RoomBookingService(context);

            // act
            var availableRooms = roomBookingService.GetAvailableRooms(date);

            // Assert
            Assert.Equal(2, availableRooms.Count());
            Assert.Contains(availableRooms, x => x.Id == 2);
            Assert.Contains(availableRooms, x => x.Id == 3);
            Assert.DoesNotContain(availableRooms, x => x.Id == 1);

        }

        [Fact]
        public void ShouldSaveRoomBooking()
        {
            // in memory db for testing
            var dbOptions = new DbContextOptionsBuilder<RoomBookingAppDbContext>().UseInMemoryDatabase("ShouldSaveTest").Options;

            // create a scope
            var context = new RoomBookingAppDbContext(dbOptions);

            var roomBookingService = new RoomBookingService(context);

            var roomBooking = new RoomBooking { RoomId = 1, Date = new DateTime(2022, 09, 17) };

            roomBookingService.Save(roomBooking);

            var bookings = context.RoomBookings.ToList();
            var booking = Assert.Single(bookings);

            Assert.Equal(booking.Date, roomBooking.Date);
            Assert.Equal(booking.RoomId, roomBooking.RoomId);
        }
    }
}

