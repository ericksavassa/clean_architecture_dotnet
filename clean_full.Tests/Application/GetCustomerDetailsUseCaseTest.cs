using clean_full.Application.Repositories;
using clean_full.Application.UseCases;
using clean_full.Application.UseCases.GetCustomerDetails;
using clean_full.Domain.Accounts;
using clean_full.Domain.Customers;
using clean_full.Domain.ValueObjects;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace clean_full.Tests.Application
{
    public class GetCustomerDetailsUseCaseTest
    {
        private readonly Mock<IAccountReadOnlyRepository> _accountReadOnlyRepository;
        private readonly Mock<ICustomerReadOnlyRepository> _customerReadOnlyRepository;
        private readonly GetCustomerDetailsUseCase getCustomerDetailsUseCase;

        public GetCustomerDetailsUseCaseTest()
        {
            _accountReadOnlyRepository = new Mock<IAccountReadOnlyRepository>();
            _customerReadOnlyRepository = new Mock<ICustomerReadOnlyRepository>();

            getCustomerDetailsUseCase = new GetCustomerDetailsUseCase(_customerReadOnlyRepository.Object, _accountReadOnlyRepository.Object);
        }

        [Fact]
        public async Task GetCustomerDetails_ValidId_ShouldReturnAnCustomer()
        {
            //ARRANGE
            var customerId = Guid.NewGuid();
            var ssn = new SSN("0101010000");
            var name = new Name("Test_Customer");

            var accountList = new AccountCollection();
            var account = new Account(customerId);
            accountList.Add(account.Id);
            
            var customer = Customer.Load(customerId, name, ssn, accountList);
                        
            _accountReadOnlyRepository.Setup(m => m.Get(account.Id)).Returns(Task.FromResult(account));
            _customerReadOnlyRepository.Setup(m => m.Get(customerId)).Returns(Task.FromResult(customer));

            //ACT
            CustomerOutput customerOutPut = await getCustomerDetailsUseCase.Execute(customerId);
            
            //ASSERT
            _accountReadOnlyRepository.Verify(v => v.Get(account.Id), Times.Once());
            Assert.Equal(customerOutPut.CustomerId, customerId);
        }

        
        [Fact]
        public async Task Deposit_AccountNotFound_ShouldThrowAnException()
        {
            //ARRANGE
            var customerId = Guid.NewGuid();
            Customer customer = null;

            _customerReadOnlyRepository.Setup(m => m.Get(customerId)).Returns(Task.FromResult(customer));

            //ACT
            async Task<CustomerOutput> function() => await getCustomerDetailsUseCase.Execute(customerId);

            //ASSERT
            await Assert.ThrowsAnyAsync<clean_full.Application.ApplicationException>(function);
        }
    }
}
