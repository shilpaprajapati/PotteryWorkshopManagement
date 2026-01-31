using Microsoft.EntityFrameworkCore;
using PotteryWorkshop.Domain.Entities;
using PotteryWorkshop.Domain.Interfaces;
using PotteryWorkshop.Infrastructure.Data;

namespace PotteryWorkshop.Infrastructure.Repositories;

public class CustomerRepository : Repository<Customer>, ICustomerRepository
{
    public CustomerRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Customer?> GetByEmailAsync(string email)
    {
        return await _dbSet
            .FirstOrDefaultAsync(c => c.Email == email);
    }
}
