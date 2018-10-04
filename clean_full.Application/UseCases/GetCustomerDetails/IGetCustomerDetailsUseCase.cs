namespace clean_full.Application.UseCases.GetCustomerDetails
{
    using System;
    using System.Threading.Tasks;

    public interface IGetCustomerDetailsUseCase
    {
        Task<CustomerOutput> Execute(Guid customerId);
    }
}