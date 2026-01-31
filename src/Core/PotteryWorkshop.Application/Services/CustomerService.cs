using AutoMapper;
using PotteryWorkshop.Application.DTOs;
using PotteryWorkshop.Domain.Entities;
using PotteryWorkshop.Domain.Interfaces;

namespace PotteryWorkshop.Application.Services;

public class CustomerService : ICustomerService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CustomerService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CustomerDto>> GetAllCustomersAsync()
    {
        var customers = await _unitOfWork.Customers.GetAllAsync();
        return _mapper.Map<IEnumerable<CustomerDto>>(customers);
    }

    public async Task<CustomerDto?> GetCustomerByIdAsync(int id)
    {
        var customer = await _unitOfWork.Customers.GetByIdAsync(id);
        return customer == null ? null : _mapper.Map<CustomerDto>(customer);
    }

    public async Task<CustomerDto?> GetCustomerByEmailAsync(string email)
    {
        var customer = await _unitOfWork.Customers.GetByEmailAsync(email);
        return customer == null ? null : _mapper.Map<CustomerDto>(customer);
    }

    public async Task<CustomerDto> CreateCustomerAsync(CreateCustomerDto createCustomerDto)
    {
        // Check if customer with email already exists
        var existingCustomer = await _unitOfWork.Customers.GetByEmailAsync(createCustomerDto.Email);
        if (existingCustomer != null)
        {
            throw new InvalidOperationException($"Customer with email {createCustomerDto.Email} already exists.");
        }

        var customer = _mapper.Map<Customer>(createCustomerDto);
        var createdCustomer = await _unitOfWork.Customers.AddAsync(customer);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<CustomerDto>(createdCustomer);
    }

    public async Task UpdateCustomerAsync(UpdateCustomerDto updateCustomerDto)
    {
        var customer = await _unitOfWork.Customers.GetByIdAsync(updateCustomerDto.Id);
        if (customer == null)
        {
            throw new KeyNotFoundException($"Customer with ID {updateCustomerDto.Id} not found.");
        }

        _mapper.Map(updateCustomerDto, customer);
        await _unitOfWork.Customers.UpdateAsync(customer);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteCustomerAsync(int id)
    {
        var customer = await _unitOfWork.Customers.GetByIdAsync(id);
        if (customer == null)
        {
            throw new KeyNotFoundException($"Customer with ID {id} not found.");
        }

        await _unitOfWork.Customers.DeleteAsync(customer);
        await _unitOfWork.SaveChangesAsync();
    }
}
