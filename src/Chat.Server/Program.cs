using Chat.Server.Builders;
using Microsoft.AspNetCore.Builder;

namespace Chat.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            ServiceBuilder.Build(builder);

            var app = builder.Build();

            AppBuilder.Build(app);

            app.Run();
        }
    }
}
