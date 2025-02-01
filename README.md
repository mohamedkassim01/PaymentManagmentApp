# PaymentManagmentApp

This repository contains the **PaymentManagmentApp** project, which includes both the backend and frontend components within the same repository.

- **Backend:** ASP.NET Core API for managing payment data.
- **Frontend:** React application built with Vite for managing payments.

## Table of Contents

- [Overview](#overview)
- [Backend (ASP.NET Core API)](#backend-aspnet-core-api)
- [PaymentApp (React with Vite)](#paymentapp-react-with-vite)
- [Directory Structure](#directory-structure)
 
## Overview

This repository contains two projects:

1. **Backend**: An ASP.NET Core API that handles the logic for managing payment data.
2. **Frontend**: A React-based web application built with Vite that communicates with the backend API to manage payment data through a user-friendly interface.

## Backend (ASP.NET Core API)

The backend project is built using **ASP.NET Core** and serves as the API for managing payments.

### Features

- CRUD operations for payments.
- Database interaction via Entity Framework.

### Setup

1. Navigate to the **Backend** directory:

```bash
   cd Backend\SystemPayment.api
```


2. Restore the dependencies:

```bash
   dotnet restore
```

3. Set up your database connection string in `appsettings.json`.

4. Run the application:

```bash
   dotnet run
```

The backend API will be available at `http://localhost:5190/swagger/index.html`.

## PaymentApp (React with Vite)

The PaymentApp is a React application, built with Vite to provide fast and efficient development. 

### Features
User interface for creating, updating, and managing payment records.
Responsive design for a good user experience.
Integration with the backend API to perform CRUD operations on payment data.
### Setup
Navigate to the Frontend directory:

```bash
cd PaymentApp
```
Install the required dependencies:

```bash
npm install  --force
```
Start the development server:

```bash
npm run dev
```
The frontend application will be available at `http://localhost:5173`.

### Directory Structure
```bash
/PaymentManagmentApp
  ├── /Backend       # ASP.NET Core API project
  └── /PaymentApp      # React + Vite frontend project
```
**/Backend**: Contains all files for the backend API, including controllers, models, and database configuration.
**/PaymentApp**: Contains all files for the frontend application, including React components, state management, and API calls.
