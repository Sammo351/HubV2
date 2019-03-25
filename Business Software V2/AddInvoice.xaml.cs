using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
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
    /// Interaction logic for AddInvoice.xaml
    /// </summary>
    public partial class AddInvoice : Page
    {
        public AddInvoice()
        {
            InitializeComponent();
        }

        private void ItemsDropped(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                
                // Note that you can have more than one file.
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                // Assuming you have one file that you care about, pass it off to whatever
                // handling code you have defined.

                //textBlock.Text = results[0].Email;
                ProcessedInvoice[] results = null;
                Stopwatch stopwatch = Stopwatch.StartNew();
                stopwatch.Start();
                processingBar.IsIndeterminate = true;
                processingBar.Visibility = Visibility.Visible;

                var t = Task.Run(() =>
                {
                    results = InvoiceProcessor.Process(files);
                    this.Dispatcher.Invoke(() =>
                    {
                        stopwatch.Stop();
                        foreach (ProcessedInvoice inv in results)
                        {
                            textBlock.Text += $"{inv.ABN} : {inv.Email} | GST: {inv.GstRegistered} \n";
                        }
                        textBlock.Text += "\n Time: " + stopwatch.Elapsed;
                        processingBar.Visibility = Visibility.Visible;
                        processingBar.IsIndeterminate = false;
                        processingBar.Value = processingBar.Maximum;
                    });
                });


                Task.Run(() => { /* Threaded Code */ });


            }
        }


    }
}
