using System;
using System.Collections.Generic;
using AsyncInnAPI.Data;
using AsyncInnAPI.Models;
using AsyncInnAPI.Models.Dtos;
using AsyncInnAPI.Models.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Tests
{
    public class AmenityManagerTest
    {
        [Fact]
        public async void CanCreateAmenityAndSaveToDatabase()
        {
            DbContextOptions<AsyncInnDbContext> options = new DbContextOptionsBuilder<AsyncInnDbContext>()
                .UseInMemoryDatabase("CanCreateAmenityAndSaveToDatabase")
                .Options;

            using AsyncInnDbContext context = new AsyncInnDbContext(options);

            AmenityManager service = new AmenityManager(context);

            AmenityDto dto = new AmenityDto() { Name = "Test" };

            Assert.Equal(0, context.Amenities.CountAsync().Result);

            var result = await service.CreateAmenity(dto);

            var actual = context.Amenities.FindAsync(result.Id).Result;

            Assert.Equal(1, await context.Amenities.CountAsync());
            Assert.IsType<Amenity>(actual);
            Assert.Equal(1, actual.Id);
            Assert.Equal("Test", actual.Name);
        }

        [Fact]
        public async void CanCreateAmenityAndSaveMultipleToDatabase()
        {
            DbContextOptions<AsyncInnDbContext> options = new DbContextOptionsBuilder<AsyncInnDbContext>()
                .UseInMemoryDatabase("CanCreateAmenityAndSaveMultipleToDatabase")
                .Options;

            using AsyncInnDbContext context = new AsyncInnDbContext(options);

            AmenityManager service = new AmenityManager(context);

            var resultOne = await service.CreateAmenity(new AmenityDto() { Name = "Test One" });
            var resultTwo = await service.CreateAmenity(new AmenityDto() { Name = "Test Two" });

            var actualOne = await context.Amenities.FindAsync(resultOne.Id);
            var actualTwo = await context.Amenities.FindAsync(resultTwo.Id);

            Assert.Equal(2, await context.Amenities.CountAsync());
            Assert.IsType<Amenity>(actualOne);
            Assert.Equal(1, actualOne.Id);
            Assert.Equal("Test One", actualOne.Name);
            Assert.IsType<Amenity>(actualTwo);
            Assert.Equal(2, actualTwo.Id);
            Assert.Equal("Test Two", actualTwo.Name);
        }

        [Fact]
        public async void CreateAmenityReturnsAmenityDtoWithId()
        {
            DbContextOptions<AsyncInnDbContext> options = new DbContextOptionsBuilder<AsyncInnDbContext>()
                .UseInMemoryDatabase("CreateAmenityReturnsAmenityDtoWithId")
                .Options;

            using AsyncInnDbContext context = new AsyncInnDbContext(options);

            AmenityManager service = new AmenityManager(context);

            var actual = await service.CreateAmenity(new AmenityDto() { Name = "Test" });

            Assert.IsType<AmenityDto>(actual);
            Assert.Equal(1, actual.Id);
            Assert.Equal("Test", actual.Name);
        }

        [Fact]
        public async void CanGetAmenityFromDatabase()
        {
            DbContextOptions<AsyncInnDbContext> options = new DbContextOptionsBuilder<AsyncInnDbContext>()
                .UseInMemoryDatabase("CanGetAmenityFromDatabase")
                .Options;

            using AsyncInnDbContext context = new AsyncInnDbContext(options);

            AmenityManager service = new AmenityManager(context);

            var result = await service.CreateAmenity(new AmenityDto() { Name = "Test" });

            var actual = await service.GetAmenity(result.Id);

            Assert.IsType<AmenityDto>(actual);
            Assert.Equal(1, actual.Id);
            Assert.Equal("Test", actual.Name);
        }

        [Fact]
        public async void CanGetAmenitiesFromDatabase()
        {
            DbContextOptions<AsyncInnDbContext> options = new DbContextOptionsBuilder<AsyncInnDbContext>()
                .UseInMemoryDatabase("CanGetAmenitiesFromDatabase")
                .Options;

            using AsyncInnDbContext context = new AsyncInnDbContext(options);

            AmenityManager service = new AmenityManager(context);

            _ = await service.CreateAmenity(new AmenityDto() { Name = "Test One" });
            _ = await service.CreateAmenity(new AmenityDto() { Name = "Test Two" });

            var amenitiesFromDb = context.Amenities.ToListAsync().Result;
            var dtos = await service.GetAmenities();

            var expected = new List<string>();
            var actual = new List<string>();

            foreach (var item in amenitiesFromDb)
                expected.Add(item.Name);

            foreach (var item in dtos)
                actual.Add(item.Name);

            Assert.Equal(2, actual.Count);
            Assert.Equal(expected.Count, actual.Count);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async void CanUpdateAmenityFromDatabase()
        {
            DbContextOptions<AsyncInnDbContext> options = new DbContextOptionsBuilder<AsyncInnDbContext>()
                .UseInMemoryDatabase("CanUpdateAmenityFromDatabase")
                .Options;

            using AsyncInnDbContext context = new AsyncInnDbContext(options);

            AmenityManager service = new AmenityManager(context);

            _ = await service.CreateAmenity(new AmenityDto() { Name = "Test One" });

            await service.UpdateAmenity(new AmenityDto() { Id = 1, Name = "Test Two" });

            var actual = await service.GetAmenity(1);

            Assert.Equal(1, await context.Amenities.CountAsync());
            Assert.NotEqual("Test One", actual.Name);
            Assert.Equal("Test Two", actual.Name);
        }

        [Fact]
        public async void CanDeleteAmenityFromDatabase()
        {
            DbContextOptions<AsyncInnDbContext> options = new DbContextOptionsBuilder<AsyncInnDbContext>()
                .UseInMemoryDatabase("CanDeleteAmenityFromDatabase")
                .Options;

            using AsyncInnDbContext context = new AsyncInnDbContext(options);

            AmenityManager service = new AmenityManager(context);

            _ = await service.CreateAmenity(new AmenityDto() { Name = "Test" });

            Assert.Equal(1, await context.Amenities.CountAsync());

            await service.DeleteAmenity(1);

            Assert.Equal(0, await context.Amenities.CountAsync());
        }
    }
}
