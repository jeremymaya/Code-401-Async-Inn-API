using System;
namespace AsyncInnAPI.Models.Dtos
{
    public class HotelRoomDto
    {
        public int HotelId { get; set; }
        public int RoomNumber { get; set; }
        public decimal Rate { get; set; }
        public bool PetFriendly { get; set; }
        public int RoomId { get; set; }
        public RoomDto Room { get; set; }
    }
}
