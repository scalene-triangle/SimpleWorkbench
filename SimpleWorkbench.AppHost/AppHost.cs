var builder = DistributedApplication.CreateBuilder(args);

var api = builder.AddProject<Projects.SimpleWorkbench_Api>("api");

builder.AddNpmApp("web", "../apps/simple-workbench-web", "dev")
    .WithHttpEndpoint(env: "PORT")
    .WithReference(api)
    .WaitFor(api);

builder.Build().Run();
