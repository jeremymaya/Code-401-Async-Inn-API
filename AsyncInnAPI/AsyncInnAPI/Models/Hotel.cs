using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AsyncInnAPI.Models
{
    public class Hotel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        public ICollection<HotelRoom> Rooms { get; set; }
    }
}
