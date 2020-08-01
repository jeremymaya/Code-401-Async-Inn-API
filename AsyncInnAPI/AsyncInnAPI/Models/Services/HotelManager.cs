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
    public class HotelManager : IHotelManager
    {
        private AsyncInnDbContext _context;

        public HotelManager(AsyncInnDbContext context)
        {
            _context = context;
        }

        public async Task<HotelDto> CreateHotel(HotelDto dto)
        {
            Hotel hotel = new Hotel()
            {
                Name = dto.Name,
                StreetAddress = dto.StreetAddress,
                City = dto.City,
                State = dto.State,
                Phone = dto.Phone
            };

            _context.Entry(hotel).State = EntityState.Added;

            await _context.SaveChangesAsync();

            dto.Id = hotel.Id;
            dto.Rooms = new List<HotelRoomDto>();

            return dto;
        }

        public async Task DeleteHotel(int id)
        {
            var hotel = await _context.Hotels.FindAsync(id);

            _context.Entry(hotel).State = EntityState.Deleted;

            await _context.SaveChangesAsync();
        }

        public async Task<HotelDto> GetHotel(int id)
        {
            var hotel = await _context.Hotels.Where(x => x.Id == id)
                                             .Include(x => x.Rooms)
                                             .ThenInclude(x => x.Room)
                                             .ThenInclude(x => x.Amenities)
                                             .ThenInclude(x => x.Amenity)
                                             .FirstOrDefaultAsync();

            HotelDto dto = new HotelDto()
            {
                Id = hotel.Id,
                Name = hotel.Name,
                StreetAddress = hotel.StreetAddress,
                City = hotel.City,
                State = hotel.State,
                Phone = hotel.Phone,
                Rooms = new List<HotelRoomDto>()
            };

            foreach (var hotelRoom in hotel.Rooms)
            {
                HotelRoomDto hotelRoomDto = new HotelRoomDto()
                {
                    HotelId = hotelRoom.HotelId,
                    RoomNumber = hotelRoom.RoomNumber,
                    Rate = hotelRoom.Rate,
                    PetFriendly = hotelRoom.PetFriendly,
                    RoomId = hotelRoom.RoomId,
                    Room = new RoomDto()
                };

                if (hotelRoom.Room != null)
                {
                    hotelRoomDto.Room.Id = hotelRoom.Room.Id;
                    hotelRoomDto.Room.Name = hotelRoom.Room.Name;
                    hotelRoomDto.Room.Layout = hotelRoom.Room.Layout.ToString();
                    hotelRoomDto.Room.Amenities = new List<AmenityDto>();

                    foreach (var amenity in hotelRoom.Room.Amenities)
                        hotelRoomDto.Room.Amenities.Add(new AmenityDto()
                        {
                            Id = amenity.Amenity.Id,
                            Name = amenity.Amenity.Name
                        });
                }

                dto.Rooms.Add(hotelRoomDto);
            }

            return dto;
        }

        public async Task<List<HotelDto>> GetHotels()
        {
            var hotels = await _context.Hotels.Include(x => x.Rooms)
                                              .ThenInclude(x => x.Room)
                                              .ToListAsync();

            List<HotelDto> dtos = new List<HotelDto>();

            foreach (var hotel in hotels)
                dtos.Add(await GetHotel(hotel.Id));

            return dtos;
        }

        public async Task UpdateHotel(HotelDto dto)
        {
            Hotel hotel = new Hotel()
            {
                Name = dto.Name,
                StreetAddress = dto.StreetAddress,
                City = dto.City,
                State = dto.State,
                Phone = dto.Phone
            };

            _context.Entry(hotel).State = EntityState.Modified;

            await _context.SaveChangesAsync();
        }
    }
}
