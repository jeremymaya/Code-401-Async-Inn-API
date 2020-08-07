using System;
using AsyncInnAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AsyncInnAPI.Data
{
    public class AsyncInnDbContext : DbContext
    {
        public AsyncInnDbContext(DbContextOptions<AsyncInnDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql("Host=db;Database=AsyncInnDbContext;Username=sa;Password=ReallyStrongPassword1234!");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RoomAmenity>().HasKey(ce => new { ce.RoomId, ce.AmenityId });
            modelBuilder.Entity<HotelRoom>().HasKey(ce => new { ce.HotelId, ce.RoomNumber});

            modelBuilder.Entity<Hotel>().HasData(
                new Hotel
                {
                    Id = 1,
                    Name = "Hotel One",
                    StreetAddress = "1111 One Street",
                    City = "One",
                    State = "Number",
                    Phone = "111-111-1111"
                },
                new Hotel
                {
                    Id = 2,
                    Name = "Hotel Two",
                    StreetAddress = "2222 Two Street",
                    City = "Two",
                    State = "Number",
                    Phone = "222-222-2222"
                },
                new Hotel
                {
                    Id = 3,
                    Name = "Hotel Three",
                    StreetAddress = "3333 Three Street",
                    City = "Three",
                    State = "Number",
                    Phone = "333-333-3333"
                },
                new Hotel
                {
                    Id = 4,
                    Name = "Hotel Four",
                    StreetAddress = "4444 Four Street",
                    City = "Four",
                    State = "Number",
                    Phone = "206-681-4444"
                },
                new Hotel
                {
                    Id = 5,
                    Name = "Hotel Five",
                    StreetAddress = "5555 Five Street",
                    City = "Five",
                    State = "Number",
                    Phone = "555-555-5555"
                }
                );

            modelBuilder.Entity<Room>().HasData(
                new Room
                {
                    Id = 1,
                    Name = "Room One",
                    Layout = Layout.OneBedroom
                },
                new Room
                {
                    Id = 2,
                    Name = "Room Two",
                    Layout = Layout.Studio
                },
                new Room
                {
                    Id = 3,
                    Name = "Room Three",
                    Layout = Layout.TwoBedroom
                },
                new Room
                {
                    Id = 4,
                    Name = "Room Four",
                    Layout = Layout.Studio
                },
                new Room
                {
                    Id = 5,
                    Name = "Room Five",
                    Layout = Layout.OneBedroom
                },
                new Room
                {
                    Id = 6,
                    Name = "Room Six",
                    Layout = Layout.TwoBedroom
                }
                );

            modelBuilder.Entity<Amenity>().HasData(
                new Amenity
                {
                    Id = 1,
                    Name = "Hair Dryer"
                },
                new Amenity
                {
                    Id = 2,
                    Name = "Water"
                },
                new Amenity
                {
                    Id = 3,
                    Name = "Extra Pillows"
                },
                new Amenity
                {
                    Id = 4,
                    Name = "Extra Blankets"
                },
                new Amenity
                {
                    Id = 5,
                    Name = "Wine"
                }
                );
            modelBuilder.Entity<RoomAmenity>().HasData(
                new RoomAmenity
                {
                    RoomId = 1,
                    AmenityId = 1
                },
                new RoomAmenity
                {
                    RoomId = 1,
                    AmenityId = 2
                },
                new RoomAmenity
                {
                    RoomId = 2,
                    AmenityId = 3
                },
                new RoomAmenity
                {
                    RoomId = 2,
                    AmenityId = 4
                },
                new RoomAmenity
                {
                    RoomId = 3,
                    AmenityId = 5
                }
                );
            modelBuilder.Entity<HotelRoom>().HasData(
                new HotelRoom
                {
                    HotelId = 1,
                    RoomNumber = 101,
                    RoomId = 1,
                    Rate = 150,
                    PetFriendly = true
                },
                new HotelRoom
                {
                    HotelId = 1,
                    RoomNumber = 201,
                    RoomId = 2,
                    Rate = 100,
                    PetFriendly = false
                },
                new HotelRoom
                {
                    HotelId = 2,
                    RoomNumber = 301,
                    RoomId = 3,
                    Rate = 50,
                    PetFriendly = true
                }
                );
        }

        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Amenity> Amenities { get; set; }
        public DbSet<RoomAmenity> RoomAmenities { get; set; }
        public DbSet<HotelRoom> HotelRooms { get; set; }
    }
}
