using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AsyncInnAPI.Models.Interfaces
{
    public interface IAmenityManager
    {
        Task<Amenity> CreateAmenity(Amenity amenity);
        Task DeleteAmenity(int id);
        Task<List<Amenity>> GetAmenities();
        Task<Amenity> GetAmenity(int id);
        Task UpdateAmenity(Amenity amenity);
    }
}
