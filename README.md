💳 Local Payment Gateway API
This project is a local payment gateway system that allows users to deposit money via local payment providers such as Vodafone Cash, Etisalat Cash, We Cash, or Orange Cash.

🚀 Key Features
User Payment Flow:

The user selects a payment method.

The system displays the phone number to transfer to.

The user inputs the amount, their number, and uploads a payment screenshot.

The operation is sent to a Telegram Bot with full transfer details.

Admin can approve or reject the payment from Telegram, and the system updates the status accordingly.

Admin Capabilities:

Manage allowed domains (CORS) dynamically.

Update Telegram bot settings (Token, ChatId).

Add, update, or delete payment methods (Vodafone, Etisalat, etc.).

Secure APIs using JWT authentication.

Admin-only endpoints using role-based authorization.

API Design:

Built with ASP.NET Core 8 Web API.

Clean and well-structured architecture using:

✅ Clean Architecture Principles

✅ Unit of Work pattern

✅ Generic Repository

✅ DTOs for clean data transfer

✅ Separation of concerns and SOLID principles

Testing:

Includes unit tests using xUnit and FakeItEasy for mocking.

Configuration:

Settings managed through AppSettings.json and bound to a static AppSettings class.

JWT keys, DB connection, Telegram bot, and webhook URLs are all centralized in config.

🧱 Technologies Used
.NET 8 (ASP.NET Core)

Entity Framework Core

SQL Server

Telegram Bot API

JWT Authentication

xUnit & FakeItEasy

Clean Architecture

📂 Example API Endpoints
http
POST /api/user/login
POST /api/user/register
GET  /api/admin/transfers
PUT  /api/admin/transfers/{id}
GET  /api/user/transfers
POST /api/user/transfers
🛡 Security
JWT-based authentication with cookies.

Role-based authorization (Admin / User).

CORS support with admin-level control over allowed domain
