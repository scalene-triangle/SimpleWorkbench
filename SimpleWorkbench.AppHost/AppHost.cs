var builder = DistributedApplication.CreateBuilder(args);

var api = builder.AddProject<Projects.SimpleWorkbench_Api>("api");

builder.AddNpmApp("web", "../apps/simple-workbench-web", "dev")
    .WithReference(api)
    .WaitFor(api);

builder.Build().Run();
