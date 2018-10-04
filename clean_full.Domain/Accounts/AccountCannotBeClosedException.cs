namespace clean_full.Domain.Accounts
{
    public sealed class AccountCannotBeClosedException : DomainException
    {
        internal AccountCannotBeClosedException(string message)
            : base(message)
        { }
    }
}
