using Business_Software_V2.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Business_Software_V2
{
    class DataProcessing
    {
        

        public static void ProcessInvoices(params string[] files)
        {
            //textBlock.Text = results[0].Email;
            ProcessedInvoice[] results = null;
            Stopwatch stopwatch = Stopwatch.StartNew();
            stopwatch.Start();
            

            var t = Task.Run(() =>
            {
                results = InvoiceProcessor.Process(files);
                Application.Current.Dispatcher.Invoke(() =>
                {
                    stopwatch.Stop();
                    foreach (ProcessedInvoice inv in results)
                    {
                        // textBlock.Text += $"{inv.ABN} : {inv.Email} | GST: {inv.GstRegistered} \n";
                        TryABN(inv);
                    }
                    //textBlock.Text += "\n Time: " + stopwatch.Elapsed;
                
                });
            });
        }

        static void TryABN(ProcessedInvoice inv)
        {
            if (String.IsNullOrEmpty(inv.ABN))
            {
                PromptTextDialog promptTextDialog = new PromptTextDialog();
                promptTextDialog.SetCaption("Can't find ABN, please specify $$");
                promptTextDialog.ShowDialog();
                inv.ABN = promptTextDialog.Text;
            }


            if (!ABNHelper.DoesABNExist(inv.ABN))
            {
                string text = $"ABN Not found: {inv.ABN}, would you like to create a new folder for {inv.ABN}?";
                var response = MessageBox.Show(text, "ABN not found", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes);

                if (response == MessageBoxResult.Yes)
                {
                    NewABN newABN = new NewABN();
                    newABN.DataContext = new ABNData() { ABN = inv.ABN, Email = inv.Email, Phone = inv.Phone, CompanyName = inv.CompanyName };
                    newABN.Show();
                }
            }

            string path = ABNHelper.GetDirectory(inv.ABN);
            string iPath = path + @"\Invoices\";


            ABNHelper.CreateDirectoryFromPath(iPath);

            string tPath = iPath + System.IO.Path.GetFileName(inv.FilePath);

            if (!File.Exists(tPath))
                File.Copy(inv.FilePath, tPath, false);

            InvoiceHelper.OnInvoiceAdded();

        }
    }
}
