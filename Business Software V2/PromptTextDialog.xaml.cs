using System.Windows;
using System.Windows.Input;

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
            if (e.Key == Key.Enter || e.Key == Key.Return)
            {
                Text = text.Text;
                DialogResult = true;
            }
        }
    }
}
