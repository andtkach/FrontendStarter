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

app.Run();

public partial class Program { }
