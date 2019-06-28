namespace Job.Scheduler
{
    /// <summary>
    /// Represents the job and its pre-req jobs
    /// </summary>
    internal class JobWithPreReqJobEdge
    {
        internal string PreReqJob { get; set; }
        internal string JobToComplete { get; set; }

        
        public JobWithPreReqJobEdge(string preReqJob, string jobToComplete)
        {
            this.PreReqJob = preReqJob;
            this.JobToComplete = jobToComplete;
        }
    }
}
