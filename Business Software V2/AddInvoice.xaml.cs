﻿using Business_Software_V2.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
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
using DataFormats = System.Windows.DataFormats;
using DragEventArgs = System.Windows.DragEventArgs;
using MessageBox = System.Windows.MessageBox;

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
            processingBar.Visibility = Visibility.Collapsed;
            PreviewKeyDown += AddInvoice_PreviewKeyDown;
        }

        private void AddInvoice_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.F6)
            {
                MessageBox.Show("Horay!");
                UploadInvoicesButton_Click(null, e);
            }
        }

        private void ItemsDropped(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {

                // Note that you can have more than one file.
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                // Assuming you have one file that you care about, pass it off to whatever
                // handling code you have defined.
                ProcessInvoices(files);






            }
        }

        void ProcessInvoices(string[] files)
        {
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
                        // textBlock.Text += $"{inv.ABN} : {inv.Email} | GST: {inv.GstRegistered} \n";
                        TryABN(inv);
                    }
                    //textBlock.Text += "\n Time: " + stopwatch.Elapsed;
                    processingBar.Visibility = Visibility.Visible;
                    processingBar.IsIndeterminate = false;
                    processingBar.Value = processingBar.Maximum;
                });
            });
        }

        void TryABN(ProcessedInvoice inv)
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

        public void UploadInvoicesButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Multiselect = true
            };

            var response = openFileDialog.ShowDialog();
            if (response == DialogResult.OK)
            {
                string[] files = openFileDialog.FileNames;
                ProcessInvoices(files);
            }
        }
    }
}
