# 🎮 NJS1703 Himari Backend

Backend API for the Himari project is built with .NET 8.0 and Entity Framework Core, featuring real-time AI chatbot streaming and PayOS VietQR payment integration.

## 🚀 Quick Start Guide

### 📋 Requirements
- ✅ .NET 8.0 SDK
- 🛢️ SQL Server
- 💻 Visual Studio 2022 or VS Code

### 📥 Clone and Configure
```bash
# Clone repository
git clone https://github.com/lion3993vn/NJS1703_Himari_BE.git
cd NJS1703_Himari_BE

# Create and configure appsettings.json
# Create appsettings.json file with:
```

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "HimariServerLocal": "Data Source=YOUR_SERVER;Initial Catalog=HimariServer;Persist Security Info=True;TrustServerCertificate=True"
  },
  "JWT": {
    "ValidAudience": "HimariServer",
    "ValidIssuer": "http://YOUR_SERVER",
    "SecretKey": "YOUR_SECRET_KEY_HERE",
    "TokenValidityInMinutes": 30,
    "RefreshTokenValidityInDays": 7
  },
  "RedisSettings": {
    "RedisConnectionString": "YOUR_CONNECTION_STRING",
    "InstanceName": "HimariServer",
    "DefaultExpiryMinutes": 30
  },
  "GoogleCredential": {
    "ClientId": "YOUR_CLIENT_ID"
  },
  "MailSettings": {
    "Mail": "YOUR_MAIL",
    "DisplayName": "Himari Cosmetics",
    "Password": "YOUR_APP_PASSWORD",
    "Host": "smtp.gmail.com",
    "Port": 587
  },
  "PayOS": {
    "ClientID": "YOUR_CLIENTID",
    "ApiKey": "YOUR_APIKEY",
    "ChecksumKey": "YOUR_CHECKSUM_KEY",
    "CancelUrl": "CANCEL_URL",
    "ReturnUrl": "RETURN_URL"
  },
  "FirebaseStorage": {
    "BucketName": "BUCKET_NAME"
  },
  "Deepseek": {
    "APIKey": "API_KEY"
  },
  "Gemini": {
    "APIKey": "API_KEY"
  },
  "ChromaDB": {
    "URL": "URL_CHROMA_DB",
    "Collections": "COLLECTIONS_NAME"
  }
}

```

### 🗃️ Create and Update Database
```bash
# Update database from existing migrations
dotnet ef database update
```

### ▶️ Run the Application
```bash
# Run the API
dotnet run
```

API will be available at:
- 🔗 https://localhost:7168
- 🔗 http://localhost:5168
- 📚 Swagger UI: https://localhost:7168/swagger/index.html
