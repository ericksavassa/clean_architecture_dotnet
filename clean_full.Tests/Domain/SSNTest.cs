using clean_full.Domain.ValueObjects;
using Xunit;

namespace clean_full.Tests.Domain
{
    public class SSNTest
    {
        [Fact]
        public void Empty_SSN_Should_Not_Be_Created()
        {
            //
            // Arrange
            string empty = string.Empty;

            //
            // Act and Assert
            Assert.Throws<SSNShouldNotBeEmptyException>(
                () => new SSN(empty));
        }

        [Fact]
        public void Valid_SSN_Should_Be_Created()
        {
            //
            // Arrange
            string valid = "08724050601";

            //
            // Act
            SSN SSN = new SSN(valid);

            // Assert
            Assert.Equal(valid, SSN);
        }
    }
}
