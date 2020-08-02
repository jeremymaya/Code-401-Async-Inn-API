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

        /// <summary>
        /// Creates an amenity entry in the database
        /// </summary>
        /// <param name="dto">Data transfer object containing an amenity properties to be created</param>
        /// <returns>Data transfer object with an Id</returns>
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

        /// <summary>
        /// Deletes an amenity entry in the database based on the Id
        /// </summary>
        /// <param name="id">Id of the amenity to be deleted</param>
        public async Task DeleteAmenity(int id)
        {
            var amenity = await _context.Amenities.FindAsync(id);

            _context.Entry(amenity).State = EntityState.Deleted;

            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Gets a list of data transfer objects containing amenity information
        /// </summary>
        /// <returns>List of data transfer objects containing amenity c</returns>
        public async Task<List<AmenityDto>> GetAmenities()
        {
            var amenities = await _context.Amenities.ToListAsync();

            List<AmenityDto> dtos = new List<AmenityDto>();

            foreach (var amenity in amenities)
                dtos.Add(await GetAmenity(amenity.Id));

            return dtos;
        }

        /// <summary>
        /// Get a data transfer object containing an amenity information
        /// </summary>
        /// <param name="id">Id of the amenity to be read</param>
        /// <returns>Data transfer object containig an amenity information</returns>
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

        /// <summary>
        /// Updates a transfer object containing an amenity information
        /// </summary>
        /// <param name="dto">Data transfer object containing an amenity information to be updated</param>
        public async Task UpdateAmenity(AmenityDto dto)
        {
            var amenity = await _context.Amenities.FindAsync(dto.Id);

            amenity.Name = dto.Name;

            _context.Entry(amenity).State = EntityState.Modified;

            await _context.SaveChangesAsync();
        }
    }
}
