using Microsoft.EntityFrameworkCore;
using PotteryWorkshop.Domain.Entities;
using PotteryWorkshop.Domain.Interfaces;
using PotteryWorkshop.Infrastructure.Data;

namespace PotteryWorkshop.Infrastructure.Repositories;

public class WorkshopRepository : Repository<Workshop>, IWorkshopRepository
{
    public WorkshopRepository(ApplicationDbContext context) : base(context)
    {
    }

    public override async Task<Workshop?> GetByIdAsync(int id)
    {
        return await _dbSet
            .Include(w => w.Bookings)
            .FirstOrDefaultAsync(w => w.Id == id);
    }

    public override async Task<IEnumerable<Workshop>> GetAllAsync()
    {
        return await _dbSet
            .Include(w => w.Bookings)
            .ToListAsync();
    }

    public async Task<IEnumerable<Workshop>> GetActiveWorkshopsAsync()
    {
        return await _dbSet
            .Include(w => w.Bookings)
            .Where(w => w.IsActive)
            .ToListAsync();
    }

    public async Task<IEnumerable<Workshop>> GetUpcomingWorkshopsAsync()
    {
        return await _dbSet
            .Include(w => w.Bookings)
            .Where(w => w.IsActive && w.StartDate > DateTime.UtcNow)
            .OrderBy(w => w.StartDate)
            .ToListAsync();
    }
}
