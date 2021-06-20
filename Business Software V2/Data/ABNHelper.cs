using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;

namespace Business_Software_V2.Data
{
    public static class ABNHelper
    {
        public static Action UpdatedBusinessCard;

        public static string defaultPath = @"D:\Desktop\Hub Test Folder\Trades";

        public static string FormatABN(string abn)
        {
            abn = abn.Replace(" ", "");
            
            String format = "00 000 000 000";
            ulong a = Convert.ToUInt64(abn);
            abn = a.ToString(format);
            Console.WriteLine(abn);
            return abn;
        }


        public static bool DoesABNExist(string abn)
        {
            abn = FormatABN(abn);
            return Directory.Exists(defaultPath + @"\" + abn);
        }

        public static string GetDirectory(string abn)
        {
            abn = abn.Split('\\').Last();
            abn = FormatABN(abn);
            return defaultPath + @"\" + abn;
        }

        public static void CreateDirectory(string abn)
        {
            abn = FormatABN(abn);
            Console.WriteLine(abn);

            if (!Directory.Exists(defaultPath))
                Directory.CreateDirectory(defaultPath);

            if (!Directory.Exists(defaultPath + "\\" + abn))
                Directory.CreateDirectory(defaultPath + "\\" + abn);
        }


        public static void CreateDirectoryFromPath(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

        }

        public static void SetupInfoFile(ABNData data)
        {
            CreateDirectory(FormatABN(data.ABN));

            string path = GetDirectory(data.ABN);
            string x = JsonConvert.SerializeObject(data);
            DataCompany dataCompany = new DataCompany();
            dataCompany.ABN = FormatABN(data.ABN);
            dataCompany.CompanyName = data.CompanyName;
            dataCompany.OfficeNumber = data.Phone;
            dataCompany.Email = data.Email;

            StreamWriter writer = File.CreateText(path + "/CompanyInfo.info");
            writer.Write(JsonConvert.SerializeObject(dataCompany));
            writer.Flush();
            writer.Close();
        }

        public static string GetCompanyName(string abn)
        {
            //THIS ERRORS WHEN ADDING A NEW ABN
            abn = FormatABN(abn);
            string path = GetDirectory(abn) + "/CompanyInfo.info";
            if (File.Exists(path))
            {
                string text = File.ReadAllText(path);
                var data = JsonConvert.DeserializeObject<DataCompany>(text);
                return data.CompanyName;
            }
            else
                return "Company N/A";

        }

        public static string[] GetAllABNs()
        {
            return Directory.GetDirectories(defaultPath);
        }

    }
}
