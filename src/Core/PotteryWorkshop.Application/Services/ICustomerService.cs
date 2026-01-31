using PotteryWorkshop.Application.DTOs;

namespace PotteryWorkshop.Application.Services;

public interface ICustomerService
{
    Task<IEnumerable<CustomerDto>> GetAllCustomersAsync();
    Task<CustomerDto?> GetCustomerByIdAsync(int id);
    Task<CustomerDto?> GetCustomerByEmailAsync(string email);
    Task<CustomerDto> CreateCustomerAsync(CreateCustomerDto createCustomerDto);
    Task UpdateCustomerAsync(UpdateCustomerDto updateCustomerDto);
    Task DeleteCustomerAsync(int id);
}
