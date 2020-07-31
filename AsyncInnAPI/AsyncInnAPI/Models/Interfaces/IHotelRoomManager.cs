using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AsyncInnAPI.Models.Interfaces
{
    public interface IHotelRoomManager
    {
        Task<HotelRoom> CreateHotelRoom(HotelRoom hotelRoom);
        Task DeleteHotelRoom(int hotelId, int roomNumber);
        Task<List<HotelRoom>> GetHotelRooms(int hotelId);
        Task<HotelRoom> GetHotelRoom(int hotelId, int roomNumber);
        Task UpdateHotelRoom(HotelRoom hotelRoom);
    }
}
