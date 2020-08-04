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

* Hotel table has one to many relationship with HotelRoom table
* Room table has one to many relationship with HotelRoom table
* Amenities table has one to many relationship with RoomAmenities table
* HotelRoom table is a joint table with a payload
* RoomAmenities table is a pure join table
* Layout is an enum

Diagram Credit: [Amanda Iverson](https://github.com/Aiverson1011)

## Dependency Injection

Update your README to contain information about your architecture. Add a section that, in your own words, discusses what the architecture pattern is and how it is used in the app.

## Idenity

Update your README with description of what identity is.

## Endpoints

### GET ```hotels```

Gets the surf conditions for a specific beach ID.

#### Parameters

| Path parameter | Description |
|:-|:-|
| ```{hotelId}``` | The value for the hotel you want to look up. Database has been seeded with 5 hotels |

Sample request

```bash
curl -I -X GET "https://api.openweathermap.org/data/2.5/surfreport?zip=95050&appid=APIKEY&units=imperial&days=2"
```

#### Sample response

The following is a sample response from the ```hotels/{hotelId}``` endpoint:

```json
{
    "surfreport": [
        {
            "beach": "Santa Cruz",
            "monday": {
                "1pm": {
                    "tide": 5,
                    "wind": 15,
                    "watertemp": 80,
                    "surfheight": 5,
                    "recommendation": "Go surfing!"
                },
                "2pm": {
                    "tide": -1,
                    "wind": 1,
                    "watertemp": 50,
                    "surfheight": 3,
                    "recommendation": "Surfing conditions are okay, not great."
                },
                "3pm": {
                    "tide": -1,
                    "wind": 10,
                    "watertemp": 65,
                    "surfheight": 1,
                    "recommendation": "Not a good day for surfing."
                }
                ...
            }
        }
    ]
}
```

#### Response definitions

| Response item | Description | Data type |
|:-|:-|:-|
| hotel | The beach you selected based on the beach ID in the request. The beach name is the official name as described in the National Park Service Geodatabase. | String |

----

## Credits

* [Github - Amanda Iverson](https://github.com/Aiverson1011)
* [ASP.NET Core Development with macOS](https://gist.github.com/jeremymaya/a36c1de8220d76beca85a2804a2cecc4)
* [Documenting API endpoints - Documentation](https://idratherbewriting.com/learnapidoc/docapis_finished_doc_result.html)

----

## Change Log

1.0: *Lab 12 Completed* - 29 Jul 2020
