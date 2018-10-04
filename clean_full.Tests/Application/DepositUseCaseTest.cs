using clean_full.Application.Repositories;
using clean_full.Application.UseCases.Deposit;
using clean_full.Domain.Accounts;
using clean_full.Domain.ValueObjects;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace clean_full.Tests.Application
{
    public class DepositUseCaseTest
    {
        private readonly Mock<IAccountReadOnlyRepository> _accountReadOnlyRepository;
        private readonly Mock<IAccountWriteOnlyRepository> _accountWriteOnlyRepository;
        private readonly DepositUseCase depositUseCase;

        public DepositUseCaseTest()
        {
            _accountReadOnlyRepository = new Mock<IAccountReadOnlyRepository>();
            _accountWriteOnlyRepository = new Mock<IAccountWriteOnlyRepository>();

            depositUseCase = new DepositUseCase(_accountReadOnlyRepository.Object, _accountWriteOnlyRepository.Object);
        }

        [Fact]
        public async Task Deposit_ValidAccount_ShouldCompleteTask()
        {
            //ARRANGE
            var customerId = Guid.NewGuid();
            var account = new Account(customerId);
            var amount = new Amount(10);

            _accountReadOnlyRepository.Setup(m => m.Get(account.Id)).Returns(Task.FromResult(account));
            _accountWriteOnlyRepository.Setup(m => m.Update(account, It.IsAny<Credit>())).Returns(Task.CompletedTask);

            //ACT
            DepositOutput outPut = await depositUseCase.Execute(account.Id, amount);
            
            //ASSERT
            _accountReadOnlyRepository.Verify(v => v.Get(account.Id), Times.Once());
            _accountWriteOnlyRepository.Verify(v => v.Update(account, It.IsAny<Credit>()), Times.Once());
            Assert.Equal(amount.ToString(), outPut.UpdatedBalance.ToString());
            Assert.NotNull(outPut.Transaction);
        }

        [Fact]
        public async Task Deposit_AccountWithOldDeposits_ShouldCompleteTask()
        {
            //ARRANGE
            var customerId = Guid.NewGuid();
            var account = new Account(customerId);
            var amount = new Amount(10);
            account.Deposit(amount);

            _accountReadOnlyRepository.Setup(m => m.Get(account.Id)).Returns(Task.FromResult(account));
            _accountWriteOnlyRepository.Setup(m => m.Update(account, It.IsAny<Credit>())).Returns(Task.CompletedTask);

            //ACT
            DepositOutput outPut = await depositUseCase.Execute(account.Id, amount);

            //ASSERT
            _accountReadOnlyRepository.Verify(v => v.Get(account.Id), Times.Once());
            _accountWriteOnlyRepository.Verify(v => v.Update(account, It.IsAny<Credit>()), Times.Once());
            Assert.Equal(20, outPut.UpdatedBalance);
        }
        
        [Fact]
        public async Task Deposit_AccountNotFound_ShouldThrowAnException()
        {
            //ARRANGE
            var accountId = Guid.NewGuid();
            Account account = null;
            var amount = new Amount(10);

            _accountReadOnlyRepository.Setup(p => p.Get(accountId)).Returns(Task.FromResult(account));

            //ACT
            async Task<DepositOutput> function() => await depositUseCase.Execute(accountId, amount);

            //ASSERT
            await Assert.ThrowsAnyAsync<clean_full.Application.ApplicationException>(function);
        }
    }
}
