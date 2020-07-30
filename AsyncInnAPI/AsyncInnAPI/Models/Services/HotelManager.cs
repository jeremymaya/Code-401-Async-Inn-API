using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AsyncInnAPI.Data;
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

        public async Task<Hotel> CreateHotel(Hotel hotel)
        {
            _context.Entry(hotel).State = EntityState.Added;

            await _context.SaveChangesAsync();

            return hotel;
        }

        public async Task DeleteHotel(int id)
        {
            var hotel = await GetHotel(id);

            _context.Entry(hotel).State = EntityState.Deleted;

            await _context.SaveChangesAsync();
        }

        public async Task<Hotel> GetHotel(int id)
        {
            var hotel = await _context.Hotels.FindAsync(id);

            return hotel;
        }

        public async Task<List<Hotel>> GetHotels()
        {
            var hotels = await _context.Hotels.ToListAsync();

            return hotels;
        }

        public async Task UpdateHotel(Hotel hotel)
        {
            _context.Entry(hotel).State = EntityState.Modified;

            await _context.SaveChangesAsync();
        }
    }
}
