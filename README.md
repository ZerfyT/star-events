# Star Events - Online Ticketing System

Star Events is a feature-rich, full-stack ASP.NET Core MVC web application designed for online event ticketing in Sri Lanka. It provides a seamless platform for customers to discover and book tickets for events, for organizers to manage their offerings, and for administrators to oversee the entire system.

The application is built using repository pattern to ensuring separation of concerns and maintainability. It uses MySQL for data storage and integrates with Stripe for secure online payments.

## ✨ Key Features

### For Customers

- User Authentication: Secure registration and login.
- Profile Management: View and update personal details and see booking history.
- Event Discovery: Browse, search, and filter events by category, date, or location.
- Secure Payments: Integrated with Stripe for reliable payment processing.
- E-Ticketing: Receive unique, QR-coded e-tickets upon successful purchase.
- Promotions: Apply discount codes for special offers.

### For Event Organizers

- Event Management: Full CRUD (Create, Read, Update, Delete) functionality for events.
- Real-time Sales Tracking: Monitor ticket sales and revenue through a dedicated dashboard.
- Ticket Management: Define ticket types (e.g., VIP, General) and set pricing and availability.

### For Administrators

- User Management: Full CRUD operations for all users and their roles (Admin, Organizer, Customer).
- System-wide Management: Manage core entities like Venues and Categories.
- Promotions & Discounts: Create and manage promotional codes.
- Comprehensive Reporting: Generate system-wide reports on sales, user activity, and events.

## 💻 Technology Stack

- Backend: C#, ASP.NET Core 8 MVC, Entity Framework Core 8
- Database: MySQL Server
- Authentication: ASP.NET Core Identity (Users, Roles, Cookie-based auth)
- Frontend: Razor Views, HTML5, CSS3, JavaScript, Bootstrap 5
- Payment Gateway: Stripe
- QR Code Generation: QRCoder Library
- Architecture: Layered Architecture, Repository Pattern, Dependency Injection

## Project Structure

- `Controllers/` — Handles HTTP requests for each entity (Event, Booking, User, etc.)
- `Models/` — Entity classes and ViewModels (`Models/ViewModels/`)
- `Repository/Interfaces/` and `Repository/Services/` — Repository pattern for data access
- `Data/ApplicationDbContext.cs` — Entity Framework Core DB context
- `Migrations/` — Database schema migrations
- `Areas/Identity/Pages/` — Razor Pages for authentication
- `Views/` — Razor views organized by feature
- `wwwroot/` — Static assets (CSS, JS, images, uploads)

## 🚀 Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [MySQL Server](https://dev.mysql.com/downloads/mysql/)
- [Stripe CLI](https://stripe.com/docs/stripe-cli) (Optional for payment testing)

### Setup & Installation

1. Clone the repository:

    ```sh
    git clone https://github.com/ZerfyT/star-events.git
    cd star-events
    ```

2. Configure Database Connection:

    Open `appsettings.json` and update the `DefaultConnection` string with your MySQL credentials.

    ```json
    "ConnectionStrings": {
        "DefaultConnection": "Server=127.0.0.1;Port=3306;Database=star_events;Uid=root;Pwd=user_password;"
    }
    ```

3. Configure Stripe API Keys:

    Add your Stripe keys to appsettings.json.

    ```json
    "Stripe": {
        "SecretKey": "sk_test_...",
        "PublicKey": "pk_test_..."
    }
    ```

4. Apply Database Migrations:

    This will create the database schema based on the Entity Framework models.

    ```sh
    dotnet ef database update
    ```

5. Run the Application:

    The application will start, and the database seeder (DbInitializer) will automatically run to populate the database with initial roles, users, venues, categories, and sample events.

    ```sh
    dotnet run
    ```

    The application will be available at <https://localhost:5001> or a similar port.

6. (Optional) Start Stripe webhook forwarding:

    Run following command on Terminal:

    ```sh
    stripe listen --forward-to http://127.0.0.1:8000/webhook-stripe
    ```

### Seeded Test Accounts

The following user accounts are automatically created by the data seeder:

| Role           | Email                   | Password       |
|----------------|------------------------|----------------|
| Admin          | <admin@starevents.lk>    | Admin@123      |
| Event Organizer| <organizer@starevents.lk>| Organizer@123  |

## License

[MIT](https://choosealicense.com/licenses/mit/)
