﻿// <auto-generated />
using AsyncInnAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace AsyncInnAPI.Migrations
{
    [DbContext(typeof(AsyncInnDbContext))]
    [Migration("20201011064344_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("AsyncInnAPI.Models.Amenity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Amenities");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Hair Dryer"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Water"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Extra Pillows"
                        },
                        new
                        {
                            Id = 4,
                            Name = "Extra Blankets"
                        },
                        new
                        {
                            Id = 5,
                            Name = "Wine"
                        });
                });

            modelBuilder.Entity("AsyncInnAPI.Models.Hotel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("City")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Phone")
                        .HasColumnType("text");

                    b.Property<string>("State")
                        .HasColumnType("text");

                    b.Property<string>("StreetAddress")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Hotels");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            City = "One",
                            Name = "Hotel One",
                            Phone = "111-111-1111",
                            State = "Number",
                            StreetAddress = "1111 One Street"
                        },
                        new
                        {
                            Id = 2,
                            City = "Two",
                            Name = "Hotel Two",
                            Phone = "222-222-2222",
                            State = "Number",
                            StreetAddress = "2222 Two Street"
                        },
                        new
                        {
                            Id = 3,
                            City = "Three",
                            Name = "Hotel Three",
                            Phone = "333-333-3333",
                            State = "Number",
                            StreetAddress = "3333 Three Street"
                        },
                        new
                        {
                            Id = 4,
                            City = "Four",
                            Name = "Hotel Four",
                            Phone = "206-681-4444",
                            State = "Number",
                            StreetAddress = "4444 Four Street"
                        },
                        new
                        {
                            Id = 5,
                            City = "Five",
                            Name = "Hotel Five",
                            Phone = "555-555-5555",
                            State = "Number",
                            StreetAddress = "5555 Five Street"
                        });
                });

            modelBuilder.Entity("AsyncInnAPI.Models.HotelRoom", b =>
                {
                    b.Property<int>("HotelId")
                        .HasColumnType("integer");

                    b.Property<int>("RoomNumber")
                        .HasColumnType("integer");

                    b.Property<bool>("PetFriendly")
                        .HasColumnType("boolean");

                    b.Property<decimal>("Rate")
                        .HasColumnType("money");

                    b.Property<int>("RoomId")
                        .HasColumnType("integer");

                    b.HasKey("HotelId", "RoomNumber");

                    b.HasIndex("RoomId");

                    b.ToTable("HotelRooms");

                    b.HasData(
                        new
                        {
                            HotelId = 1,
                            RoomNumber = 101,
                            PetFriendly = true,
                            Rate = 150m,
                            RoomId = 1
                        },
                        new
                        {
                            HotelId = 1,
                            RoomNumber = 201,
                            PetFriendly = false,
                            Rate = 100m,
                            RoomId = 2
                        },
                        new
                        {
                            HotelId = 2,
                            RoomNumber = 301,
                            PetFriendly = true,
                            Rate = 50m,
                            RoomId = 3
                        });
                });

            modelBuilder.Entity("AsyncInnAPI.Models.Room", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("Layout")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Rooms");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Layout = 1,
                            Name = "Room One"
                        },
                        new
                        {
                            Id = 2,
                            Layout = 0,
                            Name = "Room Two"
                        },
                        new
                        {
                            Id = 3,
                            Layout = 2,
                            Name = "Room Three"
                        },
                        new
                        {
                            Id = 4,
                            Layout = 0,
                            Name = "Room Four"
                        },
                        new
                        {
                            Id = 5,
                            Layout = 1,
                            Name = "Room Five"
                        },
                        new
                        {
                            Id = 6,
                            Layout = 2,
                            Name = "Room Six"
                        });
                });

            modelBuilder.Entity("AsyncInnAPI.Models.RoomAmenity", b =>
                {
                    b.Property<int>("RoomId")
                        .HasColumnType("integer");

                    b.Property<int>("AmenityId")
                        .HasColumnType("integer");

                    b.HasKey("RoomId", "AmenityId");

                    b.HasIndex("AmenityId");

                    b.ToTable("RoomAmenities");

                    b.HasData(
                        new
                        {
                            RoomId = 1,
                            AmenityId = 1
                        },
                        new
                        {
                            RoomId = 1,
                            AmenityId = 2
                        },
                        new
                        {
                            RoomId = 2,
                            AmenityId = 3
                        },
                        new
                        {
                            RoomId = 2,
                            AmenityId = 4
                        },
                        new
                        {
                            RoomId = 3,
                            AmenityId = 5
                        });
                });

            modelBuilder.Entity("AsyncInnAPI.Models.HotelRoom", b =>
                {
                    b.HasOne("AsyncInnAPI.Models.Hotel", "Hotel")
                        .WithMany("Rooms")
                        .HasForeignKey("HotelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AsyncInnAPI.Models.Room", "Room")
                        .WithMany("Rooms")
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("AsyncInnAPI.Models.RoomAmenity", b =>
                {
                    b.HasOne("AsyncInnAPI.Models.Amenity", "Amenity")
                        .WithMany("Amenities")
                        .HasForeignKey("AmenityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AsyncInnAPI.Models.Room", "Room")
                        .WithMany("Amenities")
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
