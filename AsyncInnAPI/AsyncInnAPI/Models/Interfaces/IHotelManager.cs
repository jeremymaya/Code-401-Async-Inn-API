using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AsyncInnAPI.Models.Dtos;

namespace AsyncInnAPI.Models.Interfaces
{
    public interface IHotelManager
    {
        Task<HotelDto> CreateHotel(HotelDto dto);
        Task DeleteHotel(int id);
        Task<List<HotelDto>> GetHotels();
        Task<HotelDto> GetHotel(int id);
        Task UpdateHotel(HotelDto dto);
    }
}
