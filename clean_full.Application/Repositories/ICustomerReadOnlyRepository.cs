namespace clean_full.Application.Repositories
{
    using clean_full.Domain.Customers;
    using System;
    using System.Threading.Tasks;

    public interface ICustomerReadOnlyRepository
    {
        Task<Customer> Get(Guid id);
    }
}
