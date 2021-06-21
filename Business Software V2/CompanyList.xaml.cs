using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Business_Software_V2
{
    /// <summary>
    /// Interaction logic for CompanyList.xaml
    /// </summary>
    public partial class CompanyList : Page
    {
        public CompanyList()
        {
            InitializeComponent();
            GetAllCompanies();
            DataContext = this;
        }

        public async void GetAllCompanies()
        {
            DataCompany[] allCompanies = await CompanyHelper.GetAllCompanies();
            for (int i = 0; i < allCompanies.Length; i++)
            {
                DataCompany c = allCompanies[i];
                Companies.Add(c);
            }

            ListboxCompanies.ItemsSource = Companies;
        }

        public ObservableCollection<DataCompany> Companies = new ObservableCollection<DataCompany>();

        private void RemoveCompanyContextMenu_OnClick(object sender, RoutedEventArgs e)
        {

        }

        private void ColumnHeader_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ListboxCompanies_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListviewContacts.DataContext = ListboxCompanies?.SelectedItem;
        }

        private void ListboxCompanies_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            CompanyPage companyPage = new CompanyPage
            {
                DataContext = ((DataCompany)ListboxCompanies.SelectedItem)
            };
            companyPage.Show();
            companyPage.Owner = Application.Current.MainWindow;


        }
    }
}
