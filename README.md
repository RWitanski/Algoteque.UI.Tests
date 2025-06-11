This repository contains automated UI tests for the Algoteque web application, using Playwright and xUnit on .NET 8.

---
## Table of Contents
•	#project-structure
•	#configuration
•	#test-workflows
•	#pages
•	#fixtures
•	#running-the-tests
•	#extending-the-tests
•	#troubleshooting

---

## Project Structure
•	Configuration/
Contains configuration loading logic and DTOs.
•	Pages/
Page Object Model (POM) classes for each UI page.
•	Tests.cs
Main test class with test methods.
•	appSettings.json
Configuration file for test parameters.

---

## Configuration
Test settings are managed via appSettings.json in the root directory.
The ReadConfiguration singleton loads these settings at runtime.
Example appSettings.json:
{
  "ConfigurationDto": {
    "WebsiteUrl": "https://example.com",
    "User": "standard_user",
    "Pass": "your_password",
    "Headless": true
  }
}
Configuration fields:
•	WebsiteUrl: Base URL for the application under test.
•	User / Pass: Default credentials for login.
•	Headless: Run browsers in headless mode (true/false).

---

## Test Workflows
Login Test
•	Runs for multiple browsers and user types.
•	Validates successful and failed login scenarios.
Add Product, Checkout, and Back Home
•	Logs in, adds a product to the cart, completes checkout, and returns to the main page.
•	Validates each step using page assertions.

---

## Pages
Each page in the application has a corresponding class in the Pages/ directory, following the Page Object Model pattern.
Examples:
•	LoginPage
•	MainPage
•	CartPage
•	CheckoutStepOnePage
•	CheckoutStepTwoPage
•	CheckoutCompletePage
Each page class encapsulates selectors and actions for that page.

---

## Fixtures
•	PlaywrightFixture
Handles Playwright initialization and browser lifecycle.
•	Provides LaunchBrowserAsync for launching browsers in headless/headful mode.
•	Implements IAsyncLifetime for setup/teardown.

---

## Running the Tests
1.	Install dependencies:
        dotnet restore
2.	Update appSettings.json with your environment details.      
3.	Run tests:
        dotnet test
 1. 
By default, tests run in both Chromium and Firefox browsers.

---

## Extending the Tests
•	Add new page classes in Pages/ for new UI areas.
•	Add new test methods in Tests.cs or additional test classes.
•	Update appSettings.json for new configuration needs.

---

## CI/CD

Automated tests are executed in the CI/CD pipeline using the `algoteque-tests.yml` workflow file.

- The workflow runs on every push and pull request to the main branch.
- It sets up the .NET 8 environment and installs dependencies.
- Playwright browsers are installed using `playwright install`.
- Tests are executed with `dotnet test`.
- Test results and artifacts are published for review.

**Example workflow steps:**
1. Checkout code
2. Setup .NET 8
3. Restore dependencies (`dotnet restore`)
4. Install Playwright browsers (`pwsh bin/Debug/net8.0/playwright.ps1 install`)
5. Run tests (`dotnet test`)
6. Publish results

You can find the workflow file at `workflows/algoteque-tests.yml`.
---

Troubleshooting
•	Ensure appSettings.json is present and correctly formatted.
•	Check browser drivers are compatible with Playwright.
•	Review test output in Visual Studio’s Output pane or terminal for error details.
