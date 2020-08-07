# Code-401-Async-Inn-API

![Actions Status](https://github.com/jeremymaya/Code-401-Async-Inn-API/workflows/build/badge.svg)

## Async Inn API

* Lab 18 -  Roles
* Lab 17 -  Identity
* Lab 14 -  Navigation Properties & Routing
* Lab 13 -  Dependency Injection
* Lab 12 - Intro to Entity Framework Core and APIs

Author: Kyungrae Kim

----

## Description

This is a RESTful API server built using ASP.NET Core to allow Async Hotel mangement to better manage the assets in their hotels. This application can modify and manage rooms, amenities, and new hotel locations. The data entered by the user will persist across a relational database and maintain its integrity as changes are made to the system.

The Below is link to the MVC web application version of the assignment:

<https://github.com/jeremymaya/Code-401-Async-Inn>

----

## Getting Started

Clone this repository to your local machine.

```bash
git clone https://github.com/jeremymaya/Code-401-Async-Inn-API.git
```

## To run the program from Visual Studio

Select ```File``` -> ```Open``` -> ```Code-401-Async-Inn-API```

Next navigate to the location you cloned the Repository.

Double click on the ```AsyncInnAPI``` directory.

Then select and open ```AsyncInnAPI.sln```

----

## Entity Relationship Diagram

![Entity Relationship Diagram](Assets/ERD.png)

Diagram Credit: [Amanda Iverson](https://github.com/Aiverson1011)

* Hotel table has one to many relationship with HotelRoom table
* Room table has one to many relationship with HotelRoom table
* Amenities table has one to many relationship with RoomAmenities table
* HotelRoom table is a joint table with a payload
* RoomAmenities table is a pure join table
* Layout is an enum

----

## Features

* RESTful API
* Entity Framework Core
* Dependency Injection
* Data Transfer Objects
* Idenity
* Authorization
* JSON Web Token (JWT)

----

## Endpoints

### Account

| Method | EndPoint | Description |
|:-|:-|:-|
| POST | ```/api/Account/Register``` | |
| POST | ```/api/Account/Assign/Role``` | |
| POST | ```/api/Account/Login``` | |

 ```json
Sample Request Body of POST /api/Account/Register

{
    "email": "user@example.com",
    "password": "string",
    "firstName": "string",
    "lastName": "string",
    "role": "string"
}
```

### Amenities

| Method | EndPoint | Description |
|:-|:-|:-|
| GET | ```/api/Amenities``` | |
| POST | ```/api/Amenities``` | |
| GET | ```/api/Amenities/{id}``` | |
| PUT | ```/api/Amenities/{id}``` | |
| DELTE | ```/api/Amenities/{id}``` | |

```json
Sample Response of GET /api/Amenities

[
    {
        "id": 0,
        "name": "string"
    }
]
```

### HotelRooms

| Method | EndPoint | Description |
|:-|:-|:-|
| GET | ```/api/Hotels/{hotelId}/Rooms``` | |
| POST | ```/api/Hotels/{hotelId}/Rooms``` | |
| GET | ```/api/Hotels/{hotelId}/Rooms/{roomNumber}``` | |
| PUT | ```/api/Hotels/{hotelId}/Rooms/{roomNumber}``` | |
| DELTE | ```/api/Hotels/{hotelId}/Rooms/{roomNumber}``` | |

```json
Sample Response of GET /api/Hotels/{hotelId}/Rooms

[
    {
        "hotelId": 0,
        "roomNumber": 0,
        "rate": 0,
        "petFriendly": true,
        "roomId": 0,
        "room": {
            "id": 0,
            "name": "string",
            "layout": "string",
            "amenities": [
                {
                    "id": 0,
                    "name": "string"
                }
            ]
        }
    }
]
```

### Hotels

| Method | EndPoint | Description |
|:-|:-|:-|
| GET | ```/api/Hotels``` | |
| POST | ```/api/Hotels``` |
| GET | ```/api/Hotels/{id}``` |
| PUT | ```/api/Hotels/{id}``` |
| DELTE | ```/api/Hotels/{id}``` |

```json
Sample Response of GET /api/Hotels

[
    {
        "id": 0,
        "name": "string",
        "streetAddress": "string",
        "city": "string",
        "state": "string",
        "phone": "string",
        "rooms": [
            {
                "hotelId": 0,
                "roomNumber": 0,
                "rate": 0,
                "petFriendly": true,
                "roomId": 0,
                "room": {
                    "id": 0,
                    "name": "string",
                    "layout": "string",
                    "amenities": [
                        {
                            "id": 0,
                            "name": "string"
                        }
                    ]
                }
            }
        ]
    }
]
```

### Rooms

| Method | EndPoint | Description |
|:-|:-|:-|
| GET | ```/api/Rooms/``` | |
| POST | ```/api/Rooms/``` | |
| GET | ```/api/Rooms/{id}``` | |
| PUT | ```/api/Rooms/{id}``` | |
| DELETE | ```/api/Rooms/{id}``` | |
| POST | ```/api/Rooms/{roomId}/Amenity/{amenityId}``` | |
| DELTE | ```/api/Rooms/{roomId}/Amenity/{amenityId}``` | |

```json
Sample Response of GET /api/Rooms

[
    {
        "id": 0,
        "name": "string",
        "layout": "string",
        "amenities": [
            {
                "id": 0,
                "name": "string"
            }
        ]
    }
]
```

----

## Using Postgres Instead of SQL Server on Docker

Go to [ASP.NET Core Development with macOS](https://gist.github.com/jeremymaya/a36c1de8220d76beca85a2804a2cecc4) for more details.

If you have not already, download the [Docker Desktop for Mac](https://hub.docker.com/editions/community/docker-ce-desktop-mac?tab=description).

1. Download Postgres with the following command:

    ```bash
    docker pull posgtres
    ```

2. Launch the Docker Image with the following command:

    ```bash
    docker run --name psql -e POSTGRES_USER=sa -e POSTGRES_PASSWORD=ReallyStrongPassowrd1234! -p 5432:5432 -d postgres
    ```

3. Update the connection string with the following:

    ```json
    "ConnectionStrings": {
        "DefaultConnection": "Host=localhost;Database=DbName;Username=sa;Password=ReallyStrongPassword1234!"
    }
    ```

4. Add a dependency on ```Npgsql.EntityFrameworkCore.PostgreSQL```

----

## Credits

* [Github - Amanda Iverson](https://github.com/Aiverson1011)
* [ASP.NET Core Development with macOS](https://gist.github.com/jeremymaya/a36c1de8220d76beca85a2804a2cecc4)
* [Documenting API endpoints - Documentation](https://idratherbewriting.com/learnapidoc/docapis_finished_doc_result.html)
* [The Code Buzz - ASP.NET Core 3.1 – Use JWT Authorization Token in Swagger(OpenAPI)](https://thecodebuzz.com/jwt-authorization-token-swagger-open-api-asp-net-core-3-0/)
* [Docker Hub - postgres](https://hub.docker.com/_/postgres)
* [Hacker Noon - How To Install PostgreSQL With Docker Quickly](https://hackernoon.com/how-to-install-postgresql-with-docker-quickly-ki7g363m)
* [Npgsql Doc - Entity Framework Core](https://www.npgsql.org/efcore/index.html)
* [Code Maze - How to Configure PostgreSQL in Entity Framework Core](https://code-maze.com/configure-postgresql-ef-core/)
* [medium@RobertKhou - Getting Started with Entity Framework Core (PostgreSQL)](https://medium.com/@RobertKhou/getting-started-with-entity-framework-core-postgresql-c6fa09681624)

----

## Change Log

* 1.6: *Lab 18 Completed* - 5 Aug 2020
* 1.5: *Lab 17 Completed* - 3 Aug 2020
* 1.4: *Lab 16 Completed* - 2 Aug 2020
* 1.3: *Lab 14 Completed* - 30 Jul 2020
* 1.2: *Lab 13 Completed* - 30 Jul 2020
* 1.1: *Lab 12 Completed* - 29 Jul 2020
