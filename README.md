# 🚗 Rent2Me — Car Rental System Backend

## Project Overview

The **Rent2Me API** is an **ASP.NET Core 5 Web API** that powers the Car Rental System. It manages all business logic, database operations, authentication, contract workflows, rental processes, and real-time communication for admins, car owners, and renters, following clean architecture principles and industry best practices.

---

## Features

- **User Management**  
  - Registration and secure authentication using JWT tokens.  
  - Role-based access control (Admin, Car Owner, Renter).

- **Car Management**  
  - CRUD operations for car listings (Add, Update, Delete, Display).  
  - Property deed and image handling per vehicle.

- **Subscription System**  
  - Users subscribe to plans with process limits.  
  - Dynamic association of subscription plans to users.

- **Rental Workflow**  
  - Renters submit rental requests; owners approve or reject.  
  - Rental contracts are generated as image files for signing.  
  - Secure upload/download of signed contracts.

- **Feedback & Licensing**  
  - Feedback submission and retrieval endpoints.  
  - Driving license retrieval per customer.

- **Search & Filtering**  
  - Dynamic search for cars based on criteria like model, brand, price, and availability.

- **Exception Handling & Validation**  
  - Global exception error handling.

- **Real-Time Notifications**  
  - \SignalR integration for instant updates on rental status.

- **Logging & Documentation**  
  - Structured logging with Serilog.  
  - Interactive API documentation via Swagger (OpenAPI).

---

## Technologies Used

- **Backend Framework**: ASP.NET Web API  
- **ORM**: Entity Framework Core  
- **Database**: Microsoft SQL Server  
- **Authentication**: JWT Bearer Tokens  
- **Documentation & Testing**: Swagger UI, Postman  
- **Real-Time**: SignalR

---

## Setup Instructions

1. **Clone the repository**
   ```bash
   git clone https://github.com/your-username/crs-pis-backend.git
   ```

2. **Configure Database**  
   - Create a new SQL Server database (e.g., `CarRentalSystemDB`).  
   - Execute the provided SQL scripts or run EF Core migrations:
     ```bash
     dotnet ef database update
     ```

3. **Configure `appsettings.json`**  
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=YOUR_SERVER;Database=CarRentalSystemDB;Trusted_Connection=True;"
   }
   ```

4. **Run the API**
   ```bash
   dotnet run --project ./API/Crspis.Backend.csproj
   ```

5. **Access Swagger UI**
   Navigate to: `https://localhost:5001/swagger`

---

## API Endpoints

Below is a list of all available backend endpoints in the CRS‑PIS Web API:

```
GET    /api/Car/DisplayAllCars
GET    /api/Car/{licensePlate}/propertyDeed
GET    /api/Car/{licensePlate}/image
PUT    /api/Car/{licensePlate}/updateCarPropertyDeed
PUT    /api/Car/{licensePlate}/updateCarImage
PUT    /api/Car/update
GET    /api/Car/search
DELETE /api/Car/RemoveCar
POST   /api/Car/AddCar
GET    /api/Car/DisplayCar

GET    /api/Contract/Admin
GET    /api/Contract/SystemContract
GET    /api/Contract/renter_contract
GET    /api/Contract/{requestId}/contract

POST   /api/Feedback/submit
GET    /api/Feedback/receivedFeedbacks

GET    /api/GetDrivingLicense/{customerId}/driving_license

POST   /api/Login/login
POST   /api/Login/ForgetPassword
POST   /api/Login/reset
POST   /api/Mail/send/Notification/{userId}/Notification/mark-as-read
POST   /api/Register/register

POST   /api/RentalRequest/request
POST   /api/RentalRequest/accept
POST   /api/RentalRequest/reject

POST   /api/Subscription/Add
DELETE /api/Subscription/delete
GET    /api/Subscription/subscription-plans
POST   /api/Subscription/subscribe

GET    /api/UserProfile/{nationalID}
GET    /api/UserProfile/{customerId}/image
GET    /api/UserProfile/{customerId}/drivingLicense
GET    /api/UserProfile
PUT    /api/UserProfile/DisplayUserRequests/update
PUT    /api/UserProfile/{customerId}/updateProfileImage
PUT    /api/UserProfile/{customerId}/updateDrivingLicense
```
