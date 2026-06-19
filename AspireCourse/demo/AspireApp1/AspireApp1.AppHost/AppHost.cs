#pragma warning disable ASPIRECERTIFICATES001
var builder = DistributedApplication.CreateBuilder(args);

var keycloack = builder.AddKeycloak("keycloack", 6001)
    .WithoutHttpsCertificate()
    .WithDataVolume("Keycloack-data");

var postgres = builder.AddPostgres("postgres", port: 5432)
    .WithDataVolume("postgres-data")
    .WithImage("postgres","18")
    .WithPgWeb();

var questionDb = postgres.AddDatabase("questionDb");

var questionservice = builder.AddProject<Projects.QuestionService>("question-svc")
    .WithReference(keycloack)
    .WithReference(questionDb)
    .WaitFor(keycloack)
    .WaitFor(keycloack);


builder.Build().Run();