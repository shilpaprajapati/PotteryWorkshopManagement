using PotteryWorkshop.Domain.Interfaces;
using PotteryWorkshop.Infrastructure.Data;

namespace PotteryWorkshop.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private IWorkshopRepository? _workshopRepository;
    private IBookingRepository? _bookingRepository;
    private ICustomerRepository? _customerRepository;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    public IWorkshopRepository Workshops => _workshopRepository ??= new WorkshopRepository(_context);
    public IBookingRepository Bookings => _bookingRepository ??= new BookingRepository(_context);
    public ICustomerRepository Customers => _customerRepository ??= new CustomerRepository(_context);

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
