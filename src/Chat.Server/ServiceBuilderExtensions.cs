using System.Text;
using Chat.Application.Contracts.Repositories;
using Chat.Application.Contracts.Security;
using Chat.Application.Contracts.Services;
using Chat.Application.Security;
using Chat.Application.Services;
using Chat.Common.Types;
using Chat.Infrastructure.Database;
using Chat.Infrastructure.Database.Repositories;
using Chat.Infrastructure.Interfaces.Swagger;
using EntityFramework.Exceptions.MySQL.Pomelo;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Chat.Server;

public static class ServiceBuilderExtensions
{
    public static WebApplicationBuilder AddChat(this WebApplicationBuilder self)
    {
        return self.AddDatabase()
            .AddLogging()
            .AddDependencyInjection()
            .AddAuthentication()
            .AddSwagger()
            .AddApi();
    }

    public static WebApplicationBuilder AddLogging(this WebApplicationBuilder self)
    {
        self.Services.AddLogging(log =>
        {
            log.AddConsole();
        });
        return self;
    }

    public static WebApplicationBuilder AddApi(this WebApplicationBuilder self)
    {
        self.Services.AddEndpointsApiExplorer();
        self.Services.AddRouting(options => options.LowercaseUrls = true);
        self.Services.AddApiVersioning(api =>
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
        self.Services.AddControllers();
        self.Services.AddSignalR();

        self.Services.PostConfigure<ApiBehaviorOptions>(apiBehaviorOptions =>
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
        return self;
    }

    public static WebApplicationBuilder AddDependencyInjection(this WebApplicationBuilder self)
    {
        self.Services.AddScoped<IUserService, UserService>();
        self.Services.AddScoped<IUserRepository, UserRepository>();
        self.Services.AddScoped<IMessageService, MessageService>();
        self.Services.AddScoped<IMessageRepository, MessageRepository>();
        self.Services.AddScoped<IServerService, ServerService>();
        self.Services.AddScoped<IServerRepository, ServerRepository>();
        self.Services.AddScoped<IChannelService, ChannelService>();
        self.Services.AddScoped<IChannelRepository, ChannelRepository>();
        self.Services.AddScoped<IFriendRepository, FriendRepository>();
        self.Services.AddScoped<IFriendsService, FriendsService>();
        self.Services.AddScoped<IJwtHandler, JwtHandler>();
        self.Services.AddHttpContextAccessor();
        self.Services.AddScoped<Lazy<IChannelService>>(
            provider => new Lazy<IChannelService>(provider.GetRequiredService<IChannelService>)
        );
        self.Services.AddScoped<Lazy<IServerService>>(
            provider => new Lazy<IServerService>(provider.GetRequiredService<IServerService>)
        );
        return self;
    }

    public static WebApplicationBuilder AddDatabase(this WebApplicationBuilder self)
    {
        string? databaseConnectionString = self.Configuration.GetConnectionString("Database");
        self.Services.AddDbContext<ChatDataContext>(options =>
        {
            options.UseMySql(
                databaseConnectionString,
                ServerVersion.AutoDetect(databaseConnectionString)
            );
            options.UseExceptionProcessor();
        });
        return self;
    }

    public static WebApplicationBuilder AddAuthentication(this WebApplicationBuilder self)
    {
        self.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            .AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = self.Configuration["Jwt:Issuer"],
                    ValidAudience = self.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(self.Configuration["Jwt:Key"] ?? "")
                    ),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true
                };
            });
        self.Services.AddAuthorization();
        return self;
    }

    public static WebApplicationBuilder AddSwagger(this WebApplicationBuilder self)
    {
        self.Services.AddSwaggerGen(c =>
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

        return self;
    }
}
