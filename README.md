# Challenge Tracker Application (v1.0 - Basic)

This repository tracks the development of the Challenge Tracker application. This `basic` branch represents the foundational version of the app, focusing on core CRUD (Create, Read, Update, Delete) functionality and fundamental ASP.NET Core and Entity Framework Core concepts.

## Features of the Basic App

This version of the application successfully implements:

- ✅ **Full CRUD Operations** for all primary entities:
  - Users
  - Challenges
  - Categories
  - User Profiles
- ✅ **Many-to-Many Relationship Management**: Users can be assigned to multiple challenges, and challenges can have multiple users.
- ✅ **Progress Logging**: Users can log progress updates against specific challenges they are assigned to.
- ✅ **Relational Data Display**: Views correctly load and display related data (e.g., showing a category name on the challenge page).

---

## Technology Stack

- **C# 12**
- **ASP.NET Core 8**
- **Entity Framework Core 8**
- **SQL Server**
- **MVC (Model-View-Controller) Pattern**

---

## Core Concepts Demonstrated

This project serves as a practical demonstration of the following key concepts.

### Entity Framework Core

* **Code-First Approach**: The database schema is defined and managed entirely through C# model classes.
* **Relationship Mapping**: All three primary relationship types have been successfully modeled:
    * **One-to-One**: `User` ↔ `Profile`
    * **One-to-Many**: `Category` → `Challenges`
    * **Many-to-Many**: `User` ↔ `Challenge` (via the `UserChallenge` join entity).
* **DbContext Configuration**: Use of a `DbContext` class (`ChallengesDbContext`) as the main gateway for all database interactions.
* **Asynchronous Database Operations**: Consistent use of `async`/`await` for non-blocking database calls (`ToListAsync`, `SaveChangesAsync`, etc.).
* **Eager Loading**: Strategic use of `.Include()` to load related data and prevent the N+1 query problem, ensuring efficient data retrieval.
* **Data Annotations**: Use of attributes like `[Key]`, `[Required]`, and `[StringLength]` to enforce schema rules and validation.

### ASP.NET Core MVC

* **MVC Pattern**: Clear separation of concerns between Models, Views, and Controllers.
* **RESTful Routing**: Standard implementation of controllers for handling HTTP GET and POST requests for CRUD operations.
* **Data Transfer**: Passing data from controllers to views using `ViewData` for `SelectLists` and strongly-typed models.
* **User Feedback**: Using `TempData` to display success or error messages to the user after completing an action.

---

## Database Schema

The following diagram illustrates the database structure for the basic application.


![A diagram of the challenge tracker schema](./.assets/challenges-schema.png)

---

## How to Run the Project

1.  Clone the repository and ensure you are on the `basic` branch.
2.  Update the `ConnectionString` in `appsettings.json` to point to your local SQL Server instance.
3.  Open a terminal or command prompt in the project's root directory.
4.  Run the Entity Framework Core migrations to create the database schema:
    ```bash
    dotnet ef database update
    ```
5.  Run the application:
    ```bash
    dotnet run
    ```
6.  Navigate to `https://localhost:XXXX` or `http://localhost:YYYY` in your web browser.