using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AsyncInnAPI.Data;
using AsyncInnAPI.Models.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AsyncInnAPI.Models.Services
{
    public class RoomManager : IRoomManager
    {
        private AsyncInnDbContext _context;

        public RoomManager(AsyncInnDbContext context)
        {
            _context = context;
        }

        public async Task<Room> AddAmenityToRoom(int roomId, int amenityId)
        {
            RoomAmenity roomAmenity = new RoomAmenity()
            {
                RoomId = roomId,
                AmenityId = amenityId
            };

            _context.Entry(roomAmenity).State = EntityState.Added;

            await _context.SaveChangesAsync();

            var room = await GetRoom(roomId);

            return room;
        }

        public async Task<Room> CreateRoom(Room room)
        {
            _context.Entry(room).State = EntityState.Added;

            await _context.SaveChangesAsync();

            return room;
        }

        public async Task DeleteRoom(int id)
        {
            var room = await GetRoom(id);

            _context.Entry(room).State = EntityState.Deleted;

            await _context.SaveChangesAsync();
        }

        public async Task<Room> GetRoom(int id)
        {
            var room = await _context.Rooms.Where(x => x.Id == id)
                                           .Include(x => x.Amenities)
                                           .ThenInclude(x => x.Amenity)
                                           .Include(x => x.Rooms)
                                           .ThenInclude(x => x.Room)
                                           .FirstOrDefaultAsync();
            return room;
        }

        public async Task<List<Room>> GetRooms()
        {
            var rooms = await _context.Rooms.Include(x => x.Amenities)
                                            .ThenInclude(x => x.Amenity)
                                            .ToListAsync();
            return rooms;
        }

        public async Task RemoveAmentityFromRoom(int roomId, int amenityId)
        {
            RoomAmenity roomAmenity = await _context.RoomAmenities.Where(x => x.AmenityId == amenityId).FirstOrDefaultAsync();

            _context.Entry(roomAmenity).State = EntityState.Deleted;

            await _context.SaveChangesAsync();
        }

        public async Task UpdateRoom(Room room)
        {
            _context.Entry(room).State = EntityState.Modified;

            await _context.SaveChangesAsync();
        }
    }
}
