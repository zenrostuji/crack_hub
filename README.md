# 🚀 CrackHub - Game Sharing & Download Platform

CrackHub is a modern, feature-rich web platform built with ASP.NET Core for browsing, reviewing, and downloading games. It features a robust user system with premium benefits, download management, and an automated notification system.

---

## ✨ Core Features

### 🎮 Game Catalog & Management
- **Extensive Library**: Browse games by category, tags, and features.
- **Detailed Game Info**: High-quality screenshots, system requirements, and crack information.
- **Social Interaction**: User reviews, ratings, and a "Favorites" system to keep track of games.
- **Search System**: Intelligent search with history tracking for easy access.

### 👤 User & Profile System
- **Profile Customization**: Manage personal information and social links.
- **Avatar Frames**: Unlock and equip premium avatar frames to stand out.
- **Authentication**: Secure login with standard credentials or **Google Integrated Login**.

### 📥 Download Management
- **Daily Limits**: Free users are limited to **3 downloads per day** to ensure server stability.
- **Download History**: Keep track of all your downloaded games in your profile.
- **Smart Throttling**: Automated checks to prevent abuse of the download system.

### 💎 Premium Membership
- **Unlimited Downloads**: No daily limits for premium subscribers.
- **Premium Badge**: Exclusive visual styling and avatar frames.
- **Automated Notifications**: Receive email alerts when your premium status is about to expire.

### 🛠️ Admin Dashboard
- **Content Management**: Add and update games, categories, and tags.
- **User Moderation**: Manage user accounts, roles, and premium statuses.
- **System Monitoring**: View logs and send manual notification emails.

---

## 🛠️ Technology Stack

- **Backend**: [ASP.NET Core 8.0 MVC](https://dotnet.microsoft.com/en-us/apps/aspnet/mvc)
- **Database**: [Microsoft SQL Server](https://www.microsoft.com/en-us/sql-server/)
- **ORM**: [Entity Framework Core 9.0](https://learn.microsoft.com/en-us/ef/core/)
- **Authentication**: Cookie-based Auth & [Google OAuth 2.0](https://developers.google.com/identity/protocols/oauth2)
- **Email Service**: [MailKit](http://www.mimekit.net/) (for SMTP notifications)
- **Frontend**: Razor Pages, HTML5, CSS3, JavaScript, jQuery

---

## 🚀 Getting Started

### Prerequisites
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (Express or Developer edition)
- [Visual Studio 2022](https://visualstudio.microsoft.com/vs/) (Recommended)

### Installation & Setup

1. **Clone the repository**:
   ```bash
   git clone https://github.com/zenrostuji/crack_hub.git
   cd crack_hub
   ```

2. **Setup the Database**:
   - Open SQL Server Management Studio (SSMS).
   - Create a new database named `crackhub_db`.
   - Run the following scripts in order:
     1. `crackhub/database.sql` (Creates Schema)
     2. `crackhub/du_lieu_fixed.sql` (Inserts initial data)
     3. `crackhub/dulieu.sql` (Additional data/migrations)

3. **Configure the Application**:
   - Open `crackhub/appsettings.json`.
   - Update the `DefaultConnection` string with your SQL Server credentials.
   - Configure `SmtpSettings` for email notifications (see `EMAIL_NOTIFICATION_README.md` for details).
   - Add your Google `ClientId` and `ClientSecret` if using Google Login.

4. **Run the Project**:
   ```bash
   cd crackhub
   dotnet run
   ```
   The application will be available at `https://localhost:5001` (or your configured port).

---

## 📂 Project Structure

- `crackhub/Controllers`: Logic for handling HTTP requests.
- `crackhub/Models`: Data structures and EF Core entities.
- `crackhub/Views`: UI templates using Razor.
- `crackhub/Repositories`: Data access layer (Repository Pattern).
- `crackhub/Services`: Business logic and background services (Email, Notifications).
- `crackhub/wwwroot`: Static assets (images, CSS, JS).

---

## 🤝 Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## 📄 License

This project is licensed under the MIT License - see the LICENSE file for details.

---
*Created with ❤️ by the CrackHub Team*
