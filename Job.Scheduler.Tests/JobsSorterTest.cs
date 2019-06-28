using Xunit;

namespace Job.Scheduler.Tests
{
    public class JobsSorterTest
    {
        /// <summary>
        /// Test OrderJobs method with Empty input
        /// </summary>
        [Fact]
        public void OrderJobsWithEmptyInput_Test()
        {
            // Arrange 
            var inputJobs = "";

            // Act
            var orderedJobs = JobsSorter.OrderJobs(inputJobs);

            // Assert
            Assert.Null(orderedJobs);
        }

        /// <summary>
        /// Test OrderJobs method with null input
        /// </summary>
        [Fact]
        public void OrderJobsWithNullInput_Test()
        {
            // Arrange 
            string inputJobs = null;

            // Act
            var orderedJobs = JobsSorter.OrderJobs(inputJobs);

            // Assert
            Assert.Null(orderedJobs);
        }


        [Fact]
        public void OrderJobsWithInputJobsWithoutDependencies_Test()
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

        /// <summary>
        /// Test with list of jobs with dependencies
        /// </summary>
        [Fact]
        public void OrderJobsWithInputJobsWithDependencies_Test()
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

        /// <summary>
        /// Test Circular dependency
        /// </summary>
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
            Assert.Equal("Jobs can�t have circular dependencies", orderedJobs);
        }
    }
}
