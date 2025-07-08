# TA_Csharp_Selenium_Restsharp


Test Automation Framework built with **C#**, integrating **Selenium WebDriver** for UI tests, **RestSharp** for API testing, and **Reqnroll** (SpecFlow-compatible) for BDD-style scenarios. Enhanced with **Allure Reports** for beautiful, actionable test results.

---

## 🛠️ Tech Stack

| Layer         | Tools/Libraries                                    |
|---------------|-----------------------------------------------------|
| UI Testing    | [Selenium WebDriver](https://www.selenium.dev/)     |
| API Testing   | [RestSharp](https://restsharp.dev/)                |
| BDD Layer     | [Reqnroll](https://reqnroll.dev/) (SpecFlow-style) |
| Test Runner   | [NUnit](https://nunit.org/)                        |
| Reporting     | [Allure](https://docs.qameta.io/allure/)           |
| Data Faker    | [Bogus](https://github.com/bchavez/Bogus)          |
| .NET Version  | .NET 8.0                                            |

---

## 🚀 Getting Started

### 📦 Prerequisites

- [.NET SDK 8.0+](https://dotnet.microsoft.com/)
- Chrome or another WebDriver-compatible browser
- [Allure CLI](https://docs.qameta.io/allure/#_installing_a_commandline) installed

### 📁 Folder Structure

TA_Csharp_Selenium_Restsharp/
│
  ├── UI_Automation/ # Selenium + Reqnroll steps
  ├── API_Tests/ # RestSharp test suites
  ├── Hooks/ # Setup & teardown
  ├── Pages/ # Page Object Models
  ├── Features/ # .feature BDD files
  ├── TestData/ # Fake data generators (Bogus)
  ├── allure-results/ # Raw Allure results
  ├── allure-report/ # Final Allure HTML report
  └── allureConfig.json # Allure configuration
---

## 🧪 Running Tests

### Run all tests

```bash
dotnet test
Run only tests with a specific category (e.g. Smoke)
dotnet test --filter "Category=Smoke"
Run UI tests with WebDriver
dotnet test UI_Automation/UI_Automation.csproj
Run API tests
dotnet test API_Tests/API_Tests.csproj
📊 Generating Allure Report
Step 1: Run tests
dotnet test
This creates the allure-results/ directory with test result JSON files.

Step 2: Generate and open the report
allure generate allure-results --clean -o allure-report
allure open allure-report
⚠️ Do not run allure generate on allure-report or allure-reports — input must be allure-results.

🧰 Configuration
appsettings.json

🧩 Sample Tools & Patterns
✅ Page Object Model (POM)

✅ Fluent assertions

✅ Bogus for fake data generation

✅ Custom WebDriver factory

✅ Reqnroll hooks for dependency injection and cleanup

🧼 Cleanup & Hooks
[BeforeScenario] and [AfterScenario] hooks are used for:

WebDriver lifecycle management

Allure attachment handling

API context reset

📦 Dependencies
dotnet add package Selenium.WebDriver
dotnet add package RestSharp
dotnet add package Reqnroll.NUnit
dotnet add package Allure.Commons
dotnet add package Bogus
Add this to UI_Automation.csproj and API_Tests.csproj as needed.

📋 Example dotnet test Commands
dotnet test                                # Run all tests
dotnet test --filter "Name=LoginTest"      # Filter by test name
dotnet test --filter "Category=Smoke"      # Filter by category
dotnet test --list-tests                   # List all available test methods
dotnet test --results-directory ./results  # Save TRX results to folder
dotnet test --logger "trx;LogFileName=test.trx"

👥 Contributors
Dušan Milić – QA Automation Engineer

📄 License
This project is licensed under the MIT License.
