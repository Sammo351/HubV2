using Business_Software_V2.Data;
using System.Windows;

namespace Business_Software_V2
{
    /// <summary>
    /// Interaction logic for NewABN.xaml
    /// </summary>
    public partial class NewABN : Window
    {
        public NewABN()
        {
            InitializeComponent();
        }

        public string InvoiceFile { get; internal set; }  

        private void AcceptNewABN(object sender, RoutedEventArgs e)
        {
            var data = ((ABNData)DataContext);
            ABNHelper.SetupInfoFile(data);
            ABNHelper.UpdatedBusinessCard();
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (InvoiceFile != null)
                pdfViewer.Navigate(InvoiceFile);
        }
    }

}
