using clean_full.Application.Repositories;
using clean_full.Application.UseCases;
using clean_full.Application.UseCases.GetAccountDetails;
using clean_full.Domain.Accounts;
using clean_full.Domain.ValueObjects;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace clean_full.Tests.Application
{
    public class GetAccountDetailsUseCaseTest
    {
        private readonly Mock<IAccountReadOnlyRepository> _accountReadOnlyRepository;
        private readonly GetAccountDetailsUseCase getAccountDetailsUseCase;

        public GetAccountDetailsUseCaseTest()
        {
            _accountReadOnlyRepository = new Mock<IAccountReadOnlyRepository>();
            
            getAccountDetailsUseCase = new GetAccountDetailsUseCase(_accountReadOnlyRepository.Object);
        }

        [Fact]
        public async Task GetAccountDetails_ValidId_ShouldReturnAnAccount()
        {
            //ARRANGE
            var accountId = Guid.NewGuid();
            Account account = BuildAccount(accountId);

            _accountReadOnlyRepository.Setup(m => m.Get(account.Id)).Returns(Task.FromResult(account));

            //ACT
            AccountOutput outPut = await getAccountDetailsUseCase.Execute(account.Id);

            //ASSERT
            _accountReadOnlyRepository.Verify(v => v.Get(account.Id), Times.Once());
            Assert.Equal(account.Id, outPut.AccountId);
            Assert.NotEmpty(outPut.Transactions);
        }

        [Fact]
        public async Task GetAccountDetails_AccountNotFound_ShouldThrowAnException()
        {
            //ARRANGE
            var accountId = Guid.NewGuid();
            Account account = null;
            
            _accountReadOnlyRepository.Setup(p => p.Get(accountId)).Returns(Task.FromResult(account));

            //ACT
            async Task<AccountOutput> function() => await getAccountDetailsUseCase.Execute(accountId);

            //ASSERT
            await Assert.ThrowsAnyAsync<clean_full.Application.ApplicationException>(function);
        }

        private Account BuildAccount(Guid accountId)
        {
            var customerId = Guid.NewGuid();
            TransactionCollection transactions = BuildTransactions(accountId);

            return Account.Load(accountId, customerId, transactions);
        }

        private TransactionCollection BuildTransactions(Guid accountId)
        {
            var transactions = new TransactionCollection();

            var amount = new Amount(10);
            var credit = new Credit(accountId, amount);
            transactions.Add(credit);

            amount = new Amount(5);
            var debit = new Debit(accountId, amount);
            transactions.Add(credit);

            return transactions;
        }
    }
}
