using Business_Software_V2.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
            ABNHelper.UpdatedBusinessCard += RepopulateInvoices;

            stop.Stop();
            
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(listBoxInvoice.ItemsSource);
            view.Filter = SearchBarFilter;
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

        private void RefreshButton(object sender, RoutedEventArgs e)
        {
            RepopulateInvoices();
        }

        private void OnCompanyClicked(object sender, RoutedEventArgs e)
        {
            var d = (DataInvoice)((Button)sender).DataContext;
            MessageBox.Show(d.ABN);

        }

        GridViewColumnHeader _lastHeaderClicked = null;
        ListSortDirection _lastDirection = ListSortDirection.Ascending;


        private void ColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader column = (e.OriginalSource as GridViewColumnHeader);
            ListSortDirection direction = ListSortDirection.Ascending;

            if(column != null)
            {
                if(column.Role != GridViewColumnHeaderRole.Padding)
                {
                    if (column != _lastHeaderClicked)
                        direction = ListSortDirection.Ascending;
                    else
                    {
                        if (_lastDirection == ListSortDirection.Ascending)
                            direction = ListSortDirection.Descending;
                        else
                            direction = ListSortDirection.Ascending;
                    }
                }

                string header = column.Tag.ToString();
                Sort(header, direction);
                _lastHeaderClicked = column;
                _lastDirection = direction;
            }
            
        }

        private void Sort(string sortBy, ListSortDirection direction)
        {
            ICollectionView dataView = CollectionViewSource.GetDefaultView(listBoxInvoice.ItemsSource);
            dataView.SortDescriptions.Clear();
            SortDescription sd = new SortDescription(sortBy, direction);
            dataView.SortDescriptions.Add(sd);
            dataView.Refresh();
        }

        private bool SearchBarFilter(object item)
        {
            if (string.IsNullOrEmpty(Searchbar.Text))
                return true;
            else
                return ((item as DataInvoice).CompanyName.IndexOf(Searchbar.Text, StringComparison.OrdinalIgnoreCase) >= 0)
                    || ((item as DataInvoice).ABN.IndexOf(Searchbar.Text, StringComparison.OrdinalIgnoreCase) >= 0)
                    || ((item as DataInvoice).DisplayName.IndexOf(Searchbar.Text, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        private void Searchbar_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(listBoxInvoice.ItemsSource).Refresh();
        }
    }
}
