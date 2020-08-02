using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AsyncInnAPI.Models.Interfaces;
using AsyncInnAPI.Models.Dtos;

namespace AsyncInnAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AmenitiesController : ControllerBase
    {
        private readonly IAmenityManager _amenity;

        public AmenitiesController(IAmenityManager amenity)
        {
            _amenity = amenity;
        }

        // GET: api/Amenities
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AmenityDto>>> GetAmenities()
        {
            return await _amenity.GetAmenities();
        }

        // GET: api/Amenities/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AmenityDto>> GetAmenity(int id)
        {
            var amenityDto = await _amenity.GetAmenity(id);

            if (amenityDto == null)
            {
                return NotFound();
            }

            return amenityDto;
        }

        // PUT: api/Amenities/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAmenity(int id, AmenityDto amenityDto)
        {
            if (id != amenityDto.Id)
            {
                return BadRequest();
            }

            try
            {
                await _amenity.UpdateAmenity(amenityDto);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AmenityExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Amenities
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<AmenityDto>> PostAmenity(AmenityDto amenityDto)
        {
            await _amenity.CreateAmenity(amenityDto);

            return CreatedAtAction("GetAmenity", new { id = amenityDto.Id }, amenityDto);
        }

        // DELETE: api/Amenities/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<AmenityDto>> DeleteAmenity(int id)
        {
            var amenityDto = await _amenity.GetAmenity(id);
            if (amenityDto == null)
            {
                return NotFound();
            }

            await _amenity.DeleteAmenity(id);

            return amenityDto;
        }

        private bool AmenityExists(int id)
        {
            return _amenity.GetAmenities().Result.Any(e => e.Id == id);
        }
    }
}
