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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Hotel>().HasData(
                new Hotel
                {
                    ID = 1,
                    Name = "Hotel One",
                    StreetAddress = "1111 One Street",
                    City = "One",
                    State = "Number",
                    Phone = "111-111-1111"
                },
                new Hotel
                {
                    ID = 2,
                    Name = "Hotel Two",
                    StreetAddress = "2222 Two Street",
                    City = "Two",
                    State = "Number",
                    Phone = "222-222-2222"
                },
                new Hotel
                {
                    ID = 3,
                    Name = "Hotel Three",
                    StreetAddress = "3333 Three Street",
                    City = "Three",
                    State = "Number",
                    Phone = "333-333-3333"
                },
                new Hotel
                {
                    ID = 4,
                    Name = "Hotel Four",
                    StreetAddress = "4444 Four Street",
                    City = "Four",
                    State = "Number",
                    Phone = "206-681-4444"
                },
                new Hotel
                {
                    ID = 5,
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
                    ID = 1,
                    Name = "Room One",
                    Layout = Layout.OneBedroom
                },
                new Room
                {
                    ID = 2,
                    Name = "Room Two",
                    Layout = Layout.Studio
                },
                new Room
                {
                    ID = 3,
                    Name = "Room Three",
                    Layout = Layout.TwoBedroom
                },
                new Room
                {
                    ID = 4,
                    Name = "Room Four",
                    Layout = Layout.Studio
                },
                new Room
                {
                    ID = 5,
                    Name = "Room Five",
                    Layout = Layout.OneBedroom
                },
                new Room
                {
                    ID = 6,
                    Name = "Room Six",
                    Layout = Layout.TwoBedroom
                }
                );

            modelBuilder.Entity<Amenity>().HasData(
                new Amenity
                {
                    ID = 1,
                    Name = "Hair Dryer"
                },
                new Amenity
                {
                    ID = 2,
                    Name = "Water"
                },
                new Amenity
                {
                    ID = 3,
                    Name = "Extra Pillows"
                },
                new Amenity
                {
                    ID = 4,
                    Name = "Extra Blankets"
                },
                new Amenity
                {
                    ID = 5,
                    Name = "Wine"
                }
                );
        }

        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Amenity> Amenities { get; set; }
    }
}
