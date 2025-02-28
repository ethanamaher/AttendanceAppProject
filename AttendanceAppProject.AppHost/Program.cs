var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.AttendanceAppProject_ApiService>("apiservice");

builder.AddProject<Projects.AttendanceAppProject_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();
