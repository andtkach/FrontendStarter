using Jiwebapi.Catalog.Api.BackgroundServices;
using Jiwebapi.Catalog.Api.Middleware;
using Jiwebapi.Catalog.Api.Services;
using Jiwebapi.Catalog.Api.Utility;
using Jiwebapi.Catalog.Application;
using Jiwebapi.Catalog.Application.Contracts;
using Jiwebapi.Catalog.Cache;
using Jiwebapi.Catalog.History;
using Jiwebapi.Catalog.Identity;
using Jiwebapi.Catalog.Message;
using Jiwebapi.Catalog.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace Jiwebapi.Catalog.Api
{
    public static class StartupExtensions
    {
        public static WebApplication ConfigureServices(
        this WebApplicationBuilder builder)
        {
            AddSwagger(builder.Services);
            
            builder.Services.AddApplicationServices();
            builder.Services.AddPersistenceServices(builder.Configuration);
            builder.Services.AddMessageServices(builder.Configuration);
            builder.Services.AddIdentityServices(builder.Configuration);
            builder.Services.AddCacheServices(builder.Configuration);
            builder.Services.AddHistoryServices(builder.Configuration);

            builder.Services.AddScoped<ILoggedInUserService, LoggedInUserService>();

            builder.Services.AddHttpContextAccessor();

            builder.Services.AddSingleton<ContentCacheProcessingChannel>();
            builder.Services.AddHostedService<TimeCacheService>();
            builder.Services.AddHostedService<RequestCacheService>();
            builder.Services.AddHostedService<RequestHistoryService>();

            builder.Services.AddControllers();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("Open", builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            });

            return builder.Build();

        }

        public static WebApplication ConfigurePipeline(this WebApplication app)
        {

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Jiwebapi Management API");
                });
            }

#pragma warning disable S125
            //app.UseHttpsRedirection();
#pragma warning restore S125
            //app.UseRouting();
            
            app.UseAuthentication();

            app.UseHistory();
            app.UseCustomExceptionHandler();

            app.UseCors("Open");

            app.UseAuthorization();

            app.MapControllers();

            return app;

        }
        private static void AddSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                  {
                    {
                      new OpenApiSecurityScheme
                      {
                        Reference = new OpenApiReference
                          {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                          },
                          Scheme = "oauth2",
                          Name = "Bearer",
                          In = ParameterLocation.Header,

                        },
                        new List<string>()
                      }
                    });

                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Jiwebapi Management API",

                });

                c.OperationFilter<FileResultContentTypeOperationFilter>();
            });
        }

        public static async Task ResetDataDatabaseAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            try
            {
                var context = scope.ServiceProvider.GetService<CatalogDbContext>();
                if (context != null)
                {
#pragma warning disable S125
                    //await context.Database.EnsureDeletedAsync();
#pragma warning restore S125
                    await context.Database.MigrateAsync();
                }
            }
            catch (Exception ex)
            {
                ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
                {
                    builder.AddConsole();
                    builder.AddDebug();
                });
                
                var logger = loggerFactory.CreateLogger<WebApplication>();
                logger.LogError(ex, "An error occurred while migrating the database.");
                Environment.Exit(1);
            }
        }

        public static async Task ResetMessageDatabaseAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            try
            {
                var context = scope.ServiceProvider.GetService<MessageDbContext>();
                if (context != null)
                {
#pragma warning disable S125
                    //await context.Database.EnsureDeletedAsync();
#pragma warning restore S125
                    await context.Database.MigrateAsync();
                }
            }
            catch (Exception ex)
            {
                ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
                {
                    builder.AddConsole();
                    builder.AddDebug();
                });
                
                var logger = loggerFactory.CreateLogger<WebApplication>();
                logger.LogError(ex, "An error occurred while migrating the messages database.");
            }
        }
    }
}