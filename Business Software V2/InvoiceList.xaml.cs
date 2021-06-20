using Business_Software_V2.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

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
            Loaded += InvoiceList_Loaded;
            stop.Stop();

            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(listBoxInvoice.ItemsSource);
            view.Filter = ListFilter;

        }


        private void InvoiceList_Loaded(object sender, RoutedEventArgs e)
        {
            Searchbar.Focus();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(listBoxInvoice.ItemsSource).Refresh();
        }

        List<DataInvoice> UniqueCompanies = new List<DataInvoice>();

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

            UniqueCompanies.Clear();
            FilterBox.Items.Clear();
            List<string> companyNames = new List<string>();
            foreach (DataInvoice invoice in Invoices)
            {

                if (!companyNames.Contains(invoice.CompanyName))
                {
                    companyNames.Add(invoice.CompanyName);
                    UniqueCompanies.Add(invoice);

                    CheckBox checkBox = new CheckBox() { DataContext = invoice, Content = invoice.CompanyName };
                    checkBox.Checked += CheckBox_Checked;
                    checkBox.Unchecked += CheckBox_Checked;
                    FilterBox.Items.Add(checkBox);

                }

            }

        }

        private void RefreshButton(object sender, RoutedEventArgs e)
        {
            RepopulateInvoices();
            EmailProcessing.ShowPop3Subjects();
        }



        GridViewColumnHeader _lastHeaderClicked = null;
        ListSortDirection _lastDirection = ListSortDirection.Ascending;


        private void ColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader column = (e.OriginalSource as GridViewColumnHeader);
            ListSortDirection direction = ListSortDirection.Ascending;

            if (column != null)
            {
                if (column.Role != GridViewColumnHeaderRole.Padding)
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

        private bool ListFilter(object item)
        {
            return SearchBarFilter(item) && FilterBoxValidated(item);
        }

        private bool SearchBarFilter(object item)
        {
            if (string.IsNullOrEmpty(Searchbar.Text))
                return true;
            else
                return ((item as DataInvoice).CompanyName.IndexOf(Searchbar.Text, StringComparison.OrdinalIgnoreCase) >= 0)
                    || ((item as DataInvoice).ABN.IndexOf(Searchbar.Text, StringComparison.OrdinalIgnoreCase) >= 0)
                    || ((item as DataInvoice).DisplayName.IndexOf(Searchbar.Text, StringComparison.OrdinalIgnoreCase) >= 0) ||
                        (CompanyHelper.GetCompany((item as DataInvoice).ABN).Contacts.Where(a => a.Name.IndexOf(Searchbar.Text, StringComparison.OrdinalIgnoreCase) >= 0).Count() > 0);

        }

        private bool FilterBoxValidated(object item)
        {
            var i = item as DataInvoice;
            var checkboxes = FilterBox.Items.OfType<CheckBox>();

            bool areAnyCheckboxesChecked = false;
            foreach (CheckBox checkbox in checkboxes)
            {
                if (checkbox.IsChecked.Value)
                    areAnyCheckboxesChecked = true;
            }

            if (!areAnyCheckboxesChecked)
                return true;

            if ((checkboxes.Where(a => a.IsChecked.Value == true).Where(a => ((DataInvoice)a.DataContext).CompanyName == i.CompanyName)).Count() > 0)
                return true;



            return false;
        }

        private void Searchbar_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(listBoxInvoice.ItemsSource).Refresh();
        }

        private void OnFileClicked(object sender, MouseButtonEventArgs e)
        {
            var d = (DataInvoice)((Label)sender).DataContext;
            try
            {
                Process.Start(d.FilePath);
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }

        private void OnCompanyClicked(object sender, RoutedEventArgs e)
        {
            var d = (DataInvoice)((Label)sender).DataContext;
            CompanyPage window = new CompanyPage();
            window.DataContext = CompanyHelper.GetCompany(d.ABN);
            window.Owner = Application.Current.MainWindow;
            window.Show();

        }

        private void ListboxChanged(object sender, SelectionChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(listBoxInvoice.ItemsSource).Refresh();
        }

        private void ClearTextbox(object sender, RoutedEventArgs e)
        {
            Searchbar.Clear();
        }

        private void RemoveInvoiceContextMenu_OnClick(object sender, RoutedEventArgs e)
        {
            var ds = listBoxInvoice.SelectedItems;
            if (ds == null)
                return;

            string message = ds.Count == 1 ? "Are you sure you wish to delete this item?" : $"Are you sure you wish to delete {ds.Count} items?";
            var response = MessageBox.Show(message, "Are you sure?", MessageBoxButton.YesNo);
            if (response == MessageBoxResult.Yes)
            {
                foreach (DataInvoice inv in ds)
                    File.Delete(inv.FilePath);

                RepopulateInvoices();


            }
        }

        private void Searchbar_Keydown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Down)
            {
                if (listBoxInvoice.SelectedIndex + 1 < listBoxInvoice.Items.Count)
                    listBoxInvoice.SelectedIndex++;

            }
            if (e.Key == Key.Up)
            {
                if (listBoxInvoice.SelectedIndex - 1 >= 0)
                    listBoxInvoice.SelectedIndex--;

            }

            if (e.Key == Key.F6)
            {
                e.Handled = false;
                ((AddInvoice)addInvoiceFrame.Content).UploadInvoicesButton_Click(sender, e);
            }

            if (e.Key == Key.F5)
            {
                e.Handled = false;
                RefreshButton(sender, e);
            }


            if (e.Key == Key.Return || e.Key == Key.Enter)
            {
                int index = listBoxInvoice.SelectedIndex;
                if (index == -1 && listBoxInvoice.Items.Count > 0)
                    index = 0;

                if (listBoxInvoice.Items.GetItemAt(index) != null)
                {

                    DataInvoice i = listBoxInvoice.Items.GetItemAt(index) as DataInvoice;
                    Process.Start(i.FilePath);
                }
                else
                    MessageBox.Show("Its null!");
            }
        }

    }
}

