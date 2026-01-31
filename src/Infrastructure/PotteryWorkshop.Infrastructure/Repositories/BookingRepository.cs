using Microsoft.EntityFrameworkCore;
using PotteryWorkshop.Domain.Entities;
using PotteryWorkshop.Domain.Interfaces;
using PotteryWorkshop.Infrastructure.Data;

namespace PotteryWorkshop.Infrastructure.Repositories;

public class BookingRepository : Repository<Booking>, IBookingRepository
{
    public BookingRepository(ApplicationDbContext context) : base(context)
    {
    }

    public override async Task<Booking?> GetByIdAsync(int id)
    {
        return await _dbSet
            .Include(b => b.Workshop)
            .Include(b => b.Customer)
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    public override async Task<IEnumerable<Booking>> GetAllAsync()
    {
        return await _dbSet
            .Include(b => b.Workshop)
            .Include(b => b.Customer)
            .ToListAsync();
    }

    public async Task<IEnumerable<Booking>> GetBookingsByWorkshopIdAsync(int workshopId)
    {
        return await _dbSet
            .Include(b => b.Workshop)
            .Include(b => b.Customer)
            .Where(b => b.WorkshopId == workshopId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Booking>> GetBookingsByCustomerIdAsync(int customerId)
    {
        return await _dbSet
            .Include(b => b.Workshop)
            .Include(b => b.Customer)
            .Where(b => b.CustomerId == customerId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Booking>> GetBookingsByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _dbSet
            .Include(b => b.Workshop)
            .Include(b => b.Customer)
            .Where(b => b.BookingDate >= startDate && b.BookingDate <= endDate)
            .ToListAsync();
    }
}
