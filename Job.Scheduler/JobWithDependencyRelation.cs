namespace Job.Scheduler
{
    /// <summary>
    /// Represents the job and its pre-req jobs
    /// </summary>
    internal class JobWithDependencyRelation
    {
        internal string JobDependentOn { get; set; }
        internal string JobToComplete { get; set; }

        
        public JobWithDependencyRelation(string preReqJob, string jobToComplete)
        {
            this.JobDependentOn = preReqJob;
            this.JobToComplete = jobToComplete;
        }
    }
}
