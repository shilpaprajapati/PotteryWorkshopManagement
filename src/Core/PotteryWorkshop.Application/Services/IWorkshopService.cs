using PotteryWorkshop.Application.DTOs;

namespace PotteryWorkshop.Application.Services;

public interface IWorkshopService
{
    Task<IEnumerable<WorkshopDto>> GetAllWorkshopsAsync();
    Task<IEnumerable<WorkshopDto>> GetActiveWorkshopsAsync();
    Task<IEnumerable<WorkshopDto>> GetUpcomingWorkshopsAsync();
    Task<WorkshopDto?> GetWorkshopByIdAsync(int id);
    Task<WorkshopDto> CreateWorkshopAsync(CreateWorkshopDto createWorkshopDto);
    Task UpdateWorkshopAsync(UpdateWorkshopDto updateWorkshopDto);
    Task DeleteWorkshopAsync(int id);
}
