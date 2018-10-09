using clean_full.Domain.ValueObjects;
using Xunit;

namespace clean_full.Tests.Domain
{
    public class NameTest
    {
        [Fact]
        public void Empty_Name_Should_Be_Created()
        {
            //
            // Arrange
            string empty = string.Empty;

            //
            // Act and Assert
            Assert.Throws<NameShouldNotBeEmptyException>(
                () => new Name(empty));
        }

        [Fact]
        public void Full_Name_Shoud_Be_Created()
        {
            //
            // Arrange
            string valid = "Test Name";

            //
            // Act
            Name name = new Name(valid);

            //
            // Assert
            Assert.Equal(new Name(valid), name);
        }
    }
}
