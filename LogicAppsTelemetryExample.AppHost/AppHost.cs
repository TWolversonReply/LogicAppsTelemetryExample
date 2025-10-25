using System.ComponentModel;
using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

var storage = builder.AddAzureStorage("storage")
    .RunAsEmulator( azurite =>
                     {
                         azurite.WithBlobPort(10000)
                                .WithQueuePort(10001)
                                .WithTablePort(10002);
                     });

var blobs = storage.AddBlobContainer("blobs");
var queue = storage.AddQueues("queue");
var table = storage.AddTables("table");

var logicapps = builder.AddExecutable("logicapps", Environment.ExpandEnvironmentVariables(@"%userprofile%\.azurelogicapps\dependencies\FuncCoreTools\func.exe"), "..\\LogicAppsTelemetryExample", args: ["host", "start", "--runtime", "inproc8", "--language-worker", "dotnet"])
    .WithEnvironment("PATH", $"%PATH%;{Environment.ExpandEnvironmentVariables(@"%userprofile%\.azurelogicapps\dependencies\NodeJs\")};{Environment.ExpandEnvironmentVariables(@"%userprofile%\.azurelogicapps\dependencies\DotNetSDK\")}")
    .WithEnvironment("OTEL_EXPORTER_OTLP_ENDPOINT", "https://localhost:21265")
    .WithEnvironment("OTEL_SERVICE_NAME", "LogicAppsStandardExample")
    .WithEnvironment("OTEL_EXPORTER_OTLP_HEADERS", $"x-otlp-api-key={builder.Configuration["AppHost:OtlpApiKey"]}")
    .WaitFor(blobs);

builder.Build().Run();
