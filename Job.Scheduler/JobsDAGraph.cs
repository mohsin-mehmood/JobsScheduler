using System;
using System.Collections.Generic;

namespace Job.Scheduler
{
    /// <summary>
    /// Represents Directed Acyclic Graph(DAG) for jobs 
    /// where each job is graph node 
    /// An edge from job a to b node represents job a needs to be completed before job b
    /// </summary>
    internal class JobsDAGraph
    {
        /// <summary>
        /// Dictionary for holding a list of jobs dependent(value) on a pre-req job(key)
        /// </summary>
        internal Dictionary<string, List<string>> JobWithDependendJobs { get; set; }


        /// <summary>
        /// Represents the number of jobs that needs to be completed (value) before the given job(key) can be completed
        /// </summary>
        internal Dictionary<string, int> JobWithDependentsCount { get; set; }

        public JobsDAGraph(List<JobWithDependencyRelation> jobsWithDependencies)
        {
            JobWithDependendJobs = new Dictionary<string, List<string>>();

            JobWithDependentsCount = new Dictionary<string, int>();

            // Fill dictionary holding a job (key) and list of jobs dependent on that job(value)
            foreach (var jobNode in jobsWithDependencies)
            {

                JobWithDependentsCount.Add(jobNode.JobToComplete, 0);

                if (!string.IsNullOrWhiteSpace(jobNode.JobDependentOn))
                {
                    if (!JobWithDependendJobs.ContainsKey(jobNode.JobDependentOn))
                    {
                        JobWithDependendJobs.Add(jobNode.JobDependentOn, new List<string>());
                    }

                    JobWithDependendJobs[jobNode.JobDependentOn].Add(jobNode.JobToComplete);
                }
            }


            // Fill dictionary holding a job to complete (key) and count of jobs that needs to be completed before(value)

            foreach (var jobDependency in jobsWithDependencies)
            {
                if (jobDependency != null && !String.IsNullOrWhiteSpace(jobDependency.JobDependentOn))
                {
                    foreach (var preReqJob in JobWithDependendJobs[jobDependency.JobDependentOn])
                    {
                        if (!string.IsNullOrWhiteSpace(preReqJob))
                        {
                            JobWithDependentsCount[jobDependency.JobToComplete]++;
                        }
                    }
                }
            }
        }

    }
}
