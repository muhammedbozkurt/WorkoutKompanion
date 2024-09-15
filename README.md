# Workout and Exercise REST API

This project is a REST API for managing workouts and exercises. It supports full CRUD operations for workouts and exercises, allows filtering of workouts by various parameters, and handles user authentication. The API is built with .NET Core, uses MySQL as the database, and does not use Entity Framework. All data operations are handled using stored procedures.

Database link: https://we.tl/t-nUrV4AiKs5

## Features

1. **CRUD Operations** for workouts and exercises.
2. **Filtering** workouts by duration, difficulty, and region.
3. **Pagination** support for large datasets.
4. **Workout Details** along with associated exercises can be retrieved in a single call.
5. **Authentication** via JWT (JSON Web Token).
6. **Error Logging** and handling using Serilog.
7. **Secure Communication** via HTTPS.

## Technologies

- **.NET Core**
- **MySQL** (with stored procedures)
- **Serilog** (for logging)
- **JWT Authentication** (Bearer tokens)

## Table of Contents

- Installation
- Database Setup
- Running the Project
- API Endpoints
- Authentication
- Error Handling & Logging

## Installation

1. Clone the repository: git clone https://github.com/your-username/your-repo.git, then navigate to the folder: cd your-repo.

2. Install required dependencies: dotnet restore.

3. Set up your `appsettings.json` with the required MySQL connection details and JWT settings. Example:

   {
     "ConnectionStrings": {
       "MySQLConnection": "Server=localhost;Database=workoutdb;User=root;Password=yourpassword;"
     },
     "Jwt": {
       "Key": "your-jwt-secret-key",
       "Issuer": "your-app",
       "Audience": "your-app-users"
     }
   }

## Database Setup

### 1. Exported Database

The database schema and sample data are included in the repository as a SQL file:

- SQL File: db/workoutdb_export.sql

To import the database into MySQL, use the following command: mysql -u root -p workoutdb < db/workoutdb_export.sql.

### 2. MySQL Stored Procedures

Ensure that you have all the necessary stored procedures created in your MySQL instance. The project relies heavily on stored procedures for all database operations. The following stored procedures are used:

- sp_GetWorkoutById
- sp_GetExerciseById
- sp_GetFilteredWorkoutsWithPagination
- sp_GetExercisesWithPagination
- sp_InsertWorkout
- sp_InsertExercise
- sp_UpdateWorkout
- sp_UpdateExercise
- sp_DeleteWorkout
- sp_DeleteExercise

## Running the Project

1. Build and run the project: dotnet build, then dotnet run.

2. Access the API documentation at: https://localhost:5001/swagger.

3. API requests can be tested using tools like Postman or via the Swagger UI provided by the project.

## API Endpoints

### Workout Endpoints

- GET /api/workout - Get all workouts
- GET /api/workout/filter - Get filtered workouts (with pagination)
- GET /api/workout/{id}/details - Get workout details with exercises
- POST /api/workout - Add new workout
- PUT /api/workout/{id} - Update existing workout
- DELETE /api/workout/{id} - Soft delete workout

### Exercise Endpoints

- GET /api/exercise/{id} - Get exercise by ID
- POST /api/exercise - Add new exercise
- PUT /api/exercise/{id} - Update existing exercise
- DELETE /api/exercise/{id} - Soft delete exercise

### Query Parameters for Filtering Workouts

- duration: The workout duration in minutes (optional)
- difficulty: The workout difficulty (easy, medium, hard) (optional)
- region: The target body region (upper-body, lower-body, core, full-body) (optional)
- pageNumber: The page number for pagination (default: 1)
- pageSize: The size of the page for pagination (default: 10)

Example: GET /api/workout/filter?duration=30&difficulty=medium&region=core&pageNumber=1&pageSize=10.

## Authentication

The API uses JWT Bearer Authentication. You need to provide a valid token in the Authorization header for protected endpoints.

### Example

Authorization: Bearer your-jwt-token.

## Error Handling & Logging

Errors are logged using Serilog and caught globally by the ExceptionHandlingMiddleware. All exceptions are logged and returned to the client in a structured JSON format.

### Sample Error Response

{
  "error": "An unexpected error occurred."
}

## License

This project is licensed under the MIT License.
