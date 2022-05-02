## Environment:  
- .NET version: 3.0

## Read-Only Files:   
- MovieService.Tests/IntegrationTests.cs

## Data:  
Example of a movie data JSON object:
```
{
    id: 1,
    title: "Friends",
    category: "Comedy",
    releaseDate: 1573843210
}     
```

## Requirements:

A company is launching an API service that can manage movies. The service should be a web API layer using .NET Core 3.0. You already have a prepared infrastructure, and Web API Controller MoviesController is already implemented. You need to think about the localization of the movie's data in response. The language to be used is retrieved from the request header's "Accept-Language" value.

The following API calls are already implemented:
1. Creating movies: a POST request to the endpoint api/movies adds a movie to the database. The HTTP response code is 200.
2. Getting movie by id: a GET request to the endpoint api/movies/{id} returns the details of the movie having {id} as a unique identifier. If there is no movie with {id}, response code 404 is returned. On success, the response code is 200.

Change the `SetLanguageMiddleware.cs` and `CustomStringLocalizer.cs` files in the following way:
- Implement the localization mechanism for the movie's "category" property by using the language set in the request header's "Accept-Language" value.
- The service should support 3 languages: "en"- English, "ru"- Russian, and "it"- Italian. Localized (translated) strings are given in the file CustomStringLocalizer.cs. Please use the .NET Core IStringLocalizer as a localizer to find the localized string.
- If nothing is set in the request header's "Accept-Language" value, then use "en" language by default.
- Example: If the request header's "Accept-Language" value is "it" (Italian), then the response should return the movie's "category" value with an Italian translation.


Definition of Movie model:
+ id - The ID of the movie. [INTEGER]
+ title - The title of the movie. [STRING]
+ category - The category to which movie belongs to. [STRING]
+ releaseDate - The date when the movie was released in UTC (GMT + 0). [EPOCH STRING]


## Example requests and responses with headers

**Request 1:**

GET request to api/movies/1 without an "Accept-Language" header.

The response code will be 200 with the movie's details. The category value is returned in English because there is no "Accept-Language" header and English is the default language.
```
{
    "id": 1,
    "title": "Friends",
    "category": "Comedy",
    "releaseDate": 1573843210
}   
```

**Request 2:**

GET request to api/movies/1 with the "Accept-Language" header set to "it".

The response code will be 200 with the movie's details. The category value is returned in Italian language because the "Accept-Language" header set to "it".
```
{
    "id": 1,
    "title": "Friends",
    "category": "Сommedia",
    "releaseDate": 1573843210
}    
```
