using System;
using System.ComponentModel.DataAnnotations;

namespace AsyncInnAPI.Models
{
    public class Room
    {
        public int ID { get; set; }
        public string Name { get; set; }
        [EnumDataType(typeof(Layout))]
        public Layout Layout { get; set; }
    }

    public enum Layout
    {
        Studio,
        OneBedroom,
        TwoBedroom
    }
}
