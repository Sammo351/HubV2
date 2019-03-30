using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Business_Software_V2.Data;
using IronOcr;

namespace Business_Software_V2
{
    public class ProcessedInvoice
    {
        public string ABN;
        public string Email;
        public string GstRegistered;
        public string Phone;
        public string CompanyName;
        public string FilePath;

    }

    public class InvoiceProcessor
    {
        public static ProcessedInvoice[] Process(string[] path)
        {
            var Ocr = new IronOcr.AdvancedOcr()
            {
                CleanBackgroundNoise = true,
                EnhanceContrast = true,
                EnhanceResolution = true,
                Language = IronOcr.Languages.English.OcrLanguagePack,
                Strategy = IronOcr.AdvancedOcr.OcrStrategy.Advanced,
                ColorSpace = AdvancedOcr.OcrColorSpace.Color,
                DetectWhiteTextOnDarkBackgrounds = true,
                InputImageType = AdvancedOcr.InputTypes.Document,
                RotateAndStraighten = true,
                ReadBarCodes = true,
                ColorDepth = 4
            };

            Console.WriteLine(path.Length);
            try
            {
                List<Task<ProcessedInvoice>> tasks = new List<Task<ProcessedInvoice>>();
                
                foreach(string p in path)
                {
                    tasks.Add(Task.Run(() => ProcessInvoice(Ocr, p)));
                }

                Task.WaitAll(tasks.ToArray());
                return tasks.Select(a => a.Result).ToArray();
            }
            catch (Exception ex) { }
            return null;
        }

        private static ProcessedInvoice ProcessInvoice(AdvancedOcr Ocr, string path)
        {
            var result = Ocr.Read(path);
            Regex rx = new Regex(@"(\d{3}\s*\d{3}\s*\d{3}\s*\d{2})|(\d{2}\s *\d{3}\s *\d{3}\s*\d{3})", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
            Regex email = new Regex(@"[\w\d\-]*[@][\w\d\-]*(.com.au|.com)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            Regex phone = new Regex(@"(?:\+?61|0)[2-478 ](?:[ -]?[0-9]){8}", RegexOptions.Compiled | RegexOptions.IgnoreCase);

            

            
            Match emailMatch = email.Match(result.Text);
            MatchCollection abnMatch = rx.Matches(result.Text);
            Match phoneMatch = phone.Match(result.Text);

            string abn = "";
            foreach (Match match in abnMatch)
            {
                GroupCollection groups = match.Groups;
                abn = groups[0].Value;
                Console.WriteLine("ABN: " + groups[0].Value);
            }

            string input = new WebClient().DownloadString(@"https://abr.business.gov.au/ABN/View?id=" + abn.Replace(" ", ""));

            // string text = 
            
          
            Regex abnRegistered = new Regex(@"<th>Goods &amp;.*\s+<td>\s*(.*)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            MatchCollection abnCollection = abnRegistered.Matches(input);
            string gstRegistered = "";
            foreach (Match match in abnCollection)
            {
                GroupCollection groups = match.Groups;
                gstRegistered = groups[1].Value.Replace("&nbsp;", " ");
            }

            string potentialCompanyName = FindLargestText(result);

            ProcessedInvoice processedInvoice = new ProcessedInvoice() { ABN = abn, GstRegistered = gstRegistered, Email = emailMatch.Value, Phone = phoneMatch.Value, CompanyName = potentialCompanyName, FilePath = path  };

            return processedInvoice;

        }

        public static string FindLargestText(OcrResult result)
        {
            
            OcrResult.OcrWord largestWord = result.Pages[0].Words[0];

            for(int i = 0; i < result.Pages[0].Words.Length; i++)
            {
                if (result.Pages[0].Words[i].FontSize > largestWord.FontSize)
                    largestWord = result.Pages[0].Words[i];
            }

            int lineNumber = largestWord.LineNumber;
            int numberInLine = largestWord.WordNumber;
            
            var lineOfText = result.Pages[0].LinesOfText[lineNumber-1];
            int f = numberInLine;
            string textInLine = "";
            
            for(int i = 0; i < lineOfText.WordCount; i++)
            {
                if (lineOfText.Words[i].FontSize == largestWord.FontSize)
                {
                    textInLine += " " + lineOfText.Words[i].Text;
                }
                else
                    break;
            }

            return textInLine;
        }


    }


}
