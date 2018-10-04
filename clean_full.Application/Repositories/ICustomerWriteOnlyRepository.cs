namespace clean_full.Application.Repositories
{
    using clean_full.Domain.Customers;
    using System.Threading.Tasks;

    public interface ICustomerWriteOnlyRepository
    {
        Task Add(Customer customer);
        Task Update(Customer customer);
    }
}
