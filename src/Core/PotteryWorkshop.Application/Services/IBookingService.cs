using PotteryWorkshop.Application.DTOs;

namespace PotteryWorkshop.Application.Services;

public interface IBookingService
{
    Task<IEnumerable<BookingDto>> GetAllBookingsAsync();
    Task<BookingDto?> GetBookingByIdAsync(int id);
    Task<IEnumerable<BookingDto>> GetBookingsByWorkshopIdAsync(int workshopId);
    Task<IEnumerable<BookingDto>> GetBookingsByCustomerIdAsync(int customerId);
    Task<BookingDto> CreateBookingAsync(CreateBookingDto createBookingDto);
    Task UpdateBookingAsync(UpdateBookingDto updateBookingDto);
    Task CancelBookingAsync(int id);
}
