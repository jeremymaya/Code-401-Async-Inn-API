using System;
namespace AsyncInnAPI.Models
{
    public class RoomAmenity
    {
        public int RoomId { get; set; }
        public int AmenityId { get; set; }

        public Amenity Amenity { get; set; }
        public Room Room { get; set; }
    }
}
