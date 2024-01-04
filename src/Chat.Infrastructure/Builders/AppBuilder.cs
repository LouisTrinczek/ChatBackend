using Chat.API.Hubs;

namespace Chat.Infrastructure.Builders;

public class AppBuilder
{
    public static void Build(WebApplication app)
    {
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.MapHub<ChatHub>("/chat");

        app.UseHttpsRedirection();
        app.UseWebSockets();
        app.MapControllers();
        app.UseAuthentication();
        app.UseAuthorization();
    }
}