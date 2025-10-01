# User Onboarding Backend

## Azure SQL DB Connection Setup

To set the Azure SQL DB connection string for the backend, use the following environment variable:

```
export ConnectionStrings__DefaultConnection="Server=tcp:<your-server>.database.windows.net,1433;Initial Catalog=<your-db>;Persist Security Info=False;User ID=<your-user>;Password=<your-password>;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
```

Or, set it in your Docker environment or deployment configuration. The backend will read this value at runtime.

**Note:** Never hardcode secrets in source files. Always use environment variables or secret managers.
