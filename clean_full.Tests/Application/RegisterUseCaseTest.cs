using clean_full.Application.Repositories;
using clean_full.Application.UseCases.Register;
using clean_full.Domain;
using clean_full.Domain.Accounts;
using clean_full.Domain.Customers;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace clean_full.Tests.Application
{
    public class RegisterUseCaseTest
    {
        private readonly Mock<ICustomerWriteOnlyRepository> _customerWriteOnlyRepository;
        private readonly Mock<IAccountWriteOnlyRepository> _accountWriteOnlyRepository;
        private readonly RegisterUseCase registerUseCase;

        public RegisterUseCaseTest()
        {
            _customerWriteOnlyRepository = new Mock<ICustomerWriteOnlyRepository>();
            _accountWriteOnlyRepository = new Mock<IAccountWriteOnlyRepository>();

            registerUseCase = new RegisterUseCase(_customerWriteOnlyRepository.Object, _accountWriteOnlyRepository.Object);
        }

        [Fact]
        public async Task Register_ValidValues_ShouldReturnACustomerWithAnAccount()
        {
            //ARRANGE
            string name = "Customer Name Test";
            string pin = "0101010000";
            double amount = 10;
            
            _customerWriteOnlyRepository.Setup(m => m.Add(It.IsAny<Customer>())).Returns(Task.CompletedTask);
            _accountWriteOnlyRepository.Setup(m => m.Add(It.IsAny<Account>(), It.IsAny<Credit>())).Returns(Task.CompletedTask);

            //ACT
            RegisterOutput outPut = await registerUseCase.Execute(pin, name, amount);

            //ASSERT
            _customerWriteOnlyRepository.Verify(v => v.Add(It.IsAny<Customer>()), Times.Once());
            _accountWriteOnlyRepository.Verify(v => v.Add(It.IsAny<Account>(), It.IsAny<Credit>()), Times.Once());
            Assert.Equal(outPut.Customer.Name, name);
            Assert.Equal(outPut.Account.CurrentBalance, amount);
        }

        [Fact]
        public async Task Register_InvalidSSN_ShouldThrowAnDomainException()
        {
            //ARRANGE
            string name = "Customer Name Test";
            string pin = "abc";
            double amount = 10;

            //ACT
            async Task<RegisterOutput> function() => await registerUseCase.Execute(pin, name, amount);

            //ASSERT
            await Assert.ThrowsAnyAsync<DomainException>(function);
        }

        [Fact]
        public async Task Register_EmptySSN_ShouldThrowASSNShouldNotBeEmptyException()
        {
            //ARRANGE
            var pin = string.Empty;
            string name = "Customer Name Test";
            double amount = 10;

            //ACT
            async Task<RegisterOutput> function() => await registerUseCase.Execute(pin, name, amount);

            //ASSERT
            await Assert.ThrowsAnyAsync<clean_full.Domain.ValueObjects.SSNShouldNotBeEmptyException>(function);
        }

        [Fact]
        public async Task Register_EmptySSN_ShouldThrowANameShouldNotBeEmptyException()
        {
            //ARRANGE
            var name = string.Empty;
            var pin = "0101010000";
            double amount = 10;

            //ACT
            async Task<RegisterOutput> function() => await registerUseCase.Execute(pin, name, amount);

            //ASSERT
            await Assert.ThrowsAnyAsync<clean_full.Domain.ValueObjects.NameShouldNotBeEmptyException>(function);
        }
    }
}
