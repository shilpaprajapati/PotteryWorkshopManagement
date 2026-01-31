using PotteryWorkshop.Domain.Entities;

namespace PotteryWorkshop.Domain.Interfaces;

public interface IWorkshopRepository : IRepository<Workshop>
{
    Task<IEnumerable<Workshop>> GetActiveWorkshopsAsync();
    Task<IEnumerable<Workshop>> GetUpcomingWorkshopsAsync();
}
