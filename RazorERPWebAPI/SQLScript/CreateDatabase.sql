CREATE DATABASE CompanyUsersDB;
USE CompanyUsersDB;

-- Create Companies Table
CREATE TABLE Companies (
    CompanyId INT PRIMARY KEY IDENTITY,
    CompanyName NVARCHAR(100) NOT NULL
);

-- Create Users Table
CREATE TABLE Users (
    UserId INT PRIMARY KEY IDENTITY,
    CompanyId INT FOREIGN KEY REFERENCES Companies(CompanyId),
    Username NVARCHAR(100) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(255) NOT NULL,
    Role NVARCHAR(20) CHECK (Role IN ('Admin', 'User')),
    CreatedAt DATETIME DEFAULT GETDATE()
);