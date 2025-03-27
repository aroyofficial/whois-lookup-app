# WhoisLookupAPI

## Overview

WhoisLookupAPI is an enterprise-level .NET Core API designed for performing WHOIS lookups on domain names. It integrates with WHOIS services to fetch domain registration details, ensuring reliability and scalability for large-scale queries. The API is built with a modular, maintainable, and highly efficient architecture that adheres to best practices for enterprise software development.

## Features

- **WHOIS Domain Lookup** – Retrieve detailed WHOIS information for domain names.
- **Logging & Error Handling** – Uses structured logging with BetterStack and custom exception handling.
- **Middleware Support** – Implements custom middleware for request validation, authentication, and error handling.
- **Configuration Management** – Uses `appsettings.json` to support environment-specific settings.
- **Dependency Injection** – Implements best practices for modular and scalable service management.
- **Caching** – Supports Redis caching to improve API response time and reduce redundant WHOIS queries.
- **Security** – Includes authentication and authorization mechanisms for controlled access.

## Tech Stack

- **Backend:** .NET Core 6+ with ASP.NET Web API
- **Logging:** BetterStack Log Service
- **Cache:** Redis (via StackExchange.Redis)
- **Serialization:** Newtonsoft.Json
- **Database:** PostgreSQL (optional, for logging and analytics)
- **Dependency Management:** Microsoft Dependency Injection
- **Testing Framework:** xUnit (recommended for unit and integration testing)

## Installation & Setup

### Prerequisites

Ensure you have the following installed on your system:

- .NET 6 SDK or later
- Redis (for caching, optional but recommended)
- PostgreSQL (optional, for logging and analytics)
- Docker (optional, for containerized deployment)
- Git (for version control)

### Steps to Run Locally

1. **Clone the Repository**

   ```sh
   git clone https://github.com/your-repo/whois-lookup-api.git
   cd whois-lookup-api
   ```

2. **Set Up Environment Variables**

   - Copy `appsettings.Development.json` and update necessary API keys and database settings.
   - Configure Redis and PostgreSQL settings in `appsettings.json`.

3. **Restore Dependencies**

   ```sh
   dotnet restore
   ```

4. **Run Database Migrations (if using PostgreSQL)**

   ```sh
   dotnet ef database update
   ```

5. **Run the Application**
   ```sh
   dotnet run --project WhoisLookupAPI
   ```

## API Endpoints

### 1. Domain WHOIS Lookup

**Endpoint:** `GET /api/whois/{domain}`  
**Description:** Fetch WHOIS details for a domain.  
**Response:**

```json
{
	"domain": "example.com",
	"registrant": "John Doe",
	"expiryDate": "2026-01-01",
	"status": "Active"
}
```

### 2. Health Check

**Endpoint:** `GET /api/health`  
**Description:** Checks if the API is running.  
**Response:**

```json
{
	"status": "Healthy",
	"timestamp": "2025-03-27T10:00:00Z"
}
```

### 3. Cache Clear

**Endpoint:** `DELETE /api/cache`  
**Description:** Clears the Redis cache.

## Architecture

### Project Structure

```
WhoisLookupAPI/
├── ApiClients/        # Handles external API calls (Whois API)
├── Configurations/    # App configuration settings
├── Constants/         # Application-wide constant values
├── Controllers/       # API endpoints
├── Enumerations/      # Enum types for structured data
├── Exceptions/        # Custom exception handling
├── Middlewares/       # Custom middleware for request processing
├── Models/            # Data models for request/response
├── Services/          # Business logic layer
├── Utilities/         # Helper functions
├── appsettings.json   # Configuration settings
├── Startup.cs        # Dependency injection setup
├── Program.cs        # API entry point
```

## Deployment

### Docker Deployment

1. **Build the Docker Image**
   ```sh
   docker build -t whoislookupapi .
   ```
2. **Run the Container**
   ```sh
   docker run -p 5000:5000 whoislookupapi
   ```

### Kubernetes Deployment (Optional)

For production-scale deployment, you can deploy this API using Kubernetes. Below is a sample `deployment.yaml`:

```yaml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: whoislookupapi
spec:
  replicas: 3
  selector:
    matchLabels:
      app: whoislookupapi
  template:
    metadata:
      labels:
        app: whoislookupapi
    spec:
      containers:
        - name: whoislookupapi
          image: whoislookupapi:latest
          ports:
            - containerPort: 5000
```

## Testing

### Running Unit Tests

```sh
dotnet test
```

## Contributing

1. Fork the repository.
2. Create a new branch (`feature-branch`).
3. Commit changes and push to your fork.
4. Open a Pull Request.

## License

This project is licensed under the MIT License.

---

For any issues, create a ticket or reach out to the maintainer.
