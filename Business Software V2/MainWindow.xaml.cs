using System.Windows;

namespace Business_Software_V2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //GenericFrameWindow genericFrame = new GenericFrameWindow();
            //genericFrame.frame.Source = new Uri(@"/Business Software V2;component/AddInvoice.xaml", UriKind.Relative);
            //genericFrame.Show();

            //GenericFrameWindow genericFrame2 = new GenericFrameWindow();
            //genericFrame2.frame.Source = new Uri(@"/Business Software V2;component/InvoiceList.xaml", UriKind.Relative);
            //genericFrame2.Show();
        }



        private void Menu_Exit(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
