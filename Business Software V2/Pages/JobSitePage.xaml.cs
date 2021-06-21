using Newtonsoft.Json;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;

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

            //string reader = File.ReadAllText(DataJob.JobDirectory + "/Test.job");
            //this.DataContext = JsonConvert.DeserializeObject<DataJob>(reader);
            //System.Windows.MessageBox.Show(((DataJob)DataContext).guid.ToString());

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
