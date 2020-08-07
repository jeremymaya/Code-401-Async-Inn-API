using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AsyncInnAPI.Models.Dtos;
using AsyncInnAPI.Models.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AsyncInnAPI.Controllers
{
    [Produces("application/json")]
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
        /// <summary>
        /// Gets a list of rooms in a hotel
        /// </summary>
        /// <param name="hotelId">Hotel Id</param>
        /// <returns>A list of data transfer object containing hotel information</returns>
        [AllowAnonymous]
        [HttpGet]
        [Route("{hotelId}/Rooms")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<HotelRoomDto>>> GetHotelRooms(int hotelId)
        {
            return await _hotelRoom.GetHotelRooms(hotelId);
        }

        // GET: api/Hotels/{hotelId}/Rooms/{roomNumber}
        /// <summary>
        /// Gets a room from a hotel
        /// </summary>
        /// <param name="hotelId">Hotel Id></param>
        /// <param name="roomNumber">Room Number</param>
        /// <returns>A data transfer object containing room infromation in the hotel</returns>
        [AllowAnonymous]
        [HttpGet]
        [Route("{hotelId}/Rooms/{roomNumber}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
        /// <summary>
        /// Updates a room details in a hotel
        /// </summary>
        /// <param name="hotelId">Hotel Id</param>
        /// <param name="hotelRoomDto">A data transfer object containing room infromation</param>
        /// <returns>A response code</returns>
        [Authorize(Policy = "AgentPrivilege")]
        [HttpPut]
        [Route("{hotelId}/Rooms/{roomNumber}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
        /// <summary>
        /// Creates a room in a hotel
        /// </summary>
        /// <param name="hotelId">Hotel Id</param>
        /// <param name="hotelRoomDto">A data transfer object containing room infromation</param>
        /// <returns>A data transfer object containing newly created room information</returns>
        [Authorize(Policy = "PropertyManagerPrivilege")]
        [HttpPost]
        [Route("{hotelId}/Rooms")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<HotelRoomDto>> PostHotelRoom(int hotelId, HotelRoomDto hotelRoomDto)
        {
            await _hotelRoom.CreateHotelRoom(hotelId, hotelRoomDto);

            return CreatedAtAction("GetHotelRoom", new { id = hotelId}, hotelRoomDto);
        }

        // DELETE: api/Hotels/{hotelId}/Rooms/{roomNumber}
        /// <summary>
        /// Deletes an amenity
        /// </summary>
        /// <param name="hotelId">Hotel Id</param>
        /// <param name="roomNumber">Room Number</param>
        /// <returns>A data transfer object containing deleted room information</returns>
        [Authorize(Policy = "PropertyManagerPrivilege")]
        [HttpDelete]
        [Route("{hotelId}/Rooms/{roomNumber}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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