using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace Business_Software_V2.Data
{
    public static class ABNHelper
    {
        static string defaultPath = @"D:\Desktop\Hub Test Folder";

        public static bool DoesABNExist(string abn)
        {
            return Directory.Exists(defaultPath + @"\" + abn);
        }

        public static string GetDirectory(string abn)
        {
            return Path.GetFullPath(defaultPath + @"\" + abn);
        }

        public static void CreateDirectory(string abn)
        {
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
            CreateDirectory(data.ABN);

            string path = GetDirectory(data.ABN);
            string x = JsonConvert.SerializeObject(data);
            StreamWriter writer = File.CreateText(path + "/company.info");
            writer.Write(x);
            writer.Flush();
            writer.Close();
        }

    }
}
