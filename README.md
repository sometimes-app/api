
# Initial Repo Setup
Add an ignore.json at src/Sometimes/ignore.json then add a property for your Mongo db Connection string as such:

```
{
  "MongoDbConnectionString": "<your connection string>"
}
```

## Setting up swashbuckle to gen openapi file
Run: 
- `dotnet new tool-manifest`
- `dotnet tool install SwashBuckle.AspNetCore.Cli`
