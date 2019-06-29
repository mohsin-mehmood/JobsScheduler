using System;
using System.Collections.Generic;
using System.Linq;
namespace Job.Scheduler
{
    public static class SortJobs
    {
        /// <summary>
        /// Topological sorting implmentation using Kahn Algorithm
        /// https://en.wikipedia.org/wiki/Topological_sorting#Kahn's_algorithm
        /// </summary>
        /// <param name="inputJobs"></param>
        /// <returns></returns>
        public static string TopologicalUsingKahnAlgorithm(string inputJobs)
        {
            if (string.IsNullOrWhiteSpace(inputJobs))
            {
                return null;
            }

            List<JobWithDependencyRelation> jobWithDependentJobs = new List<JobWithDependencyRelation>();

            Stack<string> jobsWithNoPreReqJob = new Stack<string>();

            List<string> orderedJobs = new List<string>();

            // Parse input string to create a list of JobDependency Edge objects
            var jobs = inputJobs.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);


            foreach (var job in jobs)
            {
                var jobAndDependencyPair = job.Split("=>".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                var preReqJob = (jobAndDependencyPair.Length > 1 ? jobAndDependencyPair[1].Trim() : null);

                jobWithDependentJobs.Add(new JobWithDependencyRelation(preReqJob, jobAndDependencyPair[0].Trim()));
            }

            // Create Job Graph object

            var jobsDACGraph = new JobsDAGraph(jobWithDependentJobs);

            // Add jobs in stacks with other dependent jobs dependent on them
            var jobsWithDependentsCount = jobsDACGraph.JobWithDependentsCount;

            foreach (var job in jobsWithDependentsCount.Keys)
            {
                if (jobsWithDependentsCount[job] == 0)
                {
                    jobsWithNoPreReqJob.Push(job);
                }
            }

            var jobWithDependents = jobsDACGraph.JobWithDependendJobs;

            /* Kahn Algorithm implementation
             * jobsWithNoPreReqJob - Stack holding jobs with no dependencies (0 incoming edges)
             * orderedJobs - List holding ordered jobs
            */
            while (jobsWithNoPreReqJob.Any())
            {
                var job = jobsWithNoPreReqJob.Pop();

                orderedJobs.Add(job);

                if (jobWithDependents.ContainsKey(job))
                {
                    foreach (var preReqJob in jobWithDependents[job])
                    {
                        // Remove the pre requisite job
                        jobsWithDependentsCount[preReqJob]--;

                        if (jobsWithDependentsCount[preReqJob] == 0)
                        {
                            jobsWithNoPreReqJob.Push(preReqJob);
                        }
                    }
                }
            }

            // Detect circular dependencies
            // After Kahn algorithm execution if there are still jobs with other jobs depending upon them.
            foreach (var job in jobsWithDependentsCount.Keys)
            {
                if (jobsWithDependentsCount[job] != 0)
                {
                    return "Jobs can’t have circular dependencies";
                }
            }

            return String.Join("", orderedJobs.ToArray().Select(o => o.ToString()));

        }
        
    }
}
