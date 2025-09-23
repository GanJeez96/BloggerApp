# Prerequisites
- [.NET 9.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/)
- IDE (Rider/Visual Studio)

# Running the application
## Run using Docker
1. From the root project folder, run the following command to start the docker containers
```
docker compose up -d
```
- The following services shall run in docker
    - `blogger-app-mysql` -> database container
    - `blogger-app-flyway` -> runs migrations automatically on mysql
    - `blogger-app-api` -> runs the Web API
2. Navigate to `https://localhost:8080/swagger/index.html` to view the Swagger UI.
3. Use the API key specified in `appsettings.Development.json` to send requests.

To stop the containers, run the following command from the root project folder
```
docker compose down -v
```

## Run Locally
1. Clone the repository.
2. From the root project folder run
```
docker compose up -d
```

2. Navigate to `/WebApi` and run the application using the following command
```
dotnet run
```
3. The `<port>` will be displayed on the terminal when you run the application or it can also be found in `launchSettings.json` under `Profiles:Http:applicationUrl`.
4. Navigate to `https://localhost:<port>/swagger/index.html` to view the Swagger UI.
5. Use the API key specified in `appsettings.Development.json` to send requests.

## Running Tests
1. Navigate to the root folder `/BloggerApp`
2. Run the following command to run all the tests (Unit tests & Integration tests)
```
dotnet test
```