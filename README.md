# DemoAPI

This is a .NET 7 API for currency conversion, with support for rate limiting, logging, error handling, and unit testing.

## Functional Requirements

### 1. Currency Conversion Endpoint

- **Endpoint:** GET /convert
- **Description:** Converts a specified amount from a source currency to a target currency.
- **Query Parameters:**
  - `sourceCurrency` (string): The ISO 4217 code of the source currency.
  - `targetCurrency` (string): The ISO 4217 code of the target currency.
  - `amount` (decimal): The amount to convert from the source currency.
- **Response:** A JSON object with the following properties:
  - `exchangeRate` (decimal): The exchange rate used for conversion.
  - `convertedAmount` (decimal): The resulting amount in the target currency.

## Non-Functional Requirements

### 1. Unit Testing

- Unit tests cover conversion logic, including edge cases.
- Any testing framework and mocking libraries can be used.

### 2. Logging

- Key events in the application, including successful conversions and errors, are logged.
- A 3rd party logging library can be used if desired.

### 3. Error Handling

- Robust error handling is implemented for invalid input and unsupported currencies.
- Descriptive error messages are returned in the API response.

## Setup Instructions

1. Clone the repository.
2. Navigate to the project directory.
3. Build the solution using `dotnet build`.
4. Run the application using `dotnet run`.

## API Endpoints

### Convert Currency

- **Method:** GET
- **Endpoint:** /convert
- **Query Parameters:**
  - `sourceCurrency`: ISO 4217 code of the source currency.
  - `targetCurrency`: ISO 4217 code of the target currency.
  - `amount`: Amount to convert from the source currency.

### Example Request/Response
```http
GET /convert?sourceCurrency=USD&targetCurrency=EUR&amount=100

{
  "exchangeRate": 0.85,
  "convertedAmount": 85
}

