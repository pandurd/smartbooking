# Running in local
Run via Docker command, Self hosted in Visual studio 2022 <br />

To start via Docker, run follwing command in root folder <br />
```
docker build . --tag smartbooking
docker run --env ASPNETCORE_ENVIRONMENT=docker --env ACESSTOKEN={ReplaceTokenHere}  -d -p 5050:80 smartbooking:latest 
```
App URL : http://localhost:5050

* Please reload once after main page loads

Swagger Docker URL : http://localhost:5050/swagger/index.html   

# High Level Design

![name-of-you-image](https://github.com/pandurd/smartbooking/blob/bb6a7c4c1bb6bec0a655aaaad7801594b025ab36/CurrentDesign.jpg)

# Scalable Design proposed

1. Reduce api cost if it incurs
2. Have centralized caching, if Movies list and Movie price from same provider is same for a particualr time (does not change in realtime)

Caching is currently done via inbuilt memory cache

![name-of-you-image](https://github.com/pandurd/smartbooking/blob/bb6a7c4c1bb6bec0a655aaaad7801594b025ab36/ScalableDesign.jpg)

# Screenshots
# Main Page
![name-of-you-image](https://github.com/pandurd/smartbooking/blob/974ca41dfcadcba08fd19cf40d8a74dd927b47aa/MainPage.jpg)

# Movie Page
![name-of-you-image](https://github.com/pandurd/smartbooking/blob/974ca41dfcadcba08fd19cf40d8a74dd927b47aa/MoviePage.jpg)


# Dev

* Used polly to retry,  should be configurable to reduce client wait time
* Used in Memory cache to cache
* Used hooks to cover functionalities needed in react. It would be still great to use redux.
* Authentication/Authorizations not covered
* if need another provider, only configuration need to be changed (MovieProvider, URL configs)
* Tests are not covered
* Not all devices are not testsed. For now optimised for Desktop site with 100%. Need some more changes for mobile/ipad
* Left nav deafault to main page

# Other Assumptions

* Only showing the result for cheapest is needed, but button is not imeplemented
* Price remains till user click buy and purchase
* Also show other avaialble prices with provider for same movie (For comparisons)
* Paginations is not required - would be great to have infinite scroll
* All service provider give similar API response with minimal change - using same model all api repsonses. should be configurebale






