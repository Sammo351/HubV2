using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Business_Software_V2
{
    /// <summary>
    /// Interaction logic for CompanyPage.xaml
    /// </summary>
    public partial class CompanyPage : Window, INotifyPropertyChanged
    {
        private bool _readOnly = true;
        public bool ReadOnly
        {
            set { _readOnly = value; OnPropertyChanged("ReadOnly"); }
            get { return _readOnly; }
        }

        ObservableCollection<DataInvoice> CompanyInvoices = new ObservableCollection<DataInvoice>();

        public CompanyPage()
        {
            InitializeComponent();
            EditingLabel.Visibility = Visibility.Collapsed;
            Loaded += CompanyPage_Loaded;
          
        }

        private void CompanyPage_Loaded(object sender, RoutedEventArgs e)
        {
            PopulateInvoices();
        }

        void PopulateInvoices()
        {
            CompanyInvoices = new ObservableCollection<DataInvoice>();
            foreach (DataInvoice inv in InvoiceHelper.GetAllInvoices())
            {
                if (inv.CompanyName == ((DataCompany)DataContext).CompanyName)
                    CompanyInvoices.Add(inv);
            }
            ListViewInvoices.ItemsSource = CompanyInvoices;

            CollectionViewSource.GetDefaultView(ListViewInvoices.ItemsSource).Refresh();
        }

        

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            ReadOnly = !ReadOnly;
            EditingLabel.Visibility = ReadOnly ? Visibility.Collapsed : Visibility.Visible;
            if(ReadOnly)
            {
                var r = MessageBox.Show("Are you sure you wish to make changes to the company? This cannot be undone.", "Are you sure?", MessageBoxButton.OKCancel);
                if(r == MessageBoxResult.OK)
                {
                    var dataCompany = (DataCompany)DataContext;
                    dataCompany.ABN = TextboxABN.Text;
                    dataCompany.CompanyName = TextboxCompanyName.Text;
                    dataCompany.OfficeNumber = TextboxOfficeNumber.Text;
                    CompanyHelper.SaveChanges(dataCompany);
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private void AddContactButton_Click(object sender, RoutedEventArgs e)
        {
            DataContact contact = new DataContact(TextContactName.Text, TextContactEmail.Text, TextContactPhone.Text);
            ((DataCompany)DataContext).Contacts.Add(contact);
            TextContactName.Clear();
            TextContactEmail.Clear();
            TextContactPhone.Clear();
        }

        private void SaveAllButton_Click(object sender, RoutedEventArgs e)
        {
            CompanyHelper.SaveChanges((DataCompany)DataContext);
        }

        private void RemoveContactButton_Click(object sender, RoutedEventArgs e)
        {
            var ds = ListviewContacts.SelectedItems;
            if (ds == null)
                return;

            string message = ds.Count == 1 ? "Are you sure you wish to delete this item?" : $"Are you sure you wish to delete {ds.Count} items?";
            var response = MessageBox.Show(message, "Are you sure?", MessageBoxButton.YesNo);
            if (response == MessageBoxResult.Yes)
            {
                DataCompany company = ((DataCompany)DataContext);
                company.Contacts.Remove((DataContact)ListviewContacts.SelectedItem);

            }
        }

        private void RemoveInvoiceContextMenu_OnClick(object sender, RoutedEventArgs e)
        {

        }

        private void OnFileClicked(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var d = (DataInvoice)((Label)sender).DataContext;
            Process.Start(d.FilePath);
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
            ICollectionView dataView = CollectionViewSource.GetDefaultView(ListViewInvoices.ItemsSource);
            dataView.SortDescriptions.Clear();
            SortDescription sd = new SortDescription(sortBy, direction);
            dataView.SortDescriptions.Add(sd);
            dataView.Refresh();
        }
    }
}
