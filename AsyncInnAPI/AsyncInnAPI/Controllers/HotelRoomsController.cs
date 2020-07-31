using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AsyncInnAPI.Models;
using AsyncInnAPI.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AsyncInnAPI.Controllers
{
    [Route("api/Hotels")]
    [ApiController]
    public class HotelRoomsController : ControllerBase
    {
        private readonly IHotelRoomManager _hotelRoom;

        public HotelRoomsController(IHotelRoomManager hotelRoom)
        {
            _hotelRoom = hotelRoom;
        }

        // GET: api/Hotels/{hotelId}/Rooms
        [HttpGet]
        [Route("{hotelId}/Rooms")]
        public async Task<ActionResult<IEnumerable<HotelRoom>>> GetHotelRooms(int hotelId)
        {
            return await _hotelRoom.GetHotelRooms(hotelId);
        }

        // GET: api/Hotels/{hotelId}/Rooms/{roomNumber}
        [HttpGet]
        [Route("{hotelId}/Rooms/{roomNumber}")]
        public async Task<ActionResult<HotelRoom>> GetHotelRoom(int hotelId, int roomNumber)
        {
            var hotelRoom = await _hotelRoom.GetHotelRoom(hotelId, roomNumber);

            if (hotelRoom == null)
            {
                return NotFound();
            }

            return hotelRoom;
        }

        // PUT: api/Hotels/{hotelId}/Rooms/{roomNumber}
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut]
        [Route("{hotelId}/Rooms/{roomNumber}")]
        public async Task<IActionResult> PutHotelRoom(int hotelId, HotelRoom hotelRoom)
        {
            if (hotelId != hotelRoom.HotelId)
            {
                return BadRequest();
            }

            try
            {
                await _hotelRoom.UpdateHotelRoom(hotelRoom);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HotelRoomExists(hotelId))
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

        // POST: api/Hotels/{hotelId}/Rooms
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        [Route("{hotelId}/Rooms")]
        public async Task<ActionResult<HotelRoom>> PostHotelRoom(int hotelId, HotelRoom hotelRoom)
        {
            await _hotelRoom.CreateHotelRoom(hotelRoom);

            return CreatedAtAction("GetHotelRoom", new { id = hotelRoom.HotelId }, hotelRoom);
        }

        // DELETE: api/Hotels/5
        [HttpDelete]
        [Route("{hotelId}/Rooms/{roomNumber}")]
        public async Task<ActionResult<HotelRoom>> DeleteHotelRoom(int hotelId, int roomNumber)
        {
            var hotelRoom = await _hotelRoom.GetHotelRoom(hotelId, roomNumber);
            if (hotelRoom == null)
            {
                return NotFound();
            }

            await _hotelRoom.DeleteHotelRoom(hotelId, roomNumber);

            return hotelRoom;
        }

        private bool HotelRoomExists(int hotelId)
        {
            return _hotelRoom.GetHotelRooms(hotelId).Result.Any(e => e.HotelId == hotelId);
        }
    }
}