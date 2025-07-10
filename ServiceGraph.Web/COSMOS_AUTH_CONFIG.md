# Cosmos DB Authentication Configuration

This application supports two authentication methods for Cosmos DB:

## 1. Managed Service Identity (MSI) - Recommended for Production

Configure in appsettings.json:
```json
{
  "DB": {
    "DatabaseName": "ServiceGraph",
    "AccountEndpoint": "https://your-cosmos-account.documents.azure.com:443/",
    "UseManagedIdentity": true,
    "UseInMemoryDatabase": false
  }
}
```

**Prerequisites:**
- Enable Managed Identity on your App Service
- Grant the App Service's managed identity appropriate permissions on the Cosmos DB account (e.g., "DocumentDB Account Contributor" or "Cosmos DB Operator")

## 2. Connection String - For Development/Legacy

Configure in appsettings.json:
```json
{
  "DB": {
    "DatabaseName": "ServiceGraph", 
    "ConnectionString": "AccountEndpoint=https://your-cosmos-account.documents.azure.com:443/;AccountKey=your-account-key;",
    "UseManagedIdentity": false,
    "UseInMemoryDatabase": false
  }
}
```

**Note:** When `UseManagedIdentity` is true, the `ConnectionString` property is ignored.