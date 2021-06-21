using Business_Software_V2.Data;
using Google.Cloud.Firestore;
using Newtonsoft.Json;
using System.Collections.Generic;
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

        internal static async System.Threading.Tasks.Task<DataCompany[]> GetAllCompanies()
        {
            CollectionReference d = App.db.Collection("Trades");
            IAsyncEnumerable<DocumentReference> refs = d.ListDocumentsAsync();
            List<DataCompany> Companies = new List<DataCompany>();
            IAsyncEnumerator<DocumentSnapshot> re = (IAsyncEnumerator<DocumentSnapshot>)d.StreamAsync();

            await foreach (DocumentReference r in refs)
            {
                DocumentSnapshot s = await r.GetSnapshotAsync();
                Companies.Add(s.ConvertTo<DataCompany>());
            }


            return Companies.ToArray();
        }

        internal static void SaveChanges(DataCompany dataCompany)
        {
            string dir = ABNHelper.GetDirectory(dataCompany.ABN);
            string filePath = dir + "/CompanyInfo.info";
            DocumentReference d = App.db.Collection("Trades").Document(dataCompany.CompanyName);
            d.SetAsync(dataCompany);
            
            string s = JsonConvert.SerializeObject(dataCompany);
            StreamWriter writer = new StreamWriter(filePath);
            writer.Write(s);
            writer.Flush();
            writer.Close();
        }
    }
}
