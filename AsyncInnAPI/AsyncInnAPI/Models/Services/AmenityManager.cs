using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AsyncInnAPI.Data;
using AsyncInnAPI.Models.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AsyncInnAPI.Models.Services
{
    public class AmenityManager : IAmenityManager
    {
        private AsyncInnDbContext _context;

        public AmenityManager(AsyncInnDbContext context)
        {
            _context = context;
        }

        public async Task<Amenity> CreateAmenity(Amenity amenity)
        {
            _context.Entry(amenity).State = EntityState.Added;

            await _context.SaveChangesAsync();

            return amenity;
        }

        public async Task DeleteAmenity(int id)
        {
            var amenity = await GetAmenity(id);

            _context.Entry(amenity).State = EntityState.Deleted;

            await _context.SaveChangesAsync();
        }

        public async Task<List<Amenity>> GetAmenities()
        {
            var amenities = await _context.Amenities.ToListAsync();

            return amenities;
        }

        public async Task<Amenity> GetAmenity(int id)
        {
            var amenity = await _context.Amenities.FindAsync(id);

            return amenity;
        }

        public async Task UpdateAmenity(Amenity amenity)
        {
            _context.Entry(amenity).State = EntityState.Modified;

            await _context.SaveChangesAsync();
        }
    }
}
