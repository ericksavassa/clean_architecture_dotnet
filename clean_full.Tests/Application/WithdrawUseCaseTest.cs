using clean_full.Application.Repositories;
using clean_full.Application.UseCases.Withdraw;
using clean_full.Domain.Accounts;
using clean_full.Domain.ValueObjects;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace clean_full.Tests.Application
{
    public class WithdrawUseCaseTest
    {
        private readonly Mock<IAccountReadOnlyRepository> _accountReadOnlyRepository;
        private readonly Mock<IAccountWriteOnlyRepository> _accountWriteOnlyRepository;
        private readonly WithdrawUseCase withdrawUseCase;

        public WithdrawUseCaseTest()
        {
            _accountReadOnlyRepository = new Mock<IAccountReadOnlyRepository>();
            _accountWriteOnlyRepository = new Mock<IAccountWriteOnlyRepository>();

            withdrawUseCase = new WithdrawUseCase(_accountReadOnlyRepository.Object, _accountWriteOnlyRepository.Object);
        }

        [Fact]
        public async Task Withdraw_ValidValues_ShouldCompleteTask()
        {
            //ARRANGE
            var account = BuildAccount(10);
            var amount = new Amount(10);

            _accountReadOnlyRepository.Setup(m => m.Get(account.Id)).Returns(Task.FromResult(account));
            _accountWriteOnlyRepository.Setup(m => m.Update(account, It.IsAny<Debit>())).Returns(Task.CompletedTask);

            //ACT
            WithdrawOutput outPut = await withdrawUseCase.Execute(account.Id, amount);

            //ASSERT
            _accountReadOnlyRepository.Verify(v => v.Get(account.Id), Times.Once());
            _accountWriteOnlyRepository.Verify(v => v.Update(account, It.IsAny<Debit>()), Times.Once());
            Assert.Equal(0, outPut.UpdatedBalance);
            Assert.NotNull(outPut.Transaction);
        }

        [Fact]
        public async Task Withdraw_AccountWithouValues_ShouldThrowAnError()
        {
            //ARRANGE
            var customerId = Guid.NewGuid();
            var account = new Account(customerId);
            var amount = new Amount(10);

            _accountReadOnlyRepository.Setup(m => m.Get(account.Id)).Returns(Task.FromResult(account));
            _accountWriteOnlyRepository.Setup(m => m.Update(account, It.IsAny<Credit>())).Returns(Task.CompletedTask);

            //ACT
            async Task<WithdrawOutput> function() => await withdrawUseCase.Execute(account.Id, amount);
            
            //ASSERT
            await Assert.ThrowsAnyAsync<InsuficientFundsException>(function);
        }

        [Fact]
        public async Task Withdraw_AccountNotFound_ShouldThrowAnException()
        {
            //ARRANGE
            var accountId = Guid.NewGuid();
            Account account = null;
            var amount = new Amount(10);

            _accountReadOnlyRepository.Setup(p => p.Get(accountId)).Returns(Task.FromResult(account));

            //ACT
            async Task<WithdrawOutput> function() => await withdrawUseCase.Execute(accountId, amount);

            //ASSERT
            await Assert.ThrowsAnyAsync<clean_full.Application.ApplicationException>(function);
        }

        private Account BuildAccount(double creditAmount)
        {
            var accountId = Guid.NewGuid();
            var customerId = Guid.NewGuid();
            TransactionCollection transactions = BuildTransactions(accountId, creditAmount);

            return Account.Load(accountId, customerId, transactions);
        }

        private TransactionCollection BuildTransactions(Guid accountId, double creditAmount)
        {
            var transactions = new TransactionCollection();

            var amount = new Amount(creditAmount);
            var credit = new Credit(accountId, amount);
            transactions.Add(credit);

            return transactions;
        }
    }
}
