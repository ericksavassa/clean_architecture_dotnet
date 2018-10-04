namespace clean_full.Application.UseCases.CloseAccount
{
    using System;
    using System.Threading.Tasks;

    public interface ICloseAccountUseCase
    {
        Task<Guid> Execute(Guid accountId);
    }
}
