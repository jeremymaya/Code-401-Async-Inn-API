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
        private AsyncInnDbContext _context;

        public HotelRoomManager(AsyncInnDbContext context)
        {
            _context = context;
        }

        public async Task<HotelRoomDto> CreateHotelRoom(HotelRoomDto dto)
        {
            Room room = new Room();

            if (dto.Room != null)
            {
                room.Id = dto.Room.Id;
                room.Name = dto.Room.Name;
                room.Layout = (Layout)Enum.Parse(typeof(Layout), dto.Room.Layout);
                room.Amenities = new List<RoomAmenity>();

                if (dto.Room.Amenities != null)
                    foreach (var roomAmenity in dto.Room.Amenities)
                        room.Amenities.Add(new RoomAmenity() { RoomId = dto.RoomId, AmenityId = roomAmenity.Id });
            }

            HotelRoom hotelRoom = new HotelRoom()
            {
                HotelId = dto.HotelId,
                RoomNumber = dto.RoomNumber,
                RoomId = dto.RoomId,
                Rate = dto.Rate,
                PetFriendly = dto.PetFriendly,
                Room = room
            };

            _context.Entry(hotelRoom).State = EntityState.Added;

            await _context.SaveChangesAsync();

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
                                                     .FirstOrDefaultAsync();

            RoomDto roomDto = new RoomDto();

            if (hotelRoom.Room != null)
            {
                roomDto.Id = hotelRoom.Room.Id;
                roomDto.Name = hotelRoom.Room.Name;
                roomDto.Layout = hotelRoom.Room.Layout.ToString();
                roomDto.Amenities = new List<AmenityDto>();

                if (hotelRoom.Room.Amenities != null)
                    foreach (var amenity in hotelRoom.Room.Amenities)
                        roomDto.Amenities.Add(new AmenityDto() { Id = amenity.AmenityId, Name = amenity.Amenity.Name });
            }

            HotelRoomDto hotelDto = new HotelRoomDto()
            {
                HotelId = hotelRoom.HotelId,
                RoomNumber = hotelRoom.RoomNumber,
                Rate = hotelRoom.Rate,
                PetFriendly = hotelRoom.PetFriendly,
                RoomId = hotelRoom.RoomId,
                Room = roomDto
            };

            return hotelDto;
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
            Room room = new Room()
            {
                Id = dto.Room.Id,
                Name = dto.Room.Name,
                Layout = (Layout)Enum.Parse(typeof(Layout), dto.Room.Layout),
                Amenities = new List<RoomAmenity>()
            };

            foreach (var roomAmenity in dto.Room.Amenities)
                room.Amenities.Add(new RoomAmenity() { RoomId = dto.RoomId, AmenityId = roomAmenity.Id });

            HotelRoom hotelRoom = new HotelRoom()
            {
                HotelId = dto.HotelId,
                RoomNumber = dto.RoomNumber,
                RoomId = dto.RoomId,
                Rate = dto.Rate,
                PetFriendly = dto.PetFriendly,
                Room = room
            };

            _context.Entry(hotelRoom).State = EntityState.Modified;

            await _context.SaveChangesAsync();
        }
    }
}
