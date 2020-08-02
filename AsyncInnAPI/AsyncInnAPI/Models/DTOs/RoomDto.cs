using System;
using System.Collections.Generic;

namespace AsyncInnAPI.Models.Dtos
{
    public class RoomDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Layout { get; set; }
        public List<AmenityDto> Amenities { get; set; }
    }
}
