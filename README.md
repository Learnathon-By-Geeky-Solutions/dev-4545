<div align="center">

# 🧑‍💻 Task Tracker – Learnathon Project

[![.NET](https://img.shields.io/badge/.NET-8.0-%23512BD4?style=for-the-badge&logo=dotnet)](https://dotnet.microsoft.com/)
[![React](https://img.shields.io/badge/React-18.2-%2361DAFB?style=for-the-badge&logo=react)](https://reactjs.org/)
[![CI/CD](https://github.com/Learnathon-By-Geeky-Solutions/dev-4545/actions/workflows/CI-test-coverage.yml/badge.svg)](https://github.com/Learnathon-By-Geeky-Solutions/dev-4545/actions)
[![SonarCloud](https://sonarcloud.io/api/project_badges/measure?project=Learnathon-By-Geeky-Solutions_dev-4545&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=Learnathon-By-Geeky-Solutions_dev-4545)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=Learnathon-By-Geeky-Solutions_dev-4545&metric=coverage)](https://sonarcloud.io/summary/new_code?id=Learnathon-By-Geeky-Solutions_dev-4545)
[![API DOC](https://img.shields.io/badge/API%20DOC-view-grey?labelColor=85EA2D&style=for-the-badge&logo=swagger&logoColor=black)](https://app.swaggerhub.com/apis-docs/NazmusSakibRhythm/employee-api/1.0)
[![Live Demo](https://img.shields.io/badge/LIVE_DEMO-AVAILABLE-green?style=for-the-badge)](https://tasktracker-learnathon.netlify.app/)

</div>

---

## 📖 Table of Contents
- [🚀 Features](#-features)
- [👥 Team](#-team)
- [🛠 Tech Stack](#-tech-stack)
- [⚙️ Installation](#️-installation)
- [📄 Configuration](#-configuration)
- [🗄 Database Setup](#-database-setup)
- [📊 Diagrams](#-diagrams)
- [SonarCloud Analysis](#sonarcloud-analysis-)
- [🚀 CI/CD Pipeline](#-cicd-pipeline)
- [📞 Contact](#-contact)

---

## 🛠️ Tech Stack

- **Backend:** ASP.NET 8 (C#)
- **Frontend:** React 18.2
- **Architecture:** Clean Architecture
- **CI/CD:** GitHub Actions
- **Code Quality:** SonarCloud
- **Documentation:** Swagger

---
## 🧱 **Clean Architecture Overview**

**This project strictly adheres to [Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html) principles**, emphasizing:

### **Layered Structure**
- **🟢 Core Layer**: Contains enterprise-wide **business entities**, **validation rules**, and domain logic.
- **🟡 Application Layer**: Implements **use cases**, **DTOs**, and interfaces (abstractions for external services).
- **🔵 Infrastructure Layer**: Handles external concerns - **database operations**, API integrations, and framework-specific implementations.
- **🟣 Presentation Layer**: React-based UI that consumes the backend API.

### **Key Principles**
- **🚀 Dependency Rule**: Inner layers **never depend on outer layers** (Core → Application → Infrastructure → Presentation).
- **🧪 Testability**: Clear separation enables **unit testing** (Core/Application) and **integration testing** (Infrastructure).
- **🔗 Dependency Injection**: Leverages .NET's DI to invert dependencies between layers.
- **🌐 Decoupled Frontend**: React app communicates via API contracts, enabling independent scaling.

**Why It Matters**: This architecture ensures long-term **maintainability**, **flexibility to change tech stacks**, and **team collaboration efficiency**.

---

## 📂 Project Structure
📦 src

├─ 🏗️ Employee.API         # Presentation Layer (ASP.NET Web API).

├─ 🧠 Employee.Core       # Core Layer (Entities, Domain Services).

├─ 🛠️ Employee.Application # Application Layer (Use Cases, DTOs, Interfaces).

└─ 📦 Employee.Infrastructure # Infrastructure Layer (EF Core, Repositories).

---


## 🚀 Features
- **Role-Based Access Control** 👨💼👩💻
- **Task Management System** ✅📅
- **Leave Application Workflow** 🏖️📩
- **Real-time Caching with Redis** ⚡🔴
- **Salary Management System** 💰📈
- **Comprehensive Reporting** 📊📑

---

## 👥 Team & Mentorship

### Team Members
| Role          | Name                  | GitHub Profile                                      |
|---------------|-----------------------|----------------------------------------------------|
| Team Leader   | Nazmus Sakib          | [![GitHub](https://img.shields.io/badge/GitHub-arghya--n-blue)](https://github.com/arghya-n) |
| Developer     | Md. Mubasshir Naib    | [![GitHub](https://img.shields.io/badge/GitHub-MubasshirNaib-green)](https://github.com/MubasshirNaib) |
| Developer     | Saikat Hossain Shohag | [![GitHub](https://img.shields.io/badge/GitHub-shohag1102-red)](https://github.com/shohag1102) |

### Mentor
| Role          | Name            | GitHub Profile                                      |
|---------------|-----------------|----------------------------------------------------|
| Mentor        | Sakib Mahmood   | [![GitHub](https://img.shields.io/badge/GitHub-sakibmahmood98-lightgrey)](https://github.com/sakibmahmood98) |

---

## 🛠 Tech Stack
### Backend
![.NET 8](https://img.shields.io/badge/.NET%208-512BD4?logo=dotnet&logoColor=white)
![Entity Framework Core](https://img.shields.io/badge/EF%20Core-4D26CE?logo=dotnet&logoColor=white)
![CQRS Pattern](https://img.shields.io/badge/CQRS-Architecture-blueviolet)
![Redis](https://img.shields.io/badge/Redis-DC382D?logo=redis&logoColor=white)

### Frontend
![React 18](https://img.shields.io/badge/React-20232A?logo=react&logoColor=61DAFB)
![TypeScript](https://img.shields.io/badge/TypeScript-3178C6?logo=typescript&logoColor=white)
![Ant Design](https://img.shields.io/badge/Ant%20Design-0170FE?logo=ant-design&logoColor=white)

**UI Template Courtesy:**  
Professional dashboard template provided by [Vivasoft](https://vivasoftltd.com/)

### DevOps
![GitHub Actions](https://img.shields.io/badge/GitHub_Actions-2088FF?logo=github-actions&logoColor=white)
![Docker](https://img.shields.io/badge/Docker-2496ED?logo=docker&logoColor=white)
![SonarCloud](https://img.shields.io/badge/SonarCloud-F3702A?logo=sonarcloud&logoColor=white)

---

## ⚙️ Installation

### Backend Setup
```bash
# Clone repository
git clone https://github.com/Learnathon-By-Geeky-Solutions/dev-4545.git
cd dev-4545/Employee.API

# Restore dependencies
dotnet restore

# Configure database (update connection string in appsettings.json)
dotnet ef database update --project Employee.Infrastructure

# Run the API
dotnet run
```

### Frontend Setup
```bash
cd frontend/EmpUI

# Install dependencies
npm install

# Start development server
npm run dev
```

---

## 📄 Configuration
Update `appsettings.json` with your environment values:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Your Connection String",
    "RedisURL": "Your Redis URL"
  },
  "Jwt": {
    "Key": "your_super_secure_key_here",
    "Issuer": "your Issuer",
    "Audience": "Your Audience"
  },
  "Cors": {
    "url": "Your frontend URL" 
  }
}
```

---

## 🗄 Database Setup
![ER Diagram](https://img.shields.io/badge/ER_Diagram-PDF-blue?style=flat-square) 
[View ER Diagram](https://github.com/Learnathon-By-Geeky-Solutions/dev-4545/blob/doc/er-diagram.pdf)

---

## 📊 Diagrams
| Diagram Type          | Link                                                                                   |
|-----------------------|---------------------------------------------------------------------------------------|
| **UML Diagram**       | [View UML Diagram](https://github.com/Learnathon-By-Geeky-Solutions/dev-4545/blob/doc/Employee%20Management%20UML.pdf) |
| **Activity Diagram**  | [View Activity Diagram](https://github.com/Learnathon-By-Geeky-Solutions/dev-4545/blob/doc/Activity%20Diagram.pdf)     |

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

## 🚀 CI/CD Pipeline
![CI/CD Flow](https://img.shields.io/badge/Workflow-GitHub_Actions-blue?logo=github-actions)

```mermaid
graph LR
A[Code Commit] --> B[Run Tests]
B --> C{Passed?}
C -->|Yes| D[Build & Package]
C -->|No| E[Alert Developers]
D --> F[Deploy to Staging]
F --> G[Run Integration Tests]
G --> H{Passed?}
H -->|Yes| I[Deploy to Production]
H -->|No| E
```

---

## 📞 Contact

| Team Member            | Email Address                          | GitHub Profile                                                      |
|------------------------|----------------------------------------|---------------------------------------------------------------------|
| **Nazmus Sakib**       | 📧 [sakib.hb7@gmail.com]()             | 🐙 [arghya-n](https://github.com/arghya-n)                          |
| **Mubasshir Naib**     | 📧 [u1904089@student.cuet.ac.bd]()     | 🐙 [MubasshirNaib](https://github.com/MubasshirNaib)                |
| **Saikat Hossain Shohag** | 📧 [u1904088@student.cuet.ac.bd]() | 🐙 [shohag1102](https://github.com/shohag1102)                      |

[![Report Issue](https://img.shields.io/badge/REPORT_ISSUE-GITHUB-red?style=for-the-badge)](https://github.com/Learnathon-By-Geeky-Solutions/dev-4545/issues)

---

<div align="center">
  <h3>🚀 Powered by Geeky Solutions Learnathon 2024 🚀</h3>
  <img src="https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSJX8KMkRkv3ipsAZCdn7mFDkrgdsl2Pa6Ow_RyOSUXButka0gA7oekX5n_nZMeqGjqiuk&usqp=CAU" width="100" alt="Geeky Solutions Logo">
</div>
