# DeviceManagement.Api

[![CI](https://github.com/yanjason065-creator/DeviceManage/actions/workflows/ci.yml/badge.svg)](https://github.com/yanjason065-creator/DeviceManage/actions/workflows/ci.yml)

Enterprise-style ASP.NET Core Web API with a comprehensive SDET automation framework.

This project demonstrates professional QA Automation practices including:

- API Automation Testing
- Integration Testing Architecture
- Test Framework Design
- Authentication & Authorization Testing
- CI/CD Pipeline Automation
- Code Coverage Analysis
- Mutation Testing
- Quality Engineering Practices


---

# Overview

DeviceManagement.Api is an enterprise-style device management system built with ASP.NET Core Web API (.NET 9).

The main purpose of this project is to demonstrate how a QA Automation Engineer / SDET designs and implements a maintainable API automation framework in a real-world engineering environment.

The project includes:

- Production-style Web API
- Independent Integration Test Framework
- Reusable API Client Layer
- Test Data Management
- Automated Quality Pipeline

```markdown
## Architecture


Detailed architecture:

[Architecture Diagram](architecture.md)

---

# Technology Stack

## Application

| Technology | Purpose |
|---|---|
| ASP.NET Core Web API (.NET 9) | REST API |
| Entity Framework Core | ORM |
| SQL Server | Production Database |
| SQLite In-Memory | Integration Testing Database |
| JWT Authentication | Security |
| Role-based Authorization | Access Control |
| FluentValidation | Request Validation |
| AutoMapper | Object Mapping |


## Testing

| Technology | Purpose |
|---|---|
| xUnit | Test Framework |
| WebApplicationFactory | API Integration Testing |
| HttpClient | API Communication |
| FluentAssertions | Test Assertions |
| Coverlet | Code Coverage |
| ReportGenerator | Coverage Report |
| Stryker.NET | Mutation Testing |


## CI/CD

| Technology | Purpose |
|---|---|
| GitHub Actions | Automated Pipeline |
| GitHub Pages | Coverage Report Hosting |


---

# Application Features

## API Capabilities

Implemented:

- Device CRUD Operations
- Filtering
- Sorting
- Pagination
- Soft Delete
- JWT Authentication
- Role Authorization
- Global Exception Handling
- Health Check


## Security

Implemented:

- JWT Token Authentication
- Role-based Authorization
- Protected API Endpoints
- Authentication / Authorization Test Coverage


---

# Solution Architecture


```
                    GitHub Actions
                          |
                          |
                  Automated Pipeline
                          |
          --------------------------------
          |                              |
     Smoke Tests                  Regression Tests
          |                              |
          --------------------------------
                          |
              Integration Test Framework
                          |
                    HttpClient
                          |
              WebApplicationFactory
                          |
              ASP.NET Core Test Host
                          |
        -----------------------------------
        |                                 |
   Controllers                      Middleware
        |
   Services
        |
   EF Core
        |
 SQLite In-Memory Database
```


---

# Test Automation Framework Architecture


```
DeviceManagement.Api.Tests

├── Assertions
│   ├── ApiResponseAssertions
│   ├── DeviceAssertions
│   └── ValidationAssertions
│
├── Attributes
│   ├── SmokeTestAttribute
│   └── RegressionTestAttribute
│
├── Builders
│   └── DeviceBuilder
│
├── Clients
│   ├── DeviceApiClient
│   ├── AuthApiClient
│   ├── CategoryApiClient
│   └── TestExceptionApiClient
│
├── Helpers
│   ├── DatabaseHelper
│   ├── DeviceTestData
│   ├── DeviceTestHelper
│   ├── JwtHelper
│   └── TestDatabaseInitializer
│
├── Infrastructure
│   ├── CustomWebApplicationFactory
│   ├── IntegrationTestBase
│   └── AuthenticationHelper
│
├── IntegrationTests
│   ├── Authentication
│   ├── Controllers
│   │   └── Devices
│   ├── Health
│   └── Middleware
│
└── Middleware
    └── ExceptionHandlingMiddlewareTests
```


---

# Integration Testing Strategy


The test framework uses a real ASP.NET Core application pipeline.

Unlike isolated unit tests, integration tests validate:

- Controller behavior
- Middleware execution
- Authentication flow
- Authorization rules
- Database interaction
- Exception handling


Test execution flow:


```
Test Case

    |
    v

IntegrationTestBase

    |
    v

CustomWebApplicationFactory

    |
    v

ASP.NET Core Test Server

    |
    v

SQLite In-Memory Database
```


Benefits:

- Real HTTP request pipeline
- Production-like behavior
- Database isolation
- Repeatable execution
- Maintainable test architecture


---

# Test Coverage


## Integration Tests

Total:

```
55+ Test Cases
```


Covered scenarios:

✅ Device Creation  
✅ Device Update  
✅ Device Delete  
✅ Device Query  
✅ Filtering  
✅ Sorting  
✅ Pagination  
✅ Validation  
✅ Authentication  
✅ Authorization  
✅ Exception Handling  
✅ Health Check  


---

# Code Coverage


Tools:

- Coverlet
- ReportGenerator


Current Metrics:


```
Line Coverage:
86%+

Branch Coverage:
80%+
```


Coverage report is automatically generated during CI pipeline and published for review.


---

# Mutation Testing


Tool:

```
Stryker.NET
```


Purpose:

Mutation testing validates whether the test suite can detect real code changes.

Result:


```
Mutation Score: 100%

Detected Mutations: 69/69

Undetected Mutations: 0
```


This demonstrates that tests verify application behavior rather than only increasing code coverage.


---

# CI/CD Pipeline


Workflow:


```
Feature Branch

       |
       v

Push Code

       |
       v

Smoke Tests

       |
       v

Pull Request

       |
       v

Merge Main

       |
       v

Regression Tests

       |
       v

Code Coverage Generation

       |
       v

Coverage Report Deployment
```


Pipeline validates:

- Build Success
- Automated Tests
- Regression Safety
- Coverage Metrics
- Quality Standards


---

# Running Locally


## Clone Repository


```bash
git clone https://github.com/yanjason065-creator/DeviceManage.git
```


## Run API


```bash
dotnet run --project DeviceManagement.Api
```


## Run Tests


```bash
dotnet test
```


## Generate Coverage


```bash
dotnet test \
/p:CollectCoverage=true \
/p:CoverletOutputFormat=cobertura
```


---

# SDET Engineering Highlights


This project demonstrates:

## Test Framework Design

- Reusable API Client abstraction
- Common integration test infrastructure
- Centralized authentication handling
- Test data builders
- Custom assertion framework


## Quality Engineering

- Automated regression testing
- CI quality gates
- Coverage monitoring
- Mutation testing


## Enterprise Testing Practices

- Maintainable test architecture
- Separation of test concerns
- Database isolation
- Real API pipeline validation


---

# Future Improvements


Potential enhancements:

- Docker containerization
- Test execution parallelization
- API Contract Testing
- Performance Testing with JMeter
- UI Automation with Playwright
- Test Reporting Dashboard


---

# Author

QA Automation Engineer / SDET Portfolio Project