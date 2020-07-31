using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AsyncInnAPI.Models;
using AsyncInnAPI.Models.Interfaces;

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
        public async Task<ActionResult<IEnumerable<Room>>> GetRooms()
        {
            return await _room.GetRooms();
        }

        // GET: api/Rooms/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Room>> GetRoom(int id)
        {
            var room = await _room.GetRoom(id);

            if (room == null)
            {
                return NotFound();
            }

            return room;
        }

        // PUT: api/Rooms/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRoom(int id, Room room)
        {
            if (id != room.Id)
            {
                return BadRequest();
            }

            try
            {
                await _room.UpdateRoom(room);
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
        public async Task<ActionResult<Room>> PostRoom(Room room)
        {
            await _room.UpdateRoom(room);

            return CreatedAtAction("GetRoom", new { id = room.Id }, room);
        }

        // DELETE: api/Rooms/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Room>> DeleteRoom(int id)
        {
            var room = await _room.GetRoom(id);
            if (room == null)
            {
                return NotFound();
            }

            await _room.DeleteRoom(id);

            return room;
        }

        [HttpPost]
        [Route("{roomId}/Amenity/{amenityId}")]
        public async Task<ActionResult<Room>> AddAmenityToRoom(int roomId, int amenityId)
        {
            var room = await _room.AddAmenityToRoom(roomId, amenityId);

            return room;
        }

        [HttpDelete]
        [Route("{roomId}/Amenity/{amenityId}")]
        public async Task<ActionResult<Room>> RemoveAmentityFromRoom(int roomId, int amenityId)
        {
            var room = await _room.GetRoom(roomId);
            if (room == null)
            {
                return NotFound();
            }

            await _room.RemoveAmentityFromRoom(roomId, amenityId);

            return room;
        }

        private bool RoomExists(int id)
        {
            return _room.GetRooms().Result.Any(e => e.Id == id);
        }
    }
}
