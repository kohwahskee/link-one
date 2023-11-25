using CaptureOneAutomation;
using CaptureOneWebControl;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace CaptureOneWindowControl
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Automator automator;

        public MainWindow()
        {
            //Program.StartServer().GetAwaiter().GetResult();
            //InitializeComponent();
        }

        private void LiveViewButtonHandler(object sender, RoutedEventArgs e)
        {
            automator.InvokeLiveView();
        }
    }
}
