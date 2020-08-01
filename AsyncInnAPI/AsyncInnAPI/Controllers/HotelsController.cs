using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AsyncInnAPI.Models;
using AsyncInnAPI.Models.Interfaces;
using AsyncInnAPI.Models.Dtos;

namespace AsyncInnAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelsController : ControllerBase
    {
        private readonly IHotelManager _hotel;

        public HotelsController(IHotelManager hotel)
        {
            _hotel = hotel;
        }

        // GET: api/Hotels
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HotelDto>>> GetHotels()
        {
            return await _hotel.GetHotels();
        }

        // GET: api/Hotels/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HotelDto>> GetHotel(int id)
        {
            var hotelDto = await _hotel.GetHotel(id);

            if (hotelDto == null)
            {
                return NotFound();
            }

            return hotelDto;
        }

        // PUT: api/Hotels/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHotel(int id, HotelDto hotelDto)
        {
            if (id != hotelDto.Id)
            {
                return BadRequest();
            }

            try
            {
                await _hotel.UpdateHotel(hotelDto);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HotelExists(id))
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

        // POST: api/Hotels
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<HotelDto>> PostHotel(HotelDto hotelDto)
        {
            await _hotel.CreateHotel(hotelDto);

            return CreatedAtAction("GetHotel", new { id = hotelDto.Id }, hotelDto);
        }

        // DELETE: api/Hotels/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<HotelDto>> DeleteHotel(int id)
        {
            var hotelDto = await _hotel.GetHotel(id);
            if (hotelDto == null)
            {
                return NotFound();
            }

            await _hotel.DeleteHotel(id);

            return hotelDto;
        }

        private bool HotelExists(int id)
        {
            return _hotel.GetHotels().Result.Any(e => e.Id == id);
        }
    }
}
