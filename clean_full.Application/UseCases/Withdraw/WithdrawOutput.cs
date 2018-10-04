namespace clean_full.Application.UseCases.Withdraw
{
    using clean_full.Domain.Accounts;
    using clean_full.Domain.ValueObjects;

    public sealed class WithdrawOutput
    {
        public TransactionOutput Transaction { get; }
        public double UpdatedBalance { get; }

        public WithdrawOutput(Debit transaction, Amount updatedBalance)
        {
            Transaction = new TransactionOutput(
                transaction.Description,
                transaction.Amount,
                transaction.TransactionDate);

            UpdatedBalance = updatedBalance;
        }
    }
}
