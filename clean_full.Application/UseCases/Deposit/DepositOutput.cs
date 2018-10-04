namespace clean_full.Application.UseCases.Deposit
{
    using clean_full.Domain.Accounts;
    using clean_full.Domain.ValueObjects;

    public sealed class DepositOutput
    {
        public TransactionOutput Transaction { get; }
        public double UpdatedBalance { get; }

        public DepositOutput(
            Credit credit,
            Amount updatedBalance)
        {
            Transaction = new TransactionOutput(
                credit.Description,
                credit.Amount,
                credit.TransactionDate);

            UpdatedBalance = updatedBalance;
        }
    }
}
