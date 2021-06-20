using Business_Software_V2.Email;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;

namespace Business_Software_V2
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        void App_Startup(object sender, StartupEventArgs e)
        {
            // Application is running 
            // Process command line args 
            ApplicationSetup.Setup();

            // Create main application window, starting minimized if specified
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Timer timer = new Timer(15000);
            timer.AutoReset = true;
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }

        private async void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            await Task.Run(() => EmailProcessing.ShowPop3Subjects());
            
        }
    }
}
