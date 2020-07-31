using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AsyncInnAPI.Models.Interfaces
{
    public interface IRoomManager
    {
        Task<Room> CreateRoom(Room room);
        Task DeleteRoom(int id);
        Task<List<Room>> GetRooms();
        Task<Room> GetRoom(int id);
        Task UpdateRoom(Room room);
        Task<Room> AddAmenityToRoom(int roomId, int amenityId);
        Task RemoveAmentityFromRoom(int roomId, int amenityId);
    }
}
