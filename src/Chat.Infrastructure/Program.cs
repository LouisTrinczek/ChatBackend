using Chat.Infrastructure.Builders;

namespace Chat.Infrastructure
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