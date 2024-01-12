namespace Chat.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.AddChat();

            var app = builder.Build();

            app.UseChat();
            app.Run();
        }
    }
}
