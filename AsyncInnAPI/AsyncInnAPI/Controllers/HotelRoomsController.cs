using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AsyncInnAPI.Models.Dtos;
using AsyncInnAPI.Models.Interfaces;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Policy = "AgentPrivilege")]
        [HttpGet]
        [Route("{hotelId}/Rooms")]
        public async Task<ActionResult<IEnumerable<HotelRoomDto>>> GetHotelRooms(int hotelId)
        {
            return await _hotelRoom.GetHotelRooms(hotelId);
        }

        // GET: api/Hotels/{hotelId}/Rooms/{roomNumber}
        [Authorize(Policy = "AgentPrivilege")]
        [HttpGet]
        [Route("{hotelId}/Rooms/{roomNumber}")]
        public async Task<ActionResult<HotelRoomDto>> GetHotelRoom(int hotelId, int roomNumber)
        {
            var hotelRoomDto = await _hotelRoom.GetHotelRoom(hotelId, roomNumber);

            if (hotelRoomDto == null)
            {
                return NotFound();
            }

            return hotelRoomDto;
        }

        // PUT: api/Hotels/{hotelId}/Rooms/{roomNumber}
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [Authorize(Policy = "AgentPrivilege")]
        [HttpPut]
        [Route("{hotelId}/Rooms/{roomNumber}")]
        public async Task<IActionResult> PutHotelRoom(int hotelId, HotelRoomDto hotelRoomDto)
        {
            if (hotelId != hotelRoomDto.HotelId)
            {
                return BadRequest();
            }

            try
            {
                await _hotelRoom.UpdateHotelRoom(hotelRoomDto);
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
        [Authorize(Policy = "PropertyManagerPrivilege")]
        [HttpPost]
        [Route("{hotelId}/Rooms")]
        public async Task<ActionResult<HotelRoomDto>> PostHotelRoom(int hotelId, HotelRoomDto hotelRoomDto)
        {
            await _hotelRoom.CreateHotelRoom(hotelRoomDto);

            return CreatedAtAction("GetHotelRoom", new { id = hotelRoomDto.HotelId }, hotelRoomDto);
        }

        // DELETE: api/Hotels/{hotelId}/Rooms/{roomNumber}
        [Authorize(Policy = "PropertyManagerPrivilege")]
        [HttpDelete]
        [Route("{hotelId}/Rooms/{roomNumber}")]
        public async Task<ActionResult<HotelRoomDto>> DeleteHotelRoom(int hotelId, int roomNumber)
        {
            var hotelRoomDto = await _hotelRoom.GetHotelRoom(hotelId, roomNumber);
            if (hotelRoomDto == null)
            {
                return NotFound();
            }

            await _hotelRoom.DeleteHotelRoom(hotelId, roomNumber);

            return hotelRoomDto;
        }

        private bool HotelRoomExists(int hotelId)
        {
            return _hotelRoom.GetHotelRooms(hotelId).Result.Any(e => e.HotelId == hotelId);
        }
    }
}