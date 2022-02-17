# Running in local
Run via Docker command, Self hosted in Visual studio 2022 <br />

To start via Docker, run follwing command in root folder <br />
```
docker build . --tag smartbooking
docker run --env ASPNETCORE_ENVIRONMENT=docker --env ACESSTOKEN={ReplaceTokenHere}  -d -p 5050:80 smartbooking:latest 
```
App URL : http://localhost:5050
Swagger Docker URL : http://localhost:5050/swagger/index.html   


