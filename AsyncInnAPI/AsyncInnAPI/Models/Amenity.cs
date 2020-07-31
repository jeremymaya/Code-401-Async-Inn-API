using System;
using System.Collections.Generic;

namespace AsyncInnAPI.Models
{
    public class Amenity
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<RoomAmenity> Amenities { get; set; }
    }
}
