using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using System.Threading.Tasks;

namespace Business_Software_V2.Email
{
    class EmailService
    {
        private const string userName = "bot@wulfrunconstructions.com";

        private const string pass = "Builders1";

        private const string server = "mail.wulfrunconstructions.com";

        public void Send(string from, string to, string subject, string html, params MimeEntity[] attachments)
        {
            var email = new MimeMessage(attachments);
            email.From.Add(MailboxAddress.Parse(from));
            email.To.Add(MailboxAddress.Parse(to));
            
            email.Subject = subject;
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = html };

            using (var smtp = new SmtpClient())
            {
                smtp.Connect(server, 0, true);
                smtp.Authenticate(userName, pass);
                smtp.Send(email);
                smtp.Disconnect(true);
            }


        }

    }
}
