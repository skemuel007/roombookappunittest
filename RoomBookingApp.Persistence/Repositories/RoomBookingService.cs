using System;
using RoomBookingApp.Core.DataServices;
using RoomBookingApp.Domain;

namespace RoomBookingApp.Persistence.Repositories
{
    public class RoomBookingService : IRoomBookingService
    {
        private readonly RoomBookingAppDbContext _context;


        public RoomBookingService(RoomBookingAppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Room> GetAvailableRooms(DateTime date)
        {
            // var unAvailableRooms = _context.RoomBookings.Where(x => x.Date == date)
            //             .Select(x => x.RoomId);

            // var availableRooms = _context.Rooms.Where(q => unAvailableRooms.ToList().Contains(q.Id) == false);

            return _context.Rooms.Where(q => q.RoomBookings.Any(x => x.Date == date) == false).ToList();

            /// return availableRooms;
        }

        public void Save(RoomBooking roomBooking)
        {
            _context.RoomBookings.Add(roomBooking);
            _context.SaveChanges();
        }
    }
}

