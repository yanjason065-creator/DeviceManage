
# DeviceManagement.Api

Enterprise-style ASP.NET Core Web API with a comprehensive
integration testing framework designed for QA Automation Engineer /
SDET practices.

This project demonstrates:

- API automation testing
- Integration testing architecture
- Test data management
- Authentication & authorization testing
- CI/CD automation
- Code coverage analysis
- Mutation testing


## Tech Stack

### Backend
- ASP.NET Core Web API (.NET 9)
- Entity Framework Core
- SQL Server
- SQLite In-Memory Database (Testing)

### Authentication
- JWT Authentication
- Role-based Authorization

### Validation
- FluentValidation

### Testing
- xUnit
- WebApplicationFactory
- FluentAssertions
- Coverlet
- ReportGenerator
- Stryker.NET

### CI/CD
- GitHub Actions
- GitHub Pages Coverage Report

Architecture


                 GitHub Actions
                       |
                       |
                Test Pipeline
                       |
        --------------------------------
        |                              |
   Smoke Tests                  Regression Tests
        |                              |
        --------------------------------
                       |
              Integration Tests
                       |
              API Client Layer
                       |
              HttpClient
                       |
          WebApplicationFactory
                       |
        ASP.NET Core Test Host
                       |
        --------------------------
        |                        |
   Controllers              Middleware
        |
   Services
        |
   EF Core
        |
 SQLite InMemory Database


Test Automation Framework
 DeviceManagement.Api.Tests

├── Clients
│   ├── DeviceApiClient
│   ├── AuthApiClient
│   └── TestExceptionApiClient
│
├── Infrastructure
│   ├── CustomWebApplicationFactory
│   ├── IntegrationTestBase
│   └── DatabaseHelper
│
├── Builders
│   └── DeviceBuilder
│
├── Assertions
│   ├── DeviceAssertions
│   └── ValidationAssertions
│
└── Tests
    ├── DeviceTests
    ├── AuthenticationTests
    ├── AuthorizationTests
    └── ExceptionHandlingTests


    Testing Statistics
    ## Test Automation Metrics

Integration Tests:
- 55+ test cases

Coverage:
- Line Coverage: 86%+
- Branch Coverage: 80%+

Mutation Testing:
- Tool: Stryker.NET
- Mutation Score: 100%

Test Categories:
✔ CRUD
✔ Validation
✔ Authentication
✔ Authorization
✔ Exception Handling
✔ Pagination
✔ Filtering
✔ Sorting
✔ Health Check

CI/CD Pipeline
Feature Branch
       |
       |
      Push
       |
       v
 Smoke Test
       |
       v
 Pull Request
       |
       v
 Merge Main
       |
       v
 Regression Test
       |
       v
 Coverage Report
       |
       v
 GitHub Pages

 Quality Engineering
 ## Quality Engineering

### Code Coverage

Coverlet generates coverage data.

ReportGenerator creates HTML reports.

### Mutation Testing

Stryker.NET validates test effectiveness by introducing code mutations.

Result:

Mutation Score: 100%

This ensures tests detect unexpected code changes.