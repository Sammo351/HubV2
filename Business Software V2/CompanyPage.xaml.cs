using Business_Software_V2.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    }
}
