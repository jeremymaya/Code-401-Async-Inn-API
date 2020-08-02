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
        private readonly AsyncInnDbContext _context;
        private readonly IHotelRoomManager _hotelRooms;

        public HotelManager(AsyncInnDbContext context, IHotelRoomManager hotelRooms)
        {
            _context = context;
            _hotelRooms = hotelRooms;
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
                Rooms = await _hotelRooms.GetHotelRooms(hotel.Id)
            };

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
            var hotel = await _context.Hotels.Where(x => x.Id == dto.Id)
                                 .Include(x => x.Rooms)
                                 .ThenInclude(x => x.Room)
                                 .ThenInclude(x => x.Amenities)
                                 .ThenInclude(x => x.Amenity)
                                 .FirstOrDefaultAsync();

            hotel.Name = dto.Name;
            hotel.StreetAddress = dto.StreetAddress;
            hotel.City = dto.City;
            hotel.State = dto.State;
            hotel.Phone = dto.Phone;

            _context.Entry(hotel).State = EntityState.Modified;

            await _context.SaveChangesAsync();
        }
    }
}
