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
    .WaitFor(blobs);

builder.Build().Run();
