var builder = DistributedApplication.CreateBuilder(args);

// Register all 4 microservices
// "catalog-api" = the name shown in Aspire dashboard
builder.AddProject<Projects.Catalog_API>("catalog-api");
builder.AddProject<Projects.Ordering_API>("ordering-api");
builder.AddProject<Projects.Customer_API>("customer-api");
builder.AddProject<Projects.Identity_API>("identity-api");

builder.Build().Run();