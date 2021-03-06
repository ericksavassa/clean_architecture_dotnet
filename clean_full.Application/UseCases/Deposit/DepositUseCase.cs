﻿namespace clean_full.Application.UseCases.Deposit
{
    using System.Threading.Tasks;
    using clean_full.Application.Repositories;
    using clean_full.Domain.Accounts;
    using System;
    using clean_full.Domain.ValueObjects;

    public sealed class DepositUseCase : IDepositUseCase
    {
        private readonly IAccountReadOnlyRepository _accountReadOnlyRepository;
        private readonly IAccountWriteOnlyRepository _accountWriteOnlyRepository;

        public DepositUseCase(
            IAccountReadOnlyRepository accountReadOnlyRepository,
            IAccountWriteOnlyRepository accountWriteOnlyRepository)
        {
            _accountReadOnlyRepository = accountReadOnlyRepository;
            _accountWriteOnlyRepository = accountWriteOnlyRepository;
        }

        public async Task<DepositOutput> Execute(Guid accountId, Amount amount)
        {
            Account account = await _accountReadOnlyRepository.Get(accountId);
            if (account == null)
                throw new AccountNotFoundException($"The account {accountId} does not exists or is already closed.");

            account.Deposit(amount);
            Credit credit = (Credit)account.GetLastTransaction();

            await _accountWriteOnlyRepository.Update(account, credit);

            DepositOutput output = new DepositOutput(
                credit,
                account.GetCurrentBalance());
            return output;
        }
    }
}
