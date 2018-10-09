using clean_full.Domain.Accounts;
using System;
using Xunit;

namespace clean_full.Tests.Domain
{
    public class CreditTest
    {
        [Fact]
        public void Credit_Should_Be_Loaded()
        {
            Credit credit = Credit.Load(
                Guid.Empty,
                Guid.Empty,
                100,
                DateTime.Today);

            Assert.Equal(Guid.Empty, credit.Id);
            Assert.Equal(Guid.Empty, credit.AccountId);
            Assert.Equal(100, credit.Amount);
            Assert.Equal(DateTime.Today, credit.TransactionDate);
            Assert.Equal("Credit", credit.Description);
        }
    }
}
