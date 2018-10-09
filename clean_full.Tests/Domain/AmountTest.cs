using clean_full.Domain.ValueObjects;
using Xunit;

namespace clean_full.Tests.Domain
{
    public class AmountTest
    {
        [Fact]
        public void Positive_Amount_Should_Be_Created()
        {
            //
            // Arrange
            double positiveAmount = 500;

            //
            // Act
            Amount amount = new Amount(positiveAmount);

            //
            // Assert
            Assert.Equal<double>(positiveAmount, amount);
        }

        [Fact]
        public void Amount_With_100_Minus_70_Should_Be_30()
        {
            //
            // Arrange
            Amount hundred = new Amount(100);
            Amount seventy = new Amount(70);

            //
            // Act
            Amount amount = hundred - seventy;

            //
            // Assert
            Assert.Equal(30, amount);
        }

        [Fact]
        public void Amount_With_100_Larger_Than_70()
        {
            //
            // Arrange
            Amount hundred = new Amount(100);
            Amount seventy = new Amount(70);

            //
            // Act & Assert
            Assert.True(hundred > seventy);
        }

        [Fact]
        public void Amount_With_30_Less_Than_Equal_30()
        {
            //
            // Arrange
            Amount thirty = new Amount(30);
            Amount seventy = new Amount(70);

            //
            // Act & Assert
            Assert.True(thirty <= seventy);
        }

        [Fact]
        public void Amount_With_10_Larger_Than_Equal_10()
        {
            //
            // Arrange
            Amount thirty = new Amount(10);
            Amount seventy = new Amount(10);

            //
            // Act & Assert
            Assert.True(thirty >= seventy);
        }
    }
}
