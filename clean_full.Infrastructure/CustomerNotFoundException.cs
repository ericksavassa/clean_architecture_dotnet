namespace clean_full.Infrastructure
{
    public class CustomerNotFoundException : InfrastructureException
    {
        internal CustomerNotFoundException(string message)
            : base(message)
        { }
    }
}
