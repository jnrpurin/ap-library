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

Follow these simple steps to make the project work on your computer:

1. **Download the project**  
   Go to the GitHub page and click on **Code → Download ZIP**, then unzip the file somewhere on your computer.  
   (If you know how to use Git, you can also run:  
   `git clone https://github.com/jnrpurin/ap-library.git`)

2. **Open the backend**  
   Open the folder called `backend` in **Visual Studio 2022** (or a newer version).  
   You’ll see a file named `LibraryManagementApp.sln` — double-click it to open the solution.

3. **Set up the database**  
   The system uses **MySQL** to store book and user data.  
   Open the file `appsettings.json` and change the `"ConnectionStrings"` section to match your MySQL user, password, and database name.  
   Example:  
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=localhost;Database=LibraryDB;User=root;Password=1234;"
   }
   ```

4. **Apply migrations**  
   Inside Visual Studio (or VS Code), open the Package Manager Console and run this command:
   ```
   update-database
   ```

   This will create the tables automatically in your database.

5. **Run the backend**  
   Click the Run button in Visual Studio to start the backend API. Into VS code you can go via teminal window and type ´dotnet run´
   You should see messages like "Application started. Listening on..." — that means it’s running!

6. **Run the frontend**  
   Open a new terminal (Command Prompt or PowerShell), go to the folder frontend, and type:
   ```
   npm install
   npm start
   ```

   This will download everything the frontend needs and open the website in your browser (usually at http://localhost:3000).

7. **Test it out!**  
   Try logging in, adding books, and managing loans — your library system should be up and running!


## Next Steps

- The next improvements planned for this project include adding monitoring and observability through application logs.
- Implementing integration tests for the repository layer, and creating middleware tests focused on authentication and JWT validation.  
- Additionally, unit tests will be added for helpers and extensions, along with frontend test coverage.  
- To enhance performance and scalability, caching with Redis will be implemented, and frontend pagination will be introduced to optimize data handling and user experience.