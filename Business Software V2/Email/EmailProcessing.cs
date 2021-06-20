using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Software_V2
{
    class EmailProcessing
    {
        private const string cPopUserName = "bot@wulfrunconstructions.com";

        private const string cPopPwd = "Builders1";

        private const string cPopMailServer = "mail.wulfrunconstructions.com";

        private const int cPopPort = 110;

        public static async Task ShowPop3Subjects()

        {
          
                using (EmailParser ep =

                       new EmailParser(cPopUserName,

                       cPopPwd, cPopMailServer, cPopPort))

                {

                    ep.OpenPop3();

                    await ep.DisplayPop3SubjectsAsync();

                    ep.ClosePop3();

                }
          

        }

    }
}

