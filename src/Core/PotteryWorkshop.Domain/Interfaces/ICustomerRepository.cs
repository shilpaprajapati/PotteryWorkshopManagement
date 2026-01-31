using PotteryWorkshop.Domain.Entities;

namespace PotteryWorkshop.Domain.Interfaces;

public interface ICustomerRepository : IRepository<Customer>
{
    Task<Customer?> GetByEmailAsync(string email);
}
