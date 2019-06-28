using Xunit;

namespace Job.Scheduler.Tests
{
    public class JobsSorterTest
    {
        [Fact]
        public void EmptyJobs_Test()
        {
            // Arrange 
            var inputJobs = "";

            // Act
            var orderedJobs = JobsSorter.OrderJobs(inputJobs);

            // Assert
            Assert.Empty(orderedJobs);
        }


        [Fact]
        public void Jobs_TestWithNoDependentJobs()
        {
            // Arrange 
            var inputJobs = @"a=>
b=>c
c=>";

            // Act
            var orderedJobs = JobsSorter.OrderJobs(inputJobs);

            // Assert
            Assert.Equal("cba", orderedJobs);
        }


        [Fact]
        public void Jobs_TestWithDependentJobs()
        {
            // Arrange 
            var inputJobs = @"a =>
b => c
c => f
d => a
e => b
f =>";

            // Act
            var orderedJobs = JobsSorter.OrderJobs(inputJobs);

            // Assert
            Assert.Equal("fcbead", orderedJobs);
        }

        [Fact]
        public void Jobs_TestCircularDependency()
        {
            // Arrange 
            var inputJobs = @"a =>
b => c
c => f
d => a
e =>
f => b";

            // Act
            var orderedJobs = JobsSorter.OrderJobs(inputJobs);

            // Assert
            Assert.Equal("Jobs can’t have circular dependencies", orderedJobs);
        }
    }
}
