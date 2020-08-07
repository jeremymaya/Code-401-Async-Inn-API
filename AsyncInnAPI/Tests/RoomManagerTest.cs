using System;
using System.Collections.Generic;
using System.Linq;
using AsyncInnAPI.Data;
using AsyncInnAPI.Models;
using AsyncInnAPI.Models.Dtos;
using AsyncInnAPI.Models.Interfaces;
using AsyncInnAPI.Models.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Tests
{
    public class RoomManagerTest
    {
        [Fact]
        public async void CanCreateRoomAndSaveToDatabase()
        {
            DbContextOptions<AsyncInnDbContext> options = new DbContextOptionsBuilder<AsyncInnDbContext>()
                .UseInMemoryDatabase("CanCreateRoomAndSaveToDatabase")
                .Options;

            using AsyncInnDbContext context = new AsyncInnDbContext(options);

            AmenityManager amenities = new AmenityManager(context);

            RoomManager service = new RoomManager(context, amenities);

            var dto = new RoomDto() { Name = "Test", Layout = "0" };

            Assert.Equal(0, context.Rooms.CountAsync().Result);

            var result = await service.CreateRoom(dto);

            var actual = context.Rooms.FindAsync(result.Id).Result;

            Assert.Equal(1, await context.Rooms.CountAsync());
            Assert.IsType<Room>(actual);
            Assert.Equal(1, actual.Id);
            Assert.Equal("Test", actual.Name);
            Assert.Equal(Layout.Studio, actual.Layout);
        }

        [Fact]
        public async void CanCreateRoomAndSaveMultipleToDatabase()
        {
            DbContextOptions<AsyncInnDbContext> options = new DbContextOptionsBuilder<AsyncInnDbContext>()
                .UseInMemoryDatabase("CanCreateRoomAndSaveMultipleToDatabase")
                .Options;

            using AsyncInnDbContext context = new AsyncInnDbContext(options);

            AmenityManager amenities = new AmenityManager(context);

            RoomManager service = new RoomManager(context, amenities);

            Assert.Equal(0, context.Rooms.CountAsync().Result);

            var resultOne = await service.CreateRoom(new RoomDto { Name = "Test One", Layout = "0" });
            var resultTwo = await service.CreateRoom(new RoomDto { Name = "Test Two", Layout = "1" });

            var actualOne = await context.Rooms.FindAsync(resultOne.Id);
            var actualTwo = await context.Rooms.FindAsync(resultTwo.Id);

            Assert.Equal(2, await context.Rooms.CountAsync());
            Assert.IsType<Room>(actualOne);
            Assert.Equal(1, actualOne.Id);
            Assert.Equal("Test One", actualOne.Name);
            Assert.Equal(Layout.Studio, actualOne.Layout);
            Assert.IsType<Room>(actualTwo);
            Assert.Equal(2, actualTwo.Id);
            Assert.Equal("Test Two", actualTwo.Name);
            Assert.Equal(Layout.OneBedroom, actualTwo.Layout);
        }

        [Fact]
        public async void CanGetRoomFromDatabase()
        {
            DbContextOptions<AsyncInnDbContext> options = new DbContextOptionsBuilder<AsyncInnDbContext>()
                .UseInMemoryDatabase("CanGetRoomFromDatabase")
                .Options;

            using AsyncInnDbContext context = new AsyncInnDbContext(options);

            AmenityManager amenities = new AmenityManager(context);

            RoomManager service = new RoomManager(context, amenities);

            var result = await service.CreateRoom(new RoomDto { Name = "Test", Layout = "0" });

            var actual = await service.GetRoom(result.Id);

            Assert.IsType<RoomDto>(actual);
            Assert.Equal(1, actual.Id);
            Assert.Equal("Test", actual.Name);
            Assert.Equal("Studio", actual.Layout);
        }

        [Fact]
        public async void CanGetAmenitiesFromDatabase()
        {
            DbContextOptions<AsyncInnDbContext> options = new DbContextOptionsBuilder<AsyncInnDbContext>()
                .UseInMemoryDatabase("CanGetAmenitiesFromDatabase")
                .Options;

            using AsyncInnDbContext context = new AsyncInnDbContext(options);

            AmenityManager amenities = new AmenityManager(context);

            RoomManager service = new RoomManager(context, amenities);

            _ = await service.CreateRoom(new RoomDto { Name = "Test One", Layout = "0" });
            _ = await service.CreateRoom(new RoomDto { Name = "Test Two", Layout = "1" });

            var roomsFromDb = context.Rooms.ToListAsync().Result;
            var dtos = await service.GetRooms();

            var expected = new List<string>();
            var actual = new List<string>();

            foreach (var item in roomsFromDb)
                expected.Add(item.Name);

            foreach (var item in dtos)
                actual.Add(item.Name);

            Assert.Equal(2, actual.Count);
            Assert.Equal(expected.Count, actual.Count);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async void CanUpdateRoomFromDatabase()
        {
            DbContextOptions<AsyncInnDbContext> options = new DbContextOptionsBuilder<AsyncInnDbContext>()
                .UseInMemoryDatabase("CanUpdateRoomFromDatabase")
                .Options;

            using AsyncInnDbContext context = new AsyncInnDbContext(options);

            AmenityManager amenities = new AmenityManager(context);

            RoomManager service = new RoomManager(context, amenities);

            _ = await service.CreateRoom(new RoomDto { Name = "Test One", Layout = "0" });

            await service.UpdateRoom(new RoomDto { Id = 1, Name = "Test Two", Layout = "1" });

            var actual = await service.GetRoom(1);

            Assert.Equal(1, await context.Rooms.CountAsync());
            Assert.NotEqual("Test One", actual.Name);
            Assert.NotEqual("Studio", actual.Layout);
            Assert.Equal("Test Two", actual.Name);
            Assert.Equal("OneBedroom", actual.Layout);
        }

        [Fact]
        public async void CanDeleteRoomFromDatabase()
        {
            DbContextOptions<AsyncInnDbContext> options = new DbContextOptionsBuilder<AsyncInnDbContext>()
                .UseInMemoryDatabase("CanDeleteRoomFromDatabase")
                .Options;

            using AsyncInnDbContext context = new AsyncInnDbContext(options);

            AmenityManager amenities = new AmenityManager(context);

            RoomManager service = new RoomManager(context, amenities);

            Assert.Equal(0, await context.Rooms.CountAsync());

            _ = await service.CreateRoom(new RoomDto { Name = "Test", Layout = "0" });

            Assert.Equal(1, await context.Rooms.CountAsync());

            await service.DeleteRoom(1);

            Assert.Equal(0, await context.Rooms.CountAsync());
        }

        [Fact]
        public async void CanAddAmenityToRoom()
        {
            DbContextOptions<AsyncInnDbContext> options = new DbContextOptionsBuilder<AsyncInnDbContext>()
                .UseInMemoryDatabase("CanAddAmenityToRoom")
                .Options;

            using AsyncInnDbContext context = new AsyncInnDbContext(options);

            AmenityManager amenities = new AmenityManager(context);

            RoomManager service = new RoomManager(context, amenities);

            var room = await service.CreateRoom(new RoomDto { Name = "Test", Layout = "0" });

            Assert.Null(room.Amenities);

            var amenity = await amenities.CreateAmenity(new AmenityDto() { Name = "Test Amenity" });

            var actual = await service.AddAmenityToRoom(room.Id, amenity.Id);

            Assert.NotEmpty(actual.Amenities);
            Assert.Single(actual.Amenities);
            Assert.Equal(amenity.Name, actual.Amenities.FirstOrDefault().Name);
        }

        [Fact]
        public async void CanDeleteAmenityToRoom()
        {
            DbContextOptions<AsyncInnDbContext> options = new DbContextOptionsBuilder<AsyncInnDbContext>()
                .UseInMemoryDatabase("CanDeleteAmenityToRoom")
                .Options;

            using AsyncInnDbContext context = new AsyncInnDbContext(options);

            AmenityManager amenities = new AmenityManager(context);

            RoomManager service = new RoomManager(context, amenities);

            var room = await service.CreateRoom(new RoomDto { Name = "Test", Layout = "0" });

            var amenity = await amenities.CreateAmenity(new AmenityDto() { Name = "Test Amenity" });

            var actual = await service.AddAmenityToRoom(room.Id, amenity.Id);

            Assert.NotEmpty(actual.Amenities);
            Assert.Single(actual.Amenities);
            Assert.Equal(amenity.Name, actual.Amenities.FirstOrDefault().Name);

            await service.RemoveAmentityFromRoom(room.Id, amenity.Id);

            Assert.Null(room.Amenities);
        }
    }
}
