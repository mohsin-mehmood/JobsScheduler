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
        /// Dictionary for holding a list of jobs dependent on a pre-req job
        /// </summary>
        internal Dictionary<string, List<string>> PreReqJobWithDependenciesList { get; set; }


        /// <summary>
        /// Represents the number of jobs dependent on the given job
        /// </summary>
        internal Dictionary<string, int> JobWithPreReqCount { get; set; }

        public JobsDAGraph(List<JobWithPreReqJobEdge> jobsWithDependencies)
        {
            PreReqJobWithDependenciesList = new Dictionary<string, List<string>>();

            JobWithPreReqCount = new Dictionary<string, int>();

            // Initialize adjacency list

            foreach (var jobNode in jobsWithDependencies)
            {
                JobWithPreReqCount.Add(jobNode.JobToComplete, 0);

                if (!string.IsNullOrWhiteSpace(jobNode.PreReqJob))
                {
                    if (!PreReqJobWithDependenciesList.ContainsKey(jobNode.PreReqJob))
                    {
                        PreReqJobWithDependenciesList.Add(jobNode.PreReqJob, new List<string>());
                    }

                    PreReqJobWithDependenciesList[jobNode.PreReqJob].Add(jobNode.JobToComplete);
                }
            }


            // Traverse adjacency lists to fill number of jobs required to be completed before the given job

            foreach (var jobDependency in jobsWithDependencies)
            {

                if (jobDependency != null && !String.IsNullOrWhiteSpace(jobDependency.PreReqJob))
                {
                    foreach (var preReqJob in PreReqJobWithDependenciesList[jobDependency.PreReqJob])
                    {
                        if (!string.IsNullOrWhiteSpace(preReqJob))
                        {
                            JobWithPreReqCount[jobDependency.JobToComplete]++;
                        }
                    }
                }
            }
        }

    }
}
