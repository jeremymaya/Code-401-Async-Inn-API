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
    public class HotelRoomManager : IHotelRoomManager
    {
        private readonly AsyncInnDbContext _context;
        private readonly IRoomManager _rooms;

        public HotelRoomManager(AsyncInnDbContext context, IRoomManager rooms)
        {
            _context = context;
            _rooms = rooms;
        }

        public async Task<HotelRoomDto> CreateHotelRoom(HotelRoomDto dto)
        {
            HotelRoom hotelRoom = new HotelRoom()
            {
                HotelId = dto.HotelId,
                RoomNumber = dto.RoomNumber,
                RoomId = dto.RoomId,
                Rate = dto.Rate,
                PetFriendly = dto.PetFriendly,
            };

            _context.Entry(hotelRoom).State = EntityState.Added;

            await _context.SaveChangesAsync();

            var hotelDto = await GetHotelRoom(dto.HotelId, dto.RoomNumber);

            dto.Room = hotelDto.Room;

            return dto;
        }

        public async Task DeleteHotelRoom(int hotelId, int roomNumber)
        {
            var hotelRoom = await _context.HotelRooms.FindAsync(hotelId, roomNumber);

            _context.Entry(hotelRoom).State = EntityState.Deleted;

            await _context.SaveChangesAsync();
        }

        public async Task<HotelRoomDto> GetHotelRoom(int hotelId, int roomNumber)
        {
            var hotelRoom = await _context.HotelRooms.Where(x => x.HotelId == hotelId && x.RoomNumber == roomNumber)
                                                     .Include(x => x.Room)
                                                     .ThenInclude(x => x.Amenities)
                                                     .ThenInclude(x => x.Amenity)
                                                     .FirstOrDefaultAsync();

            HotelRoomDto dto = new HotelRoomDto()
            {
                HotelId = hotelRoom.HotelId,
                RoomNumber = hotelRoom.RoomNumber,
                Rate = hotelRoom.Rate,
                PetFriendly = hotelRoom.PetFriendly,
                RoomId = hotelRoom.RoomId,
                Room = await _rooms.GetRoom(hotelRoom.RoomId)
            };

            return dto;
        }

        public async Task<List<HotelRoomDto>> GetHotelRooms(int hotelId)
        {
            var hotelRooms = await _context.HotelRooms.Where(x => x.HotelId == hotelId)
                                                      .Include(x => x.Room)
                                                      .ThenInclude(x => x.Rooms)
                                                      .ToListAsync();

            List<HotelRoomDto> dtos = new List<HotelRoomDto>();

            foreach (var hotelRoom in hotelRooms)
                dtos.Add(await GetHotelRoom(hotelRoom.HotelId, hotelRoom.RoomNumber));

            return dtos;
        }

        public async Task UpdateHotelRoom(HotelRoomDto dto)
        {
            var hotelRoom = await _context.HotelRooms.Where(x => x.HotelId == dto.HotelId && x.RoomNumber == dto.RoomNumber)
                                         .Include(x => x.Room)
                                         .ThenInclude(x => x.Amenities)
                                         .ThenInclude(x => x.Amenity)
                                         .FirstOrDefaultAsync();

            hotelRoom.RoomId = dto.RoomId;
            hotelRoom.Rate = dto.Rate;
            hotelRoom.PetFriendly = dto.PetFriendly;

            _context.Entry(hotelRoom).State = EntityState.Modified;

            await _context.SaveChangesAsync();
        }
    }
}
