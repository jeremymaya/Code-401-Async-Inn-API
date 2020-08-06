using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AsyncInnAPI.Data;
using AsyncInnAPI.Models.Dtos;
using AsyncInnAPI.Models.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AsyncInnAPI.Models.Services
{
    public class RoomManager : IRoomManager
    {
        private readonly AsyncInnDbContext _context;
        private readonly IAmenityManager _amenities;

        public RoomManager(AsyncInnDbContext context, IAmenityManager amenities)
        {
            _context = context;
            _amenities = amenities;
        }

        public async Task<RoomDto> AddAmenityToRoom(int roomId, int amenityId)
        {
            RoomAmenity roomAmenity = new RoomAmenity()
            {
                RoomId = roomId,
                AmenityId = amenityId
            };

            _context.Entry(roomAmenity).State = EntityState.Added;

            await _context.SaveChangesAsync();

            var dto = await GetRoom(roomId);

            return dto;
        }

        public async Task<RoomDto> CreateRoom(RoomDto dto)
        {
            Room room = new Room()
            {
                Name = dto.Name,
                Layout = (Layout)Enum.Parse(typeof(Layout), dto.Layout),
            };

            _context.Entry(room).State = EntityState.Added;

            await _context.SaveChangesAsync();

            dto.Id = room.Id;

            return dto;
        }

        public async Task DeleteRoom(int id)
        {
            var room = await _context.Rooms.FindAsync(id);

            _context.Entry(room).State = EntityState.Deleted;

            await _context.SaveChangesAsync();
        }

        public async Task<RoomDto> GetRoom(int id)
        {
            var room = await _context.Rooms.Where(x => x.Id == id)
                                           .Include(x => x.Amenities)
                                           .ThenInclude(x => x.Amenity)
                                           .Include(x => x.Rooms)
                                           .ThenInclude(x => x.Room)
                                           .FirstOrDefaultAsync();

            RoomDto dto = new RoomDto()
            {
                Id = room.Id,
                Name = room.Name,
                Layout = room.Layout.ToString(),
                Amenities = await _amenities.GetAmenities()
            };

            return dto;
        }

        public async Task<List<RoomDto>> GetRooms()
        {
            var rooms = await _context.Rooms.Include(x => x.Amenities)
                                            .ThenInclude(x => x.Amenity)
                                            .ToListAsync();

            List<RoomDto> dtos = new List<RoomDto>();

            foreach (var room in rooms)
                dtos.Add(await GetRoom(room.Id));

            return dtos;
        }

        public async Task RemoveAmentityFromRoom(int roomId, int amenityId)
        {
            RoomAmenity roomAmenity = await _context.RoomAmenities.Where(x => x.AmenityId == amenityId).FirstOrDefaultAsync();

            _context.Entry(roomAmenity).State = EntityState.Deleted;

            await _context.SaveChangesAsync();
        }

        public async Task UpdateRoom(RoomDto dto)
        {
            var room = await _context.Rooms.FindAsync(dto.Id);

            room.Name = dto.Name;
            room.Layout = (Layout)Enum.Parse(typeof(Layout), dto.Layout);

            _context.Entry(room).State = EntityState.Modified;

            await _context.SaveChangesAsync();
        }
    }
}
