using System.Collections.Generic;
using System.IO;

namespace Business_Software_V2
{
    internal class JobHelper
    {
        internal static IEnumerable<JobData> GetAllJobs()
        {
            List<JobData> jobsList = new List<JobData>();
            string[] jobs = Directory.GetFiles(JobData.JobDirectory);
            foreach (string job in jobs)
            {
                string d = File.ReadAllText(job);
                jobsList.Add(JobData.Load(d));

            }
            return jobsList;

        }
    }
}