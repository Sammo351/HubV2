using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Business_Software_V2
{
    /// <summary>
    /// Interaction logic for JobSitePage.xaml
    /// </summary>
    public partial class JobSitePage : Page
    {
        public JobSitePage()
        {
            InitializeComponent();

            string reader = File.ReadAllText(JobData.JobDirectory + "/Test.job");
            this.DataContext = JsonConvert.DeserializeObject<JobData>(reader);
            System.Windows.MessageBox.Show(((JobData)DataContext).guid.ToString());
            
        }

        private void WorkingDrawings_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
           
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Multiselect = true
            };

            var response = openFileDialog.ShowDialog();
            if (response == DialogResult.OK)
            {
                string[] files = openFileDialog.FileNames;
                ProcessFile(files);
            }
        }

        void ProcessFile(string[] files) { }
    }
}
