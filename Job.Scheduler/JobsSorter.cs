using System;
using System.Collections.Generic;
using System.Linq;
namespace Job.Scheduler
{
    public static class JobsSorter
    {
        /// <summary>
        /// Jobs sorting implmentation using Kahn Algorithm
        /// </summary>
        /// <param name="inputJobs"></param>
        /// <returns></returns>
        public static string OrderJobs(string inputJobs)
        {
            if (string.IsNullOrWhiteSpace(inputJobs))
            {
                return null;
            }

            List<JobWithPreReqJobEdge> jobWithPreReqJobs = new List<JobWithPreReqJobEdge>();

            Stack<string> jobsWithNoPreReqJob = new Stack<string>();

            List<string> orderedJobs = new List<string>();

            // Parse input string to create a list of JobDependency Edge objects
            var jobs = inputJobs.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);


            foreach (var job in jobs)
            {
                var jobAndDependencyPair = job.Split("=>".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                var preReqJob = (jobAndDependencyPair.Length > 1 ? jobAndDependencyPair[1].Trim() : null);

                jobWithPreReqJobs.Add(new JobWithPreReqJobEdge(preReqJob, jobAndDependencyPair[0].Trim()));
            }

            // Create Job Graph object

            var jobsDACGraph = new JobsDAGraph(jobWithPreReqJobs);

            // Add all jobs with no dependencies in stack

            var jobsWithDependenciesCount = jobsDACGraph.JobWithPreReqCount;

            foreach (var job in jobsWithDependenciesCount.Keys)
            {
                if (jobsWithDependenciesCount[job] == 0)
                {
                    jobsWithNoPreReqJob.Push(job);
                }
            }

            var adjacencyList = jobsDACGraph.PreReqJobWithDependenciesList;

            while (jobsWithNoPreReqJob.Any())
            {
                var job = jobsWithNoPreReqJob.Pop();

                orderedJobs.Add(job);

                if (adjacencyList.ContainsKey(job))
                {
                    foreach (var preReqJob in adjacencyList[job])
                    {
                        // Remove the pre requisite job
                        jobsWithDependenciesCount[preReqJob]--;

                        if (jobsWithDependenciesCount[preReqJob] == 0)
                        {
                            jobsWithNoPreReqJob.Push(preReqJob);
                        }
                    }
                }
            }

            // Detect circular dependencies
            foreach (var job in jobsWithDependenciesCount.Keys)
            {
                if (jobsWithDependenciesCount[job] != 0)
                {
                    return "Jobs can’t have circular dependencies";
                }
            }

            return String.Join("", orderedJobs.ToArray().Select(o => o.ToString()));

        }
        
    }
}
