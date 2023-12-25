using System.Diagnostics;
using Chat.Infrastructure.Interfaces.Swagger;
using Microsoft.EntityFrameworkCore;
using Chat.Persistence.Context;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.OpenApi.Models;

namespace Chat.Infrastructure
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            string? connectionString = builder.Configuration.GetConnectionString("Database");

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new OpenApiInfo()
                    {
                        Title = "Chat API",
                        Description = @"
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
                            Name = "Louis Trinczek", Email = "trinczeklouis@gmail.com",
                            Url = new Uri("https://github.com/LouisTrinczek")
                        },
                        Version = "v1"
                    }
                );

                c.OperationFilter<RemoveVersionParameterFilter>();
                c.DocumentFilter<ReplaceVersionWithExactValueInPathFilter>();
                
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
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
                    .EnableDetailedErrors()
                    .LogTo(Console.WriteLine, LogLevel.Information)
            );
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

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.MapControllers();
            app.Run();
        }
    }
}