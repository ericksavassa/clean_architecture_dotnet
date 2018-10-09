using clean_full.Application.Repositories;
using clean_full.Application.UseCases.CloseAccount;
using clean_full.Domain;
using clean_full.Domain.Accounts;
using clean_full.Domain.ValueObjects;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace clean_full.Tests.Application
{
    public class CloseAccountUseCaseTest
    {
        private readonly Mock<IAccountReadOnlyRepository> _accountReadOnlyRepository;
        private readonly Mock<IAccountWriteOnlyRepository> _accountWriteOnlyRepository;
        private readonly CloseAccountUseCase closedAccountUseCase;

        public CloseAccountUseCaseTest()
        {
            _accountReadOnlyRepository = new Mock<IAccountReadOnlyRepository>();
            _accountWriteOnlyRepository = new Mock<IAccountWriteOnlyRepository>();

            closedAccountUseCase = new CloseAccountUseCase(_accountReadOnlyRepository.Object, _accountWriteOnlyRepository.Object);
        }

        [Fact]
        public async Task ClosedAccount_ValidAccount_ShouldCompleteTask()
        {
            //ARRANGE
            var customerId = Guid.NewGuid();
            var account = new Account(customerId);

            _accountReadOnlyRepository.Setup(m => m.Get(account.Id)).Returns(Task.FromResult(account));
            _accountWriteOnlyRepository.Setup(m => m.Delete(account)).Returns(Task.CompletedTask);

            //ACT
            Guid accountId = await closedAccountUseCase.Execute(account.Id);

            //ASSERT
            _accountReadOnlyRepository.Verify(v => v.Get(account.Id), Times.Once());
            _accountWriteOnlyRepository.Verify(v => v.Delete(account), Times.Once());
            Assert.Equal(account.Id, accountId);
            Assert.NotEqual(customerId, accountId);
        }

        [Fact]
        public async Task ClosedAccount_AccountNotFound_ShouldThrowAnException()
        {
            //ARRANGE
            var customerId = Guid.NewGuid();
            Account account = null;

            _accountReadOnlyRepository.Setup(p => p.Get(customerId)).Returns(Task.FromResult(account));

            //ACT
            async Task<Guid> function() => await closedAccountUseCase.Execute(customerId);

            //ASSERT
            await Assert.ThrowsAnyAsync<clean_full.Application.ApplicationException>(function);
        }

        [Fact]
        public async Task ClosedAccount_AccountWithBalance_ShouldThrowADomainException()
        {
            //ARRANGE
            var customerId = Guid.NewGuid();
            var account = new Account(customerId);
            var amount = new Amount(10);
            account.Deposit(amount);

            _accountReadOnlyRepository.Setup(p => p.Get(customerId)).Returns(Task.FromResult(account));

            //ACT
            async Task<Guid> function() => await closedAccountUseCase.Execute(customerId);

            //ASSERT
            await Assert.ThrowsAnyAsync<DomainException>(function);
        }
    }
}
