# 🍽️ Restaurant Management System

A robust **.NET web application** built with **Clean Architecture** principles to manage restaurant operations such as menu management, order processing, and administrative tasks.

---

## 🏗️ Architecture Overview

The solution is structured into four layers following Clean Architecture:

RestaurantManagementSystem/

├── RestaurantManagementSystem.Presentation/ # ASP.NET Core MVC Web App (UI Layer)

├── RestaurantManagementSystem.Application/ # Business Logic & Use Cases

├── RestaurantManagementSystem.Domain/ # Core Business Entities & Interfaces

└── RestaurantManagementSystem.Infrastructure/ # Data Access & External Services


This separation of concerns ensures scalability, maintainability, and testability.

---

## 🚀 Features

- **Menu Management**: Categorize menu items with pricing and preparation time.
- **Order Processing**: Manage orders with status tracking (Pending, Preparing, Ready, Delivered).
- **Order Types**: Supports Dine-In, Takeaway, and Delivery.
- **Admin Dashboard**: Intuitive web interface for managing all restaurant operations.

---

## 🛠️ Tech Stack

- **Framework**: ASP.NET Core MVC (v6+)
- **Database**: SQL Server (with EF Core)
- **Architecture**: Clean Architecture + Domain-Driven Design
- **Object Mapping**: Mapster
- **Design Patterns**: Repository Pattern, Unit of Work, Dependency Injection

---

## 📊 Core Database Entities

- **Categories** – Menu item classifications  
- **MenuItems** – Products with price and preparation time  
- **Orders** – Customer orders with status & type  
- **OrderItems** – Links orders with menu items  

---

## 🏃 Getting Started

### Prerequisites
- [.NET 6 SDK](https://dotnet.microsoft.com/download)
- [SQL Server](https://www.microsoft.com/sql-server) (LocalDB or full instance)
- Visual Studio 2022 / VS Code

### Installation Steps
1. **Clone the repository**
   ```bash
   git clone https://github.com/elkbany/RestaurantManagementSystem.git
   cd RestaurantManagementSystemRestore dependencies


2. **dotnet restore**
   ```bash
   dotnet restore
3. **Update database connection string**
Edit appsettings.json in the Presentation project with your SQL Server credentials.

4. Run database migrations
   ```bash
   dotnet ef database update --project RestaurantManagementSystem.Infrastructure --startup-project RestaurantManagementSystem.Presentation
🏛️ Project Layers & Dependencies
Project	Depends On	Responsibilities
Presentation	Application, Infrastructure	Web UI (Controllers, Views)
Application	Domain	Business logic, DTOs, Services
Domain	None	Entities, Enums, Interfaces
Infrastructure	Domain	EF Core, Repository Implementations

🔄 Data Flow
Controllers receive HTTP requests.

Application Services execute business rules.

Repositories interact with the database via EF Core.

Unit of Work ensures consistent data changes.

📝 Sample Data
For testing, the system seeds:

Categories: Appetizers, Main Courses, Desserts

Menu Items: Spring Rolls, Grilled Chicken, Chocolate Cake

Sample orders for quick demonstrations

🤝 Contributing
Contributions are welcome!

Fork the repository

Create a branch: git checkout -b feature/YourFeature

Commit your changes: git commit -m 'Add feature'

Push: git push origin feature/YourFeature

Open a Pull Request

📄 License
Licensed under the MIT License.

Notes:
This project showcases enterprise-grade architecture principles with a focus on modularity and maintainability.


