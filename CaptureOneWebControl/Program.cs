using CaptureOneAutomation;

namespace CaptureOneWebControl
{
    public static class Program
    {
        private static void Main(string[] args)
        {
            AspNetServer.StartServer();
        }
    }

    public static class AspNetServer
    {
        public static WebApplication CreateWebApplication()
        {
            var builder = WebApplication.CreateBuilder();

            builder.Services.AddControllersWithViews();
            builder.Services.AddSingleton<Automator>();

            var app = builder.Build();

            app.UseHttpsRedirection();

            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseRouting();

            app.MapFallbackToFile("index.html");
            app.MapControllers();

            return app;
        }

        public static void StartServer()
        {
            CreateWebApplication().Run();
        }
    }
}