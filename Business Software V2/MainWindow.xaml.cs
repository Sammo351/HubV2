using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
            mainFrame.Navigate(new Uri(@"/Business Software V2;component/InvoiceList.xaml", UriKind.Relative));
            //GenericFrameWindow genericFrame = new GenericFrameWindow();
            //genericFrame.frame.Source = new Uri(@"/Business Software V2;component/AddInvoice.xaml", UriKind.Relative);
            //genericFrame.Show();

            //GenericFrameWindow genericFrame2 = new GenericFrameWindow();
            //genericFrame2.frame.Source = new Uri(@"/Business Software V2;component/InvoiceList.xaml", UriKind.Relative);
            //genericFrame2.Show();
        }


        private void Menu_Scan(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new Uri(@"/Business Software V2;component/AddInvoice.xaml", UriKind.Relative));
        }

        private void Menu_ListInvoices(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new Uri(@"/Business Software V2;component/InvoiceList.xaml", UriKind.Relative));
        }

        private void Menu_Exit(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
