using Business_Software_V2.Data;
using Newtonsoft.Json;
using System.IO;

namespace Business_Software_V2
{
    public static class CompanyHelper
    {
        public static string GetCompanyName(string abn)
        {
            return ABNHelper.GetCompanyName(abn);
        }

        public static DataCompany GetCompany(string abn)
        {
            string dir = ABNHelper.GetDirectory(abn);
            string filePath = dir + "/CompanyInfo.info";
            if (File.Exists(dir + "/CompanyInfo.info"))
            {
                DataCompany company = JsonConvert.DeserializeObject<DataCompany>(File.ReadAllText(filePath));
                return company;
            }
            return null;
        }

        internal static DataCompany[] GetAllCompanies()
        {
            DataCompany[] companies = new DataCompany[ABNHelper.GetAllABNs().Length];

            string[] abns = ABNHelper.GetAllABNs();
            for (int i = 0; i < abns.Length; i++)
            {
                companies[i] = GetCompany(abns[i]);
            }
            return companies;
        }

        internal static void SaveChanges(DataCompany dataCompany)
        {
            string dir = ABNHelper.GetDirectory(dataCompany.ABN);
            string filePath = dir + "/CompanyInfo.info";

            string s = JsonConvert.SerializeObject(dataCompany);
            StreamWriter writer = new StreamWriter(filePath);
            writer.Write(s);
            writer.Flush();
            writer.Close();
        }
    }
}
