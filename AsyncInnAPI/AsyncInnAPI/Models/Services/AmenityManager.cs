using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AsyncInnAPI.Data;
using AsyncInnAPI.Models.Dtos;
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

        public async Task<AmenityDto> CreateAmenity(AmenityDto dto)
        {
            Amenity amenity = new Amenity()
            {
                Name = dto.Name
            };

            _context.Entry(amenity).State = EntityState.Added;

            await _context.SaveChangesAsync();

            dto.Id = amenity.Id;

            return dto;
        }

        public async Task DeleteAmenity(int id)
        {
            var amenity = await _context.Amenities.FindAsync(id);

            _context.Entry(amenity).State = EntityState.Deleted;

            await _context.SaveChangesAsync();
        }

        public async Task<List<AmenityDto>> GetAmenities()
        {
            var amenities = await _context.Amenities.ToListAsync();

            List<AmenityDto> dtos = new List<AmenityDto>();

            foreach (var amenity in amenities)
                dtos.Add(await GetAmenity(amenity.Id));

            return dtos;
        }

        public async Task<AmenityDto> GetAmenity(int id)
        {
            var amenity = await _context.Amenities.FindAsync(id);

            AmenityDto dto = new AmenityDto()
            {
                Id = amenity.Id,
                Name = amenity.Name
            };

            return dto;
        }

        public async Task UpdateAmenity(AmenityDto dto)
        {
            var amenity = new Amenity()
            {
                Id = dto.Id,
                Name = dto.Name
            };

            _context.Entry(amenity).State = EntityState.Modified;

            await _context.SaveChangesAsync();
        }
    }
}
