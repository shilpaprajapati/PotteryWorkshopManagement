using PotteryWorkshop.Domain.Entities;
using PotteryWorkshop.Domain.Enums;

namespace PotteryWorkshop.Application.Common.Interfaces;

public interface IBookingService
{
    Task<Booking?> CreateBookingAsync(
        Guid workshopId,
        Guid slotId,
        string customerName,
        string customerEmail,
        string customerPhone,
        int numberOfPeople,
        string? specialRequests,
        List<BookingParticipant>? additionalParticipants,
        string? couponCode);

    Task<Booking?> GetBookingAsync(Guid bookingId);
    Task<bool> CancelBookingAsync(Guid bookingId, string reason);
    Task<decimal> CalculatePriceAsync(Guid workshopId, int numberOfPeople, string? couponCode);
}
