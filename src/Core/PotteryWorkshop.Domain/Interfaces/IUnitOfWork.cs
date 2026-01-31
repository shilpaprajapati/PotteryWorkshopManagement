namespace PotteryWorkshop.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IWorkshopRepository Workshops { get; }
    IBookingRepository Bookings { get; }
    ICustomerRepository Customers { get; }
    Task<int> SaveChangesAsync();
}
