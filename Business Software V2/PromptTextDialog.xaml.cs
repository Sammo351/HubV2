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
    /// Interaction logic for PromptTextDialog.xaml
    /// </summary>
    public partial class PromptTextDialog : Window
    {
        public PromptTextDialog()
        {
            InitializeComponent();
        }
        
        public void SetCaption(string c)
        {
            CaptionLabel.Content = c;
        }

        public string Text;
        private void text_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter || e.Key == Key.Return)
            {
                Text = text.Text;
                DialogResult = true;
            }
        }
    }
}
