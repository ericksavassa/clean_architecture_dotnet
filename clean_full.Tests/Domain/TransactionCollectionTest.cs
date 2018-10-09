using clean_full.Domain.Accounts;
using System;
using System.Collections.Generic;
using Xunit;

namespace clean_full.Tests.Domain
{
    public class TransactionCollectionTest
    {
        [Fact]
        public void Multiple_Transactions_Should_Be_Added()
        {
            var transactionCollection = new TransactionCollection();
            transactionCollection.Add(new List<ITransaction>()
            {
                new Credit(Guid.Empty, 100),
                new Debit(Guid.Empty, 30)
            });

            Assert.Equal(2, transactionCollection.GetTransactions().Count);
        }
    }
}
