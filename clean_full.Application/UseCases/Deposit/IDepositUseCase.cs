namespace clean_full.Application.UseCases.Deposit
{
    using clean_full.Domain.ValueObjects;
    using System;
    using System.Threading.Tasks;

    public interface IDepositUseCase
    {
        Task<DepositOutput> Execute(Guid accountId, Amount amount);
    }
}
