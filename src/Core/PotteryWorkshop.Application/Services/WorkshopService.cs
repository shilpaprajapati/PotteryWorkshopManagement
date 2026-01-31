using AutoMapper;
using PotteryWorkshop.Application.DTOs;
using PotteryWorkshop.Domain.Entities;
using PotteryWorkshop.Domain.Interfaces;

namespace PotteryWorkshop.Application.Services;

public class WorkshopService : IWorkshopService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public WorkshopService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<WorkshopDto>> GetAllWorkshopsAsync()
    {
        var workshops = await _unitOfWork.Workshops.GetAllAsync();
        return _mapper.Map<IEnumerable<WorkshopDto>>(workshops);
    }

    public async Task<IEnumerable<WorkshopDto>> GetActiveWorkshopsAsync()
    {
        var workshops = await _unitOfWork.Workshops.GetActiveWorkshopsAsync();
        return _mapper.Map<IEnumerable<WorkshopDto>>(workshops);
    }

    public async Task<IEnumerable<WorkshopDto>> GetUpcomingWorkshopsAsync()
    {
        var workshops = await _unitOfWork.Workshops.GetUpcomingWorkshopsAsync();
        return _mapper.Map<IEnumerable<WorkshopDto>>(workshops);
    }

    public async Task<WorkshopDto?> GetWorkshopByIdAsync(int id)
    {
        var workshop = await _unitOfWork.Workshops.GetByIdAsync(id);
        return workshop == null ? null : _mapper.Map<WorkshopDto>(workshop);
    }

    public async Task<WorkshopDto> CreateWorkshopAsync(CreateWorkshopDto createWorkshopDto)
    {
        var workshop = _mapper.Map<Workshop>(createWorkshopDto);
        var createdWorkshop = await _unitOfWork.Workshops.AddAsync(workshop);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<WorkshopDto>(createdWorkshop);
    }

    public async Task UpdateWorkshopAsync(UpdateWorkshopDto updateWorkshopDto)
    {
        var workshop = await _unitOfWork.Workshops.GetByIdAsync(updateWorkshopDto.Id);
        if (workshop == null)
        {
            throw new KeyNotFoundException($"Workshop with ID {updateWorkshopDto.Id} not found.");
        }

        _mapper.Map(updateWorkshopDto, workshop);
        await _unitOfWork.Workshops.UpdateAsync(workshop);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteWorkshopAsync(int id)
    {
        var workshop = await _unitOfWork.Workshops.GetByIdAsync(id);
        if (workshop == null)
        {
            throw new KeyNotFoundException($"Workshop with ID {id} not found.");
        }

        await _unitOfWork.Workshops.DeleteAsync(workshop);
        await _unitOfWork.SaveChangesAsync();
    }
}
