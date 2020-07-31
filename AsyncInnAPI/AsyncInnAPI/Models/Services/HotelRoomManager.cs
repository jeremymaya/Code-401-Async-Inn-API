using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AsyncInnAPI.Data;
using AsyncInnAPI.Models.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AsyncInnAPI.Models.Services
{
    public class HotelRoomManager : IHotelRoomManager
    {
        private AsyncInnDbContext _context;

        public HotelRoomManager(AsyncInnDbContext context)
        {
            _context = context;
        }

        public async Task<HotelRoom> CreateHotelRoom(HotelRoom hotelRoom)
        {
            _context.Entry(hotelRoom).State = EntityState.Added;

            await _context.SaveChangesAsync();

            return hotelRoom;
        }

        public async Task DeleteHotelRoom(int hotelId, int roomNumber)
        {
            var hotelRoom = await GetHotelRoom(hotelId, roomNumber);

            _context.Entry(hotelRoom).State = EntityState.Deleted;

            await _context.SaveChangesAsync();
        }

        public async Task<HotelRoom> GetHotelRoom(int hotelId, int roomNumber)
        {
            var hotelRoom = await _context.HotelRooms.Where(x => x.HotelId == hotelId && x.RoomNumber == roomNumber)
                                                     .Include(x => x.Room)
                                                     .FirstOrDefaultAsync();
            return hotelRoom;
        }

        public async Task<List<HotelRoom>> GetHotelRooms(int hotelId)
        {
            var hotelRooms = await _context.HotelRooms.Where(x => x.HotelId == hotelId)
                                                      .Include(x => x.Room)
                                                      .ThenInclude(x => x.Rooms)
                                                      .ToListAsync();
            return hotelRooms;
        }

        public async Task UpdateHotelRoom(HotelRoom hotelRoom)
        {
            _context.Entry(hotelRoom).State = EntityState.Modified;

            await _context.SaveChangesAsync();
        }
    }
}
