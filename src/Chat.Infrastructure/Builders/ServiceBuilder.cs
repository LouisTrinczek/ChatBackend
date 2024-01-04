using System.Text;
using Chat.Application.Security;
using Chat.Application.Services;
using Chat.Common.Types;
using Chat.Infrastructure.Interfaces.Swagger;
using Chat.Persistence.Context;
using Chat.Persistence.Interfaces;
using Chat.Persistence.Repositories;
using EntityFramework.Exceptions.MySQL;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Chat.Infrastructure.Builders;

public static class ServiceBuilder
{
    public static void Build(WebApplicationBuilder builder)
    {
        string? databaseConnectionString = builder.Configuration.GetConnectionString("Database");

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddRouting(options => options.LowercaseUrls = true);
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc(
                "v1",
                new OpenApiInfo()
                {
                    Title = "Chat API",
                    Description =
                        @"
Welcome to the Chat Application API, a RESTful interface for building a real-time chat platform. This API allows you to manage users, create servers, channels, and send messages seamlessly. Leverage the power of real-time communication to enhance your chat experience.

### This Project is Open Source. Feel free to Contribute on <a href='https://github.com/LouisTrinczek/chat-backend'>Github</a>

## Features

- **User Management**: Register and Update users to participate in the chat.
- **Servers**: Establish chat servers for users to join.
- **Channel**: Define channels within servers for focused discussions.
- **Messages**: Send real-time messages within specific channels.

## Authentication

Authentication is required for certain endpoints to ensure secure communication. Use the appropriate authentication headers when making requests.

## Error Responses

The API returns standard HTTP status codes for success and error cases. Check the response body for detailed error messages.

## Explore the API

Use the interactive Swagger documentation below to explore and test the available endpoints. Make sure to authenticate where necessary and follow the provided examples.
",
                    Contact = new OpenApiContact()
                    {
                        Name = "Louis Trinczek",
                        Email = "trinczeklouis@gmail.com",
                        Url = new Uri("https://github.com/LouisTrinczek")
                    },
                    Version = "v1"
                }
            );

            c.OperationFilter<RemoveVersionParameterFilter>();
            c.DocumentFilter<ReplaceVersionWithExactValueInPathFilter>();
            c.SchemaFilter<RequireNonNullablePropertiesSchemaFilter>();
            c.SupportNonNullableReferenceTypes();

            c.TagActionsBy(api =>
            {
                if (api.GroupName != null)
                {
                    return new[] { api.GroupName };
                }

                if (api.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor)
                {
                    return new[] { controllerActionDescriptor.ControllerName };
                }

                throw new InvalidOperationException("Unable to determine tag for endpoint.");
            });

            c.DocInclusionPredicate((name, api) => true);

            var filePath = Path.GetFullPath("../Chat.API/api.xml");
            c.IncludeXmlComments(filePath, true);
        });

        builder.Services.AddControllers();
        builder.Services.AddDbContext<ChatDataContext>(options =>
        {
            options.UseMySql(
                databaseConnectionString,
                ServerVersion.AutoDetect(databaseConnectionString)
            );
            options.UseExceptionProcessor();
        });

        builder.Services.AddApiVersioning(api =>
        {
            api.AssumeDefaultVersionWhenUnspecified = true;
            api.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
            api.ReportApiVersions = true;
            api.ApiVersionReader = ApiVersionReader.Combine(
                new QueryStringApiVersionReader("api-version"),
                new HeaderApiVersionReader("X-Version"),
                new MediaTypeApiVersionReader("ver")
            );
        });

        builder.Services.AddSignalR();
        builder.Services.PostConfigure<ApiBehaviorOptions>(apiBehaviorOptions =>
        {
            apiBehaviorOptions.InvalidModelStateResponseFactory = actionContext =>
            {
                return new BadRequestObjectResult(
                    new ApiResponse<object>(
                        ResponseStatus.Error,
                        null,
                        actionContext
                            .ModelState.Values.SelectMany(x => x.Errors)
                            .Select(x => x.ErrorMessage)
                            .ToArray()
                    )
                );
            };
        });

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(o =>
        {
            o.TokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey
                    (Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true
            };
        });
        builder.Services.AddAuthorization();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IJwtHandler, JwtHandler>();
    }
}