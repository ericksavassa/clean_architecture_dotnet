namespace clean_full.Domain.Accounts
{
    using clean_full.Domain.ValueObjects;
    using System;

    public interface ITransaction
    {
        Amount Amount { get; }
        string Description { get; }
        DateTime TransactionDate { get; }
    }
}
