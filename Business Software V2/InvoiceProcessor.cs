﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using Business_Software_V2.Data;
using IronOcr;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using Xceed.Words.NET;

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
                Stopwatch stopwatch = Stopwatch.StartNew();
                foreach (string p in path)
                {

                    tasks.Add(Task.Run(() => ProcessDocument(p, Ocr)));
                }

                Task.WaitAll(tasks.ToArray());
                stopwatch.Stop();
                Console.WriteLine(stopwatch.Elapsed.TotalSeconds);
                return tasks.Select(a => a.Result).ToArray();
            }
            catch (Exception ex) { }
            return null;
        }

        private static ProcessedInvoice ProcessDocument(string path, AdvancedOcr ocr)
        {
            string text = "";
            if (System.IO.Path.GetExtension(path) == ".pdf")
            {
                PdfReader reader = new PdfReader(path);
                text = PdfTextExtractor.GetTextFromPage(reader, 1);
            }
            else if (System.IO.Path.GetExtension(path) == ".docx")
            {
                var doc = DocX.Load(path);
                Headers h = doc.Headers;
                text = "";
                if (h.Even != null && h.Even.Paragraphs != null)
                {
                    foreach (Paragraph p in h.Even?.Paragraphs)
                        text += p.Text;
                }

                if (h.Odd != null && h.Odd.Paragraphs != null)
                {
                    foreach (Paragraph p in h.Odd?.Paragraphs)
                        text += p.Text;
                }

                if (h.First != null && h.First.Paragraphs != null)
                {
                    foreach (Paragraph p in h.First?.Paragraphs)
                        text += p.Text;
                }



                text += doc.Text;
                
                Console.WriteLine(text);
            }
            else
            {
                text = ocr.Read(path).Text;
            }

            return ProcessInvoice(text, path);
        }

        private static ProcessedInvoice ProcessInvoice(string result, string path)
        {
            Regex rx = new Regex(@"(\d{3}\s*\d{3}\s*\d{3}\s*\d{2})|(\d{2}\s *\d{3}\s *\d{3}\s*\d{3})", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
            Regex email = new Regex(@"[\w\d\-]*[@][\w\d\-]*(.com.au|.com)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            Regex phone = new Regex(@"(?:\+?61|0)[2-478 ](?:[ -]?[0-9]){8}", RegexOptions.Compiled | RegexOptions.IgnoreCase);




            Match emailMatch = email.Match(result);
            Match abnMatch = rx.Match(result);
            Match phoneMatch = phone.Match(result);

            string abn = abnMatch.Value;
          

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

            //string potentialCompanyName = FindLargestText(result);

            ProcessedInvoice processedInvoice = new ProcessedInvoice() { ABN = abn, GstRegistered = gstRegistered, Email = emailMatch.Value, Phone = phoneMatch.Value, FilePath = path };
            Console.WriteLine(abn);
            return processedInvoice;

        }

        public static string FindLargestText(OcrResult result)
        {

            OcrResult.OcrWord largestWord = result.Pages[0].Words[0];

            for (int i = 0; i < result.Pages[0].Words.Length; i++)
            {
                if (result.Pages[0].Words[i].FontSize > largestWord.FontSize)
                    largestWord = result.Pages[0].Words[i];
            }

            int lineNumber = largestWord.LineNumber;
            int numberInLine = largestWord.WordNumber;

            var lineOfText = result.Pages[0].LinesOfText[lineNumber - 1];
            int f = numberInLine;
            string textInLine = "";

            for (int i = 0; i < lineOfText.WordCount; i++)
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
