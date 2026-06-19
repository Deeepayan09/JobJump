# 🚀 JobJump

<div align="center">

### 💼 AI-Powered Job Portal Platform

Find Jobs • Hire Talent • Analyze Resumes • Smart Recruitment

![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-10.0-blue)
![C#](https://img.shields.io/badge/C%23-.NET-purple)
![SQLite](https://img.shields.io/badge/Database-SQLite-green)
![Bootstrap](https://img.shields.io/badge/UI-Bootstrap-blue)
![AI Powered](https://img.shields.io/badge/AI-Resume%20Scoring-orange)

</div>

---

## 📖 Overview

**JobJump** is a modern job portal built using **ASP.NET Core MVC**, allowing employers to post jobs and candidates to apply seamlessly.

The platform includes:

✅ Job Posting System  
✅ Job Applications  
✅ Resume Uploads  
✅ Resume AI Scoring  
✅ Employer Dashboard Analytics  
✅ Saved Jobs Feature  
✅ Role-Based Authentication  
✅ Applicant Management  
✅ Email Notifications

---

## ✨ Features

### 👨‍💼 Employer Features

- Create Job Listings
- Edit Existing Jobs
- Delete Jobs
- View Posted Jobs
- View Applicants
- Accept Candidates
- Reject Candidates
- Dashboard Analytics
- Resume AI Scoring
- Email Notifications

---

### 👨‍🎓 Employee Features

- Browse Jobs
- Search Jobs
- Filter Jobs
- Apply for Jobs
- Upload Resume
- Save Jobs
- View Saved Jobs
- Track Applications

---

### 🤖 AI Features

- Resume Scoring System
- Candidate Ranking
- Smart Applicant Evaluation

---

## 📊 Dashboard Analytics

Employers get access to:

- Total Jobs Posted
- Total Applications
- Accepted Candidates
- Rejected Candidates
- Pending Applications
- Interactive Charts (Chart.js)

---

## 🛠️ Tech Stack

| Technology | Usage |
|------------|--------|
| ASP.NET Core MVC | Backend |
| C# | Business Logic |
| Entity Framework Core | ORM |
| SQLite | Database |
| ASP.NET Identity | Authentication |
| Bootstrap 5 | UI Design |
| Chart.js | Analytics Dashboard |
| MailKit | Email Notifications |

---

## 📂 Project Structure

```text
JobJump
│
├── Controllers
├── Models
├── Views
├── Data
├── Services
├── wwwroot
├── Seed
├── appsettings.json
└── Program.cs
```

---

## 🔐 Authentication & Authorization

### Roles

#### Employer

- Create Jobs
- Manage Jobs
- View Applicants
- Accept / Reject Applications
- Dashboard Analytics

#### Employee

- Apply Jobs
- Save Jobs
- Upload Resume
- View Applications

---

## 🚀 Getting Started

### Clone Repository

```bash
git clone https://github.com/dd7146/JobJump.git
```

### Navigate

```bash
cd JobJump
```

### Restore Packages

```bash
dotnet restore
```

### Apply Database

```bash
dotnet ef database update
```

### Run Project

```bash
dotnet run
```

---

## ⚙️ Configuration

Create your own:

```json
appsettings.json
```

Example:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=jobjump.db"
  },

  "EmailSettings": {
    "Email": "your-email@gmail.com",
    "Password": "your-app-password",
    "Host": "smtp.gmail.com",
    "Port": 587
  }
}
```

⚠️ `appsettings.json` is excluded from GitHub for security reasons.

---

## 📧 Email Notification System

Automatically sends emails when:

- Application is Accepted
- Application is Rejected

Powered by:

- Gmail SMTP
- MailKit

---

## 📈 Current Features Implemented

- [x] Authentication
- [x] Authorization
- [x] Job Posting
- [x] Job Applications
- [x] Resume Upload
- [x] Saved Jobs
- [x] Dashboard Analytics
- [x] Resume AI Scoring
- [x] Email Notifications

---

## 🔮 Upcoming Features

- [ ] Real-time Chat
- [ ] Admin Panel
- [ ] Dark Mode
- [ ] AI Job Recommendations
- [ ] Candidate Auto Shortlisting
- [ ] Resume Leaderboard
- [ ] Skill Matching Engine
- [ ] Interview Scheduling

---

## 👨‍💻 Author

### Deepayan Bhattacharyya

B.Tech Computer Science Engineering

Passionate about:

- Full Stack Development
- Cloud Computing
- AI Applications
- ASP.NET Development

GitHub:

https://github.com/dd7146

---

## ⭐ Support

If you like this project:

⭐ Star the repository

🍴 Fork the repository

🚀 Contribute to improve JobJump

---

<div align="center">

### 🚀 JobJump — Connecting Talent With Opportunity

Made with ❤️ using ASP.NET Core MVC

</div>
