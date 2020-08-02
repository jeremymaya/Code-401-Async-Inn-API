using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AsyncInnAPI.Models.Dtos;

namespace AsyncInnAPI.Models.Interfaces
{
    public interface IAmenityManager
    {
        Task<AmenityDto> CreateAmenity(AmenityDto dto);
        Task DeleteAmenity(int id);
        Task<List<AmenityDto>> GetAmenities();
        Task<AmenityDto> GetAmenity(int id);
        Task UpdateAmenity(AmenityDto dto);
    }
}
