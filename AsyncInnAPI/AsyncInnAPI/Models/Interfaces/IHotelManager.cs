using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AsyncInnAPI.Models.Interfaces
{
    public interface IHotelManager
    {
        Task<Hotel> CreateHotel(Hotel hotel);
        Task DeleteHotel(int id);
        Task<List<Hotel>> GetHotels();
        Task<Hotel> GetHotel(int id);
        Task UpdateHotel(Hotel hotel);
    }
}
