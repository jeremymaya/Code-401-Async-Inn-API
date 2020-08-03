# Code-401-Async-Inn-API

![Actions Status](https://github.com/jeremymaya/Code-401-Async-Inn-API/workflows/build/badge.svg)

## Async Inn API

Lab 12 - Intro to Entity Framework Core and APIs

Author: Kyungrae Kim

----

## Description

This is a RESTful API server built using ASP.NET Core to allow Async Hotel mangement to better manage the assets in their hotels. This application can modify and manage rooms, amenities, and new hotel locations. The data entered by the user will persist across a relational database and maintain its integrity as changes are made to the system.

----

## Getting Started

Clone this repository to your local machine.

```bash
git clone https://github.com/jeremymaya/Code-401-Async-Inn-API.git
```

## To run the program from Visual Studio:

Select ```File``` -> ```Open``` -> ```Code-401-Async-Inn-API```

Next navigate to the location you cloned the Repository.

Double click on the ```AsyncInnAPI``` directory.

Then select and open ```AsyncInnAPI.sln```

----

## Entity Relationship Diagram

![Entity Relationship Diagram](Assets/ERD.png)

* Hotel table has one to many relationship with HotelRoom table
* Room table has one to many relationship with HotelRoom table
* Amenities table has one to many relationship with RoomAmenities table
* HotelRoom table is a joint table with a payload
* RoomAmenities table is a pure join table
* Layout is an enum

Diagram Credit: [Amanda Iverson](https://github.com/Aiverson1011)

----

## Credits

* [Github - Amanda Iverson](https://github.com/Aiverson1011)
* [ASP.NET Core Development with macOS](https://gist.github.com/jeremymaya/a36c1de8220d76beca85a2804a2cecc4)

----

## Change Log

1.0: *Lab 12 Completed* - 29 Jul 2020
