# PriceWatchService

[![GitHub license](https://img.shields.io/github/license/Tanzgarnele/PriceWatchService.svg?style=flat-square)](https://github.com/Tanzgarnele/PriceWatchService/blob/master/LICENSE)
[![GitHub issues](https://img.shields.io/github/issues/Tanzgarnele/PriceWatchService.svg?style=flat-square)](https://github.com/Tanzgarnele/PriceWatchService/issues)

Please note that this project is **primarily developed for learning purposes** and personal use. It may not follow all the best practices or conventions of production-ready projects. You are encouraged to explore and experiment with the codebase, share your findings, and contribute to its improvement.

## Description

PriceWatchApi is a RESTful microservice built with ASP.NET Core Web API. It provides an endpoint for retrieving data about product prices from a third-party website. The microservice includes a scraper library for scraping the data, a background service for periodically updating the data, and a database access layer for storing the data. It also includes Swagger UI for easy API documentation and testing. PriceWatchApi is designed to be easily deployable on Docker containers and is suitable for use in a wide range of applications, including web and mobile apps.

## Getting Started
1. Clone the repository.
2. Create an appsettings.json file in the PriceWatchApi project and fill in the necessary information.
3. Set up the MSSQL database.
4. Run the application.

### appsettings.json
You will need to create an appsettings.json file with the following structure:

```json
{
  "ConnectionStrings": {
    "MSSQL": "your_connection_string"
  },
  "AllowedUrls": {
    "example1": "https://www.example1.org",
    "example2": "https://www.example2.org",
  },
  "AllowedHosts": "*"
}
```
Make sure to replace "your_connection_string" in the appsettings.json section with your actual connection string for the MSSQL database.

### Database
The following schema is used:

```sql
CREATE TABLE Users (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(50) NOT NULL,
    Email NVARCHAR(320) NOT NULL UNIQUE,
    Password NVARCHAR(128) NOT NULL,
    PasswordSalt NVARCHAR(128) NOT NULL,
    Role NVARCHAR(50) NOT NULL,
    CreationDate DATETIME NOT NULL DEFAULT(GETDATE())
);

CREATE TABLE Products (
    Id INT PRIMARY KEY,
    Name VARCHAR(255) NOT NULL,
    Url VARCHAR(255) NOT NULL,
    ThumbnailUrl VARCHAR(255),
    Website VARCHAR(255) NOT NULL,
    DateAdded DATETIME NOT NULL,
    LastChange DATETIME
);

CREATE TABLE UserProducts (
    Id INT PRIMARY KEY,
    UserId INT,
    ProductId INT,
    ThresholdPrice DECIMAL(18, 2) NOT NULL,
    CONSTRAINT FK_User FOREIGN KEY (UserId) REFERENCES Users(Id),
    CONSTRAINT FK_Product FOREIGN KEY (ProductId) REFERENCES Products(Id)
);
```

## Contributing

Pull requests are welcome, and contributions that enhance the project are appreciated.
If you encounter any issues, have suggestions, or want to discuss new features, feel free to open an issue.

## License

This project is licensed under the [MIT License](https://github.com/Tanzgarnele/PriceWatchService/blob/master/LICENSE).
