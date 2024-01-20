using System.Text;
using BFF.Data;
using BFF.Services.Auth;
using BFF.Services.Category;
using BFF.Services.People;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace BFF.Extensions;

public static class StartupExtensions
{
    public static IServiceCollection AddHttpServices(this IServiceCollection services)
    {
        services.AddHttpClient<IAuthService, AuthService>(
            (provider, client) => {
                client.BaseAddress = new Uri(provider.GetService<IConfiguration>()?[Constants.ConfigIamUrl] ?? throw new InvalidOperationException("Missing auth config"));
            });

        services.AddHttpClient<ICategoryService, CategoryService>(
            (provider, client) => {
                client.BaseAddress = new Uri(provider.GetService<IConfiguration>()?[Constants.ConfigCategoryUrl] ?? throw new InvalidOperationException("Missing category config"));
            });

        services.AddHttpClient<IPeopleService, PeopleService>(
            (provider, client) => {
                client.BaseAddress = new Uri(provider.GetService<IConfiguration>()?[Constants.ConfigPeopleUrl] ?? throw new InvalidOperationException("Missing people config"));
            });


        return services;
    }

    public static IServiceCollection AddSwaggerServices(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            var jwtSecurityScheme = new OpenApiSecurityScheme
            {
                BearerFormat = "JWT",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                Description = "Put Bearer + your token in the box below",

                Reference = new OpenApiReference
                {
                    Id = JwtBearerDefaults.AuthenticationScheme,
                    Type = ReferenceType.SecurityScheme
                }
            };

            c.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                { jwtSecurityScheme, Array.Empty<string>() }
            });
        });

        return services;
    }
    
    public static IServiceCollection AddAuthenticationServices(this IServiceCollection services,
        ConfigurationManager configuration)
    {
        var audience = configuration.GetValue<string>(Constants.ConfigAudience);
        var authority = configuration.GetValue<string>(Constants.ConfigAuthority);

        if (string.IsNullOrEmpty(authority)) throw new Exception("Invalid IAM Authority configuration");
        if (string.IsNullOrEmpty(audience)) throw new Exception("Invalid IAM Audience configuration");
        
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(opt =>
            {
                opt.Authority = authority;
                opt.Audience = audience;
                opt.RequireHttpsMetadata = false;
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(DemoData.TestSecret))
                };
                opt.Events = new JwtBearerEvents()
                {
                    OnMessageReceived = context =>
                    {
                        Console.WriteLine("OnMessageReceived");
                        return Task.CompletedTask;
                    },
                    OnTokenValidated = context =>
                    {
                        Console.WriteLine("OnTokenValidated");
                        return Task.CompletedTask;
                    },
                    OnAuthenticationFailed = context =>
                    {
                        Console.WriteLine("OnAuthenticationFailed");
                        return Task.CompletedTask;
                    },
                };
            });
        
        services.AddAuthorization();

        return services;
    }
    
    public static IServiceCollection AddMockServices(this IServiceCollection services)
    {
        //services.AddTransient<IProductService, ProductService>();

        return services;
    }
}