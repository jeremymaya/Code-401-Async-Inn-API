using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AsyncInnAPI.Models
{
    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [EnumDataType(typeof(Layout))]
        public Layout Layout { get; set; }

        public ICollection<RoomAmenity> Amenities { get; set; }
        public ICollection<HotelRoom> Rooms { get; set; }
    }

    public enum Layout
    {
        Studio,
        OneBedroom,
        TwoBedroom
    }
}
