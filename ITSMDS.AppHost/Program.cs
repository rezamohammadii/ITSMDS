using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);


var sql = builder.AddSqlServer("sql")
    .WithDataVolume()
    .AddDatabase("itsmds");
var apiService = builder.AddProject<Projects.ITSMDS_ApiService>("apiservice")
    .WithReference(sql)
    ;

builder.AddProject<Projects.ITSMDS_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService);

builder.Build().Run();
