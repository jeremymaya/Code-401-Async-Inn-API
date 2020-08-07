using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AsyncInnAPI.Models.Interfaces;
using AsyncInnAPI.Models.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace AsyncInnAPI.Controllers
{
    [Produces("application/json")]
    [Authorize(Policy = "DistrictManagerPrivilege")]
    [Route("api/[controller]")]
    [ApiController]
    public class AmenitiesController : ControllerBase
    {
        private readonly IAmenityManager _amenity;

        public AmenitiesController(IAmenityManager amenity)
        {
            _amenity = amenity;
        }

        /// <summary>
        /// Gets a list of amenities
        /// </summary>
        /// <returns>A list of data transfer object containing amenity information</returns>
        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<AmenityDto>>> GetAmenities()
        {
            return await _amenity.GetAmenities();
        }

        /// <summary>
        /// Gets an amenity
        /// </summary>
        /// <param name="id">Amenity Id</param>
        /// <returns>A data transfer object containing amenity infromation</returns>
        [AllowAnonymous]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AmenityDto>> GetAmenity(int id)
        {
            var amenityDto = await _amenity.GetAmenity(id);

            if (amenityDto == null)
            {
                return NotFound();
            }

            return amenityDto;
        }

        /// <summary>
        /// Updates an amenity details
        /// </summary>
        /// <param name="id">Amenity Id</param>
        /// <param name="amenityDto">A data transfer object containing amenity infromation</param>
        /// <returns>A response code</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        /// <summary>
        /// Creates an amenity
        /// </summary>
        /// <param name="amenityDto">A data transfer object containing amenity infromation</param>
        /// <returns>A data transfer object containing newly created amenity information</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<AmenityDto>> PostAmenity(AmenityDto amenityDto)
        {
            await _amenity.CreateAmenity(amenityDto);

            return CreatedAtAction("GetAmenity", new { id = amenityDto.Id }, amenityDto);
        }

        // DELETE: api/Amenities/5
        /// <summary>
        /// Deletes an amenity
        /// </summary>
        /// <param name="id">Amenity Id</param>
        /// <returns>A data transfer object containing deleted amenity information</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
