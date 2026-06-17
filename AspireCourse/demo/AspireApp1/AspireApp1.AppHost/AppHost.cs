var builder = DistributedApplication.CreateBuilder(args);

var keycloack = builder.AddKeycloak("keycloack", 6001)
    .WithDataVolume("Keycloack-data");

builder.Build().Run();