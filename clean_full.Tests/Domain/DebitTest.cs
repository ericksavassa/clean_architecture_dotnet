using clean_full.Domain.Accounts;
using System;
using Xunit;

namespace clean_full.Tests.Domain
{
    public class DebitTest
    {
        [Fact]
        public void Debit_Should_Be_Loaded()
        {
            Debit debit = Debit.Load(
                Guid.Empty,
                Guid.Empty,
                100,
                DateTime.Today);

            Assert.Equal(Guid.Empty, debit.Id);
            Assert.Equal(Guid.Empty, debit.AccountId);
            Assert.Equal(100, debit.Amount);
            Assert.Equal(DateTime.Today, debit.TransactionDate);
            Assert.Equal("Debit", debit.Description);
        }
    }
}
