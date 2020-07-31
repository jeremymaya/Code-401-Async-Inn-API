using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AsyncInnAPI.Models.Dtos;

namespace AsyncInnAPI.Models.Interfaces
{
    public interface IRoomManager
    {
        Task<RoomDto> CreateRoom(RoomDto dto);
        Task DeleteRoom(int id);
        Task<List<RoomDto>> GetRooms();
        Task<RoomDto> GetRoom(int id);
        Task UpdateRoom(RoomDto dto);
        Task<RoomDto> AddAmenityToRoom(int roomId, int amenityId);
        Task RemoveAmentityFromRoom(int roomId, int amenityId);
    }
}
