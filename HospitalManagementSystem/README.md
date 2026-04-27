# Hospital Management System (WinForms + SQLite)

A beginner-friendly desktop application built with C# WinForms and SQLite.

## Modules Included
- Reception dashboard: register visit, search patients, daily and today's visits.
- Patient management: add/edit/delete/search patients.
- Doctor management: add/edit/delete doctors with specialization.
- Appointments: schedule and view all/upcoming appointments.
- Prescriptions: add prescriptions and view full/history by patient.

## Prerequisites
- Visual Studio 2022 Community with **.NET desktop development** workload.

## NuGet Package
1. Right-click project > **Manage NuGet Packages**.
2. Install package: **System.Data.SQLite.Core**.

## Run Steps
1. Open `HospitalManagementSystem.csproj` in Visual Studio 2022.
2. Restore NuGet packages.
3. Build and run (`F5`).
4. On first run, `hospital.db` is created automatically in `bin\Debug\net8.0-windows\`.

## Database Script
- Manual schema script: `Scripts/database.sql`.
- Automatic creation happens in `Data/Database.InitializeDatabase()`.

## Notes
- Uses parameterized queries to reduce SQL injection risk.
- Basic validation and try-catch error handling are included.
- Appointment time expects a valid time format such as `14:30`.
