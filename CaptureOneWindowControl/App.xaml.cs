using CaptureOneAutomation;
using CaptureOneWebControl;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace CaptureOneWindowControl
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public IServiceProvider ServiceProvider { get; }
        private readonly IServiceScope? _serviceScope;
        private readonly WebApplication _host;

        public App()
        {
            //var builder = WebApplication.CreateBuilder();
            //builder.Services.AddSingleton<MainWindow>();
            //builder.Services.AddControllersWithViews();

            //_host = builder.Build();

            //_host.UseHttpsRedirection();
            //_host.MapGet("/", () => "Hello World");

            //_serviceScope = _host.Services.CreateScope();
            //ServiceProvider = _serviceScope.ServiceProvider;
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            AspNetServer.StartServer();
            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();

            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            _serviceScope?.Dispose();
            await _host.StopAsync();
            await _host.DisposeAsync();
            base.OnExit(e);
        }
    }
}
