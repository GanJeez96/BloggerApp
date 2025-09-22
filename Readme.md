## Prerequisites
- [.NET 9.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/)
- IDE (Rider/Visual Studio)

## Running the application
1. Start MySQL & Flyway (using Docker Compose)
```
docker compose up -d
```
- The following services shall run in docker
  - `mysql` -> databse container
  - `flyway` -> runs migrations automatically on mysql
2. Navigate to `\webapi` and run the application using the following command
```
dotnet run
```
Use the API key specified in `appsettings.Development.json` to send requests.

## Running Tests
1. Navigate to the root folder `\BloggerApp`
2. Run the following command to run all the tests (Unit tests & Integration tests)
```
dotnet test
```