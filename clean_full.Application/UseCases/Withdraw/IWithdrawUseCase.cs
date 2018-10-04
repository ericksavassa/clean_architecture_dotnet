namespace clean_full.Application.UseCases.Withdraw
{
    using clean_full.Domain.ValueObjects;
    using System;
    using System.Threading.Tasks;

    public interface IWithdrawUseCase
    {
        Task<WithdrawOutput> Execute(Guid accountId, Amount amount);
    }
}
