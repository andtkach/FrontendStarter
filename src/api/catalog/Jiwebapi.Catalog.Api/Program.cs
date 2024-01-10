using Jiwebapi.Catalog.Api;
using Jiwebapi.Catalog.Identity;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

var builder = WebApplication.CreateBuilder(args);

Log.Information($"Jiwebapi Web API start in {builder.Environment.EnvironmentName} mode");

builder.Host.UseSerilog((context, loggerConfiguration) => loggerConfiguration
     .WriteTo.Console()
     .ReadFrom.Configuration(context.Configuration));

var app = builder
       .ConfigureServices()
       .ConfigurePipeline();

app.UseSerilogRequestLogging();

await app.ResetDataDatabaseAsync();
await app.ResetMessageDatabaseAsync();
await app.ResetIdentityDatabaseAsync();
await app.SeedUsersAsync();

VecinoClient.VecinoReporter
    .ReportMessage(app.Logger, $"JI Web API application version 1.0 started on {Environment.MachineName} at {DateTime.UtcNow}");


app.Run();

public partial class Program { }
