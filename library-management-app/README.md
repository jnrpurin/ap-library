# Library Management App

This project is a library management application developed using ASP.NET for the backend and React for the frontend. The application follows the MVC 4.0 standard and uses a clean architecture, applying SOLID principles.

## Project

### Backend

- **LibraryManagementApp.sln**: Visual Studio solution file that contains references to all backend projects.
- **Controllers**: Contains the application's controllers.
- **Models**: Contains the application's data models.
- **Repositories**: Contains the interfaces and implementations for data access.
- **Services**: Contains the interfaces and implementations for business logic.
- **Data**: Contains the database context configuration.
- **Migrations**: Contains the Entity Framework migration files.

### Frontend

- **public**: Frontend entry point, where React will be built.
- **src/components**: Contains the application components.
- **src/services**: Contains services for API calls.

## Used techs

- ASP.NET MVC 4.0
- React
- MySQL
- Entity Framework

## How to run

1. Clone the repository.
2. Configure the connection string in the `appsettings.json` file.
3. Run Entity Framework migrations to configure the database.
4. Start the backend and frontend.
