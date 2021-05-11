using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Business_Software_V2
{
    /// <summary>
    /// Interaction logic for InvoiceList.xaml
    /// </summary>
    public partial class JobList : Page
    {
        public JobList()
        {

            InitializeComponent();
            var stop = Stopwatch.StartNew();
            DataContext = this;
            PopulateJobs();
            listBoxInvoice.ItemsSource = Jobs;
            Loaded += JobList_Loaded;
            stop.Stop();

            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(listBoxInvoice.ItemsSource);
            view.Filter = ListFilter;

        }


        private void JobList_Loaded(object sender, RoutedEventArgs e)
        {
            Searchbar.Focus();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(listBoxInvoice.ItemsSource).Refresh();
        }

        List<DataInvoice> UniqueCompanies = new List<DataInvoice>();

        private void RepopulateJobs()
        {
            Jobs.Clear();
            PopulateJobs();
        }

        public ObservableCollection<JobData> Jobs = new ObservableCollection<JobData>();
        public void PopulateJobs()
        {
            foreach (JobData inv in JobHelper.GetAllJobs())
                Jobs.Add(inv);
        }

        private void RefreshButton(object sender, RoutedEventArgs e)
        {
            RepopulateJobs();
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
            return SearchBarFilter(item);
        }

        private bool SearchBarFilter(object item)
        {
            if (string.IsNullOrEmpty(Searchbar.Text))
                return true;
            else
                return ((item as JobData).Address.ToString().IndexOf(Searchbar.Text, StringComparison.OrdinalIgnoreCase) >= 0);

        }

        private void Searchbar_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(listBoxInvoice.ItemsSource).Refresh();
        }

        private void OnFileClicked(object sender, MouseButtonEventArgs e)
        {
            //OPEN JOB PAGE
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
                //Delete Jobs
                RepopulateJobs();
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

