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
    public class RoomsController : ControllerBase
    {
        private readonly IRoomManager _room;

        public RoomsController(IRoomManager room)
        {
            _room = room;
        }

        // GET: api/Rooms
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoomDto>>> GetRooms()
        {
            return await _room.GetRooms();
        }

        // GET: api/Rooms/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RoomDto>> GetRoom(int id)
        {
            var roomDto = await _room.GetRoom(id);

            if (roomDto == null)
            {
                return NotFound();
            }

            return roomDto;
        }

        // PUT: api/Rooms/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRoom(int id, RoomDto roomDto)
        {
            if (id != roomDto.Id)
            {
                return BadRequest();
            }

            try
            {
                await _room.UpdateRoom(roomDto);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoomExists(id))
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

        // POST: api/Rooms
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<RoomDto>> PostRoom(RoomDto roomDto)
        {
            await _room.UpdateRoom(roomDto);

            return CreatedAtAction("GetRoom", new { id = roomDto.Id }, roomDto);
        }

        // DELETE: api/Rooms/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<RoomDto>> DeleteRoom(int id)
        {
            var roomDto = await _room.GetRoom(id);
            if (roomDto == null)
            {
                return NotFound();
            }

            await _room.DeleteRoom(id);

            return roomDto;
        }

        // POST: api/Rooms/{roomId}/Amenity/{amenityId}
        [HttpPost]
        [Route("{roomId}/Amenity/{amenityId}")]
        public async Task<ActionResult<RoomDto>> AddAmenityToRoom(int roomId, int amenityId)
        {
            var roomDto = await _room.AddAmenityToRoom(roomId, amenityId);

            return roomDto;
        }

        // DELETE: api/Rooms/{roomId}/Amenity/{amenityId}
        [HttpDelete]
        [Route("{roomId}/Amenity/{amenityId}")]
        public async Task<ActionResult<RoomDto>> RemoveAmentityFromRoom(int roomId, int amenityId)
        {
            var roomDto = await _room.GetRoom(roomId);
            if (roomDto == null)
            {
                return NotFound();
            }

            await _room.RemoveAmentityFromRoom(roomId, amenityId);

            roomDto = await _room.GetRoom(roomId);

            return roomDto;
        }

        private bool RoomExists(int id)
        {
            return _room.GetRooms().Result.Any(e => e.Id == id);
        }
    }
}
