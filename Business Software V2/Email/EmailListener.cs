using Business_Software_V2.Data;
using Business_Software_V2.Email;
using MailKit.Net.Pop3;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Software_V2
{
    public class EmailParser : IDisposable

    {

        protected string User { get; set; }
        protected string Pwd { get; set; }
        protected string MailServer { get; set; }
        protected int Port { get; set; }
        public Pop3Client Pop3 { get; set; }
        public EmailParser(string user, string pwd, string mailserver, int port)
        {
            User = user;
            Pwd = pwd;
            MailServer = mailserver;
            Port = port;
            Pop3 = null;
        }

        public void OpenPop3()
        {
            if (Pop3 == null)
            {
                Pop3 = new Pop3Client();
                Pop3.Connect(this.MailServer, this.Port, false);
                Pop3.AuthenticationMechanisms.Remove("XOAUTH2");
                Pop3.Authenticate(this.User, this.Pwd);
            }
        }

        public void ClosePop3()
        {
            if (Pop3 != null)
            {
                Pop3.Disconnect(true);
                Pop3.Dispose();
                Pop3 = null;
            }
        }



        public async Task DisplayPop3SubjectsAsync()
        {
            BotVisitor v = new BotVisitor();

            for (int i = 0; i < Pop3?.Count; i++)
            {
                //Pop3.DeleteMessage(i);
                MimeMessage message = Pop3.GetMessage(i);
                v.Visit(message);
                Console.WriteLine("Subject: {0}", message.Subject);
                List<string> Messages = new List<string>();
                if (message.Attachments != null)
                {
                    foreach (MimeEntity mime in message.Attachments)
                    {
                        if (mime.IsAttachment)
                        {
                            var mp = (MimePart)mime;
                            string path = Path.Combine(@"D:\Desktop\Hub Test Folder", mime.ContentType.Name);
                            Console.WriteLine("Name: " + mime.ContentType.Name);
                            using (var stream = File.Create(path))
                            {
                                mp.Content.DecodeTo(stream);
                            }
                            ProcessedInvoice[] processedInvoices = await DataProcessing.ProcessInvoices(path);

                            Messages.Add($"<p>\t  - Processed {mime.ContentType.Name} as Invoice for {ABNHelper.GetCompanyName(processedInvoices[0].ABN)}</p>");
                            //File.Delete(path);
                        }
                    }

                }

           
                EmailService service = new EmailService();
                MailboxAddress address = message.From.Mailboxes.First();
                Console.WriteLine(address);
                string body = "<h3><b><u>Items Processed:</h3></b></u> \n";
                for(int j = 0; j < Messages.Count; j++)
                {
                    body += Messages[j] + '\n';
                }

           //    service.Send("bot@wulfrunconstructions.com", address.Address, $"Re: {message.Subject}", body );
                

            }
            Pop3.DeleteAllMessages();

    

        }



        public void Dispose() { }

    }
}
