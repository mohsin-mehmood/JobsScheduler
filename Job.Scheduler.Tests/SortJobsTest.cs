using Xunit;

namespace Job.Scheduler.Tests
{
    public class SortJobsTest
    {
        /// <summary>
        /// Test OrderJobs method with Empty input
        /// </summary>
        [Fact]
        public void TopologicalSort_EmptyInput_Test()
        {
            // Arrange 
            var inputJobs = "";

            // Act
            var orderedJobs = SortJobs.TopologicalUsingKahnAlgorithm(inputJobs);

            // Assert
            Assert.Null(orderedJobs);
        }

        /// <summary>
        /// Test OrderJobs method with null input
        /// </summary>
        [Fact]
        public void TopologicalSort_NullInput_Test()
        {
            // Arrange 
            string inputJobs = null;

            // Act
            var orderedJobs = SortJobs.TopologicalUsingKahnAlgorithm(inputJobs);

            // Assert
            Assert.Null(orderedJobs);
        }


        [Fact]
        public void TopologicalSort_JobsWithNoDependencies_Test()
        {
            // Arrange 
            var inputJobs = @"a=>
b=>c
c=>";

            // Act
            var orderedJobs = SortJobs.TopologicalUsingKahnAlgorithm(inputJobs);

            // Assert
            Assert.Equal("cba", orderedJobs);
        }

        /// <summary>
        /// Test with list of jobs with dependencies
        /// </summary>
        [Fact]
        public void TopologicalSort_JobsWithDependencies_Test()
        {
            // Arrange 
            var inputJobs = @"a =>
b => c
c => f
d => a
e => b
f =>";

            // Act
            var orderedJobs = SortJobs.TopologicalUsingKahnAlgorithm(inputJobs);

            // Assert
            Assert.Equal("fcbead", orderedJobs);
        }

        /// <summary>
        /// Test Circular dependency
        /// </summary>
        [Fact]
        public void TopologicalSort_JobsCircularDependeny_Test()
        {
            // Arrange 
            var inputJobs = @"a =>
b => c
c => f
d => a
e =>
f => b";

            // Act
            var orderedJobs = SortJobs.TopologicalUsingKahnAlgorithm(inputJobs);

            // Assert
            Assert.Equal("Jobs can’t have circular dependencies", orderedJobs);
        }
    }
}
