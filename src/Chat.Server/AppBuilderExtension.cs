using Chat.API.Hubs;

namespace Chat.Server;

public static class AppBuilderExtension
{
    public static WebApplication UseChat(this WebApplication self)
    {
        // Configure the HTTP request pipeline.
        if (self.Environment.IsDevelopment())
        {
            self.UseSwagger();
            self.UseSwaggerUI();
        }

        self.MapHub<ChatHub>("/chat");

        self.UseCors(c => c.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
        self.UseRouting();
        self.UseHttpsRedirection();
        self.UseWebSockets();
        self.MapControllers();
        self.UseAuthentication();
        self.UseAuthorization();
        return self;
    }
}
