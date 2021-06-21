using System.Collections.Generic;
using System.IO;

namespace Business_Software_V2
{
    internal class JobHelper
    {
        internal static IEnumerable<DataJob> GetAllJobs()
        {
            List<DataJob> jobsList = new List<DataJob>();
            string[] jobs = Directory.GetFiles(DataJob.JobDirectory);
            foreach (string job in jobs)
            {
                string d = File.ReadAllText(job);
                jobsList.Add(DataJob.Load(d));

            }
            return jobsList;

        }
    }
}