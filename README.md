# Employee Management System 🌟🏢

## Table of Contents 📑
- [Description](#description) 📝
- [Features](#features) ✨
- [Technologies Used](#technologies-used) 🛠️
- [Installation Instructions](#installation-instructions) 📦
- [Usage](#usage) 🖥️
- [Configuration](#configuration) ⚙️
  - [appsettings.json Example](#appsettingsjson-example) 📄
  - [Configuration Settings Explanation](#configuration-settings-explanation) 📝
- [Database Setup](#database-setup) 🗄️
- [Caching with Redis](#caching-with-redis) 🚀
  - [Redis Configuration Process](#redis-configuration-process) ⚙️
- [CI/CD Deployment](#cicd-deployment) 🚀
- [SonarCloud Analysis](#sonarcloud-analysis) 📊
- [Contributing](#contributing) 🤝
- [License](#license) 📄
- [Contact](#contact) 📞

---

## Description 📝✨
The **Employee Management System** is a cutting-edge 🌐 web-based application designed to simplify managing employee tasks, features, and projects. It offers a robust platform to track activities 📈, assign tasks ✅, and monitor progress 🚀. Built with modern tech and architectural patterns, it ensures scalability 📏, maintainability 🛠️, and top-notch performance ⚡.

---

## Team Members
| Name | GitHub |
|------|--------|
| Nazmus Sakib (Team Leader) | [![GitHub](https://img.shields.io/badge/GitHub-100000?style=flat&logo=github&logoColor=white)](https://github.com/arghya-n) |
|  Md. Mubasshir Naib | [![GitHub](https://img.shields.io/badge/GitHub-100000?style=flat&logo=github&logoColor=white)](https://github.com/MubasshirNaib) |
|  Saikat Hossain Shohag | [![GitHub](https://img.shields.io/badge/GitHub-100000?style=flat&logo=github&logoColor=white)](https://github.com/shohag1102) |



## Features ✨🌈
- ✅ **Task Management:** Create, assign, and track tasks with ease! 🗂️
- 🛠️ **Feature Management:** Define and manage project features effortlessly. 🔧
- 📊 **Project Oversight:** Keep an eye on progress and contributions. 👀
- 🔐 **User Authentication:** Secure role-based access control. 🛡️
- 📈 **Reporting:** Generate insightful employee and project reports. 📋
- 🔔 **Notifications:** Stay updated with task alerts and deadlines. ⏰

---

## Technologies Used 🛠️💻
- 🌟 **.NET 8:** Harnessing the latest .NET features for speed and power! ⚡
- 🏛️ **Clean Architecture:** Modular, testable, and framework-independent design. 🧩
- 🔄 **CQRS:** Separate commands and queries for scalable logic. ⚙️
- 🚀 **Redis Caching:** Boost performance with lightning-fast caching. ⚡
- 🗄️ **SQL Server:** Reliable and robust data storage. 💾
- 🌐 **CI/CD Deployment:** Automated workflows for seamless delivery. 📦

---

## Installation Instructions 📦🛠️
1. **Prerequisites:** 📋✅
   - .NET 8 SDK 🌟
   - SQL Server (local or remote) 🗄️
   - Redis Server 🚀
   - Git 🌿

2. **Clone the Repository:** 📥⬇️
   ```bash
   git clone https://github.com/Learnathon-By-Geeky-Solutions/dev-4545.git
   cd dev-4545/Employee.API
   ```

3. **Restore Dependencies:** 🔄♻️
   ```bash
   dotnet restore
   ```

4. **Database Setup:** 🗄️🔧
   - Ensure SQL Server is running 🟢.
   - Update `appsettings.json` connection string 📝.
   - Run migrations:
     ```bash
     dotnet ef database update --project Employee.Infrastructure
     ```

5. **Redis Setup:** 🚀⚡
   - Install and run Redis locally or use a remote instance 🌐.
   - Update `appsettings.json` Redis connection string 📝.

6. **Run the Application:** ▶️🎉
   ```bash
   dotnet run
   ```
   - Access at `https://localhost:{Your PORT}`  

---

## Usage 🖥️🌟
The system offers role-based interfaces:
- **Admin:** Manage users, roles, Assigns Project, Features & Tasks, Approve Leave and Update Salary ⚙️👑.

- **Employee:** View his own project,features & tasks, update  his work status, and apply for leave 📩👷.

**Get Started:**
1. Log in with credentials 🔑.
2. Explore features via the menu 🧭.
3. Check in-app help for details ℹ️.

---

## Configuration ⚙️🔧
Manage settings via `appsettings.json` and environment variables 🌍.

### appsettings.json Example 📄
```json
{
    "Logging": {
        "LogLevel": {
            "Default": "Information",
            "Microsoft.AspNetCore": "Warning"
        }
    },
    "AllowedHosts": "*",
    "ConnectionStrings": {
        "DefaultConnection": "{ConnectionString}",
        "RedisURL": "{RedisURL}"
    },
    "Jwt": {
        "Key": "{JwtKey}",
        "Issuer": "{JwtIssuer}",
        "Audience": "{JwtAudience}"
    },
    "Cors": {
        "url": "{ReactURL}"
    }
}
```

### Configuration Settings Explanation 📝
- **AllowedHosts:** Specifies allowed hosts for the application. `*` allows all hosts.
- **ConnectionStrings:**
  - `DefaultConnection`: Connection string for the SQL Server database.
  - `RedisURL`: Connection string for the Redis server.
- **Jwt:** Settings for JSON Web Token (JWT) authentication.
  - `Key`: Secret key for signing JWT tokens.
  - `Issuer`: Issuer of the JWT token.
  - `Audience`: Audience for the JWT token.
- **Cors:** Cross-Origin Resource Sharing settings.
  - `url`: URL for the React frontend application.

Use `appsettings.Development.json` for development settings 🛠️ and secure sensitive data 🔒.

---

## Database Setup 🗄️💾
Using SQL Server:
1. Create a database 🆕.
2. Update `appsettings.json` connection string 📝.
3. Apply migrations: `dotnet ef database update` 🔄.

Includes initial seeding for users and roles 🌱.

---

## Caching with Redis 🚀⚡
Boosts performance with Redis:
- **Config:** Set connection string in `appsettings.json` 📝.
- **Manage:** Use Redis CLI/tools to inspect or clear cache 🧹.

### Redis Configuration Process ⚙️
1. **Install Redis:** Download and install Redis from [redis.io](https://redis.io/) if not already installed.
2. **Run Redis Server:** Start the Redis server locally (e.g., `redis-server`) or ensure your remote Redis instance is accessible.
3. **Update Connection String:** In `appsettings.json`, replace `RedisURL` with your Redis connection string (e.g., `localhost:6379`).
4. **Verify Connection:** Use Redis CLI (e.g., `redis-cli ping`) or a Redis client to verify the connection. A `PONG` response indicates success.

---

## CI/CD Deployment 🚀🌐
![Build Status](https://github.com/yourusername/EmployeeManagementSystem/actions/workflows/ci-cd.yml/badge.svg) ✅

Automated pipeline:
- **CI:** Builds and tests on each commit 🛠️.
- **CD:** Deploys to staging/production on success 🌟.
- **Config:** See [`.github/workflows/ci-cd.yml`] 📋.

---

## SonarCloud Analysis 📊🔍
Key metrics for code quality:

| Metric 🎯        | Status 📈                                                                                   | Icon 🌟 |
|------------------|---------------------------------------------------------------------------------------------|---------|
| Quality Gate ✅  | [![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=Learnathon-By-Geeky-Solutions_dev-4545&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=Learnathon-By-Geeky-Solutions_dev-4545) | 🏆      |
| Bugs 🐞         | [![Bugs](https://sonarcloud.io/api/project_badges/measure?project=Learnathon-By-Geeky-Solutions_dev-4545&metric=bugs)](https://sonarcloud.io/summary/new_code?id=Learnathon-By-Geeky-Solutions_dev-4545)                 | 🚫      |
| Vulnerabilities 🔓 | [![Vulnerabilities](https://sonarcloud.io/api/project_badges/measure?project=Learnathon-By-Geeky-Solutions_dev-4545&metric=vulnerabilities)](https://sonarcloud.io/summary/new_code?id=Learnathon-By-Geeky-Solutions_dev-4545) | 🛡️      |
| Code Smells 👃  | [![Code Smells](https://sonarcloud.io/api/project_badges/measure?project=Learnathon-By-Geeky-Solutions_dev-4545&metric=code_smells)](https://sonarcloud.io/summary/new_code?id=Learnathon-By-Geeky-Solutions_dev-4545)     | 🧹      |
| Coverage 📏     | [![Coverage](https://sonarcloud.io/api/project_badges/measure?project=Learnathon-By-Geeky-Solutions_dev-4545&metric=coverage)](https://sonarcloud.io/summary/new_code?id=Learnathon-By-Geeky-Solutions_dev-4545)       | 📊      |



---

## Contact 📞💬
Reach out to [Your Name] at [sakib.hb7@gmail.com] 📧 or open a GitHub issue 🚨.