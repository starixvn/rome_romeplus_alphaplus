# CashService API

A .NET 9 Minimal API for handling cash-related operations.

## Technologies Used

-   .NET 9
-   ASP.NET Core Minimal API
-   Entity Framework Core
-   SQL Server
-   Swagger/OpenAPI
-   Serilog
-   FluentValidation

## Prerequisites

-   .NET 9 SDK
-   SQL Server (LocalDB or higher)
-   Visual Studio 2022 or VS Code

## Getting Started

1. Clone the repository
2. Navigate to the project directory
3. Run the following commands:

```bash
dotnet restore
dotnet build
dotnet run
```

The API will be available at:

-   API: http://localhost:5000
-   Swagger UI: http://localhost:5000/index.html

## Project Structure

```
CashService.API/
├── Configuration/     # Configuration classes
├── Endpoints/         # API endpoint handlers
├── Extensions/        # Extension methods
├── Models/           # Domain models and DTOs
├── Services/         # Business logic services
├── Program.cs        # Application entry point
└── appsettings.json  # Application settings
```

## API Documentation

The API documentation is available through Swagger UI when running in development mode. Access it at http://localhost:5000/index.html

## Environment Variables

The following environment variables can be set in appsettings.json or through environment variables:

-   `ConnectionStrings:DefaultConnection`: Database connection string
-   `Cors:AllowedOrigins`: Allowed CORS origins
-   `ApiSettings:Version`: API version

## Health Check

The API includes a health check endpoint at `/health` that returns the current status of the service.

## Logging

Logging is configured using Serilog and writes to both console and file outputs. In development mode, detailed logging is enabled.

## Contributing

1. Fork the repository
2. Create a feature branch
3. Commit your changes
4. Push to the branch
5. Create a Pull Request

## License

This project is licensed under the MIT License - see the LICENSE file for details.
