using Grpc.Core;

#pragma warning disable ASPIRECERTIFICATES001
var builder = DistributedApplication.CreateBuilder(args);

var keycloack = builder.AddKeycloak("keycloack", 6001)
    .WithoutHttpsCertificate()
    .WithDataVolume("Keycloack-data");

var postgres = builder.AddPostgres("postgres", port: 5432)
    .WithDataVolume("postgres-data")
    .WithImage("postgres","18")
    .WithPgWeb();

var typesenseApiKey = builder.AddParameter("typesense-api-key", secret: true);

var typesense = builder.AddContainer("typesense", "typesense/typesense", "30.2")
    .WithArgs("--data-dir", "/data", "--api-key",typesenseApiKey, "xyz", "--enable-cors")
    .WithVolume("typesense-data", "/data")
    .WithHttpEndpoint(8108, 8108, name: "typesense");

var typesenseContainer = typesense.GetEndpoint("typesense");

var questionDb = postgres.AddDatabase("questionDb");

var rabbitmq = builder.AddRabbitMQ("messaging")
    .WithDataVolume("rabbitmq-data")
    .WithManagementPlugin(port: 15672);

var questionservice = builder.AddProject<Projects.QuestionService>("question-svc")
    .WithReference(keycloack)
    .WithReference(questionDb)
    .WithReference(rabbitmq)
    .WaitFor(keycloack)
        .WaitFor(questionDb)
        .WaitFor(rabbitmq);

var searchService = builder.AddProject<Projects.SearchService>("search-svc")
    .WithEnvironment("typesense-api-key", typesenseApiKey)
    .WithReference(typesenseContainer)
    .WithReference(rabbitmq)
    .WaitFor(typesense)
    .WaitFor(rabbitmq);

builder.Build().Run();