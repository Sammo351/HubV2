using System.IO;

namespace Business_Software_V2
{
    public static class ApplicationSetup
    {
        public static string defaultPath = @"D:\Desktop\Hub Test Folder";
        public static string[] RequiredFolders = new string[]
        {
            "Jobs",
            "Trades",
        };

        public static void Setup()
        {
            Directory.CreateDirectory(defaultPath);
            for (int i = 0; i < RequiredFolders.Length; i++)
            {
                string path = Path.Combine(defaultPath, RequiredFolders[i]);
                Directory.CreateDirectory(path);
            }
        }


    }
}
