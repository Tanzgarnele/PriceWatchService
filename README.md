# PriceWatchService

[![GitHub license](https://img.shields.io/github/license/Tanzgarnele/PriceWatchService.svg?style=flat-square)](https://github.com/Tanzgarnele/PriceWatchService/blob/master/LICENSE)
[![GitHub release](https://img.shields.io/github/release/Tanzgarnele/PriceWatchService.svg?style=flat-square)](https://github.com/Tanzgarnele/PriceWatchService/releases)
[![GitHub issues](https://img.shields.io/github/issues/Tanzgarnele/PriceWatchService.svg?style=flat-square)](https://github.com/Tanzgarnele/PriceWatchService/issues)

**Note: This project is still a work in progress.**

## Description

PriceWatchApi is a RESTful microservice built with ASP.NET Core Web API. It provides an endpoint for retrieving data about product prices from a third-party website. The microservice includes a scraper library for scraping the data, a background service for periodically updating the data, and a database access layer for storing the data. It also includes Swagger UI for easy API documentation and testing. PriceWatchApi is designed to be easily deployable on Docker containers and is suitable for use in a wide range of applications, including web and mobile apps.

## Getting Started
1. Clone the repository.
2. Create an appsettings.json file in the PriceWatchApi project and fill in the necessary information.
3. Run the application.

### appsettings.json
You will need to create an appsettings.json file with the following structure:

```json
{
  "ConnectionStrings": {
    "MSSQL": "your_connection_string"
  },
  "AllowedHosts": "*"
}
```

## Contributing

Contributions are welcome! Please feel free to submit a pull request or open an issue if you encounter any problems or have any suggestions.

## License

This project is licensed under the [MIT License](https://github.com/<USERNAME>/<REPO_NAME>/blob/master/LICENSE).
