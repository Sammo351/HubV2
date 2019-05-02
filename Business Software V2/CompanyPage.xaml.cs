using System;
using System.ComponentModel;
using System.Windows;

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

        public CompanyPage()
        {
            InitializeComponent();
            EditingLabel.Visibility = Visibility.Collapsed;
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
    }
}
