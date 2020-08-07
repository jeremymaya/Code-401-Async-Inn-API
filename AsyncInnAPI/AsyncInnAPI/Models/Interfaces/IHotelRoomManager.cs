using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AsyncInnAPI.Models.Dtos;

namespace AsyncInnAPI.Models.Interfaces
{
    public interface IHotelRoomManager
    {
        Task<HotelRoomDto> CreateHotelRoom(int hotelId, HotelRoomDto dto);
        Task DeleteHotelRoom(int hotelId, int roomNumber);
        Task<List<HotelRoomDto>> GetHotelRooms(int hotelId);
        Task<HotelRoomDto> GetHotelRoom(int hotelId, int roomNumber);
        Task UpdateHotelRoom(HotelRoomDto dto);
    }
}
