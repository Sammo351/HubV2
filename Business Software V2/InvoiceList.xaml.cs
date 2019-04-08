using Business_Software_V2.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
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
    /// Interaction logic for InvoiceList.xaml
    /// </summary>
    public partial class InvoiceList : Page
    {
        public InvoiceList()
        {
            
            InitializeComponent();
            var stop = Stopwatch.StartNew();
            DataContext = this;
            PopulateInvoices();
            listBoxInvoice.ItemsSource = Invoices;
            InvoiceHelper.OnInvoiceAdded += RepopulateInvoices;
            stop.Stop();
            MessageBox.Show(stop.Elapsed.TotalSeconds.ToString());
        }

        private void RepopulateInvoices()
        {
            Invoices.Clear();
            PopulateInvoices();
        }

        public ObservableCollection<DataInvoice> Invoices = new ObservableCollection<DataInvoice>();
        public void PopulateInvoices()
        {
            foreach (DataInvoice inv in InvoiceHelper.GetAllInvoices())
                Invoices.Add(inv);
            
        }
    }
}
