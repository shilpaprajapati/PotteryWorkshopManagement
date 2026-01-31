using PotteryWorkshop.Domain.Entities;

namespace PotteryWorkshop.Domain.Interfaces;

public interface IBookingRepository : IRepository<Booking>
{
    Task<IEnumerable<Booking>> GetBookingsByWorkshopIdAsync(int workshopId);
    Task<IEnumerable<Booking>> GetBookingsByCustomerIdAsync(int customerId);
    Task<IEnumerable<Booking>> GetBookingsByDateRangeAsync(DateTime startDate, DateTime endDate);
}
