using Business_Software_V2.Email;
using System;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;

using Google.Cloud.Firestore;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;

namespace Business_Software_V2
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static FirestoreDb db;

        void App_Startup(object sender, StartupEventArgs e)
        {
            // Application is running 
            // Process command line args 

            string path = AppDomain.CurrentDomain.BaseDirectory + @"business-hub-237006-firebase-adminsdk-gwgna-9ce7bf87d2.json";
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);

            db = FirestoreDb.Create("business-hub-237006");
         

            ApplicationSetup.Setup();
          
            // Create main application window, starting minimized if specified
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Timer timer = new Timer(15000)
            {
                AutoReset = true
            };
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }

        //async Task DoServerAsync()
        //{
        //    CollectionReference collection = db.Collection("test");
        //    DocumentReference a = await collection.AddAsync(new { Name = new { First = "Daniel", Last = "Thomas" } });
        //}

        

        public object AuthImplicit(string projectId)
        {
            // If you don't specify credentials when constructing the client, the
            // client library will look for credentials in the environment.
            var credential = GoogleCredential.GetApplicationDefault();
            
            //var storage = StorageClient.Create(credential);

            //// Make an authenticated API request.
            //var buckets = storage.ListBuckets(projectId);
            //foreach (var bucket in buckets)
            //{
            //    Console.WriteLine(bucket.Name);
            //}
            return null;
        }

        private async void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                await Task.Run(() => EmailProcessing.ShowPop3Subjects());
            }
            catch (Exception) { }
            
        }
    }
}
