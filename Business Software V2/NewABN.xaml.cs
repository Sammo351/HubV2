using Business_Software_V2.Data;
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
using System.Windows.Shapes;

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

        private void AcceptNewABN(object sender, RoutedEventArgs e)
        {
            var data = ((ABNData) DataContext);
            ABNHelper.SetupInfoFile(data);
            ABNHelper.UpdatedBusinessCard();
            this.Close();
        }
    }
    
}
