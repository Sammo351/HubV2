using Business_Software_V2.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Software_V2
{
    public static class InvoiceHelper
    {
        public static Action OnInvoiceAdded;

        public static DataInvoice[] GetAllInvoices()
        {
            List<DataInvoice> invoices = new List<DataInvoice>();
            string[] abns = ABNHelper.GetAllABNs();
            foreach (string abn in abns)
            {
                string dir = abn + @"\Invoices";

                string[] Invoices = Directory.GetFiles(dir);
                foreach(string s in Invoices)
                {
                    DataInvoice invoice = new DataInvoice();
                    invoice.ABN = Path.GetFileName(abn);
                    invoice.FilePath = s;
                    invoices.Add(invoice);
                }
            }

            return invoices.ToArray();
        }
    }
}
