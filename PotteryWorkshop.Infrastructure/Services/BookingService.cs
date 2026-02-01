using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PotteryWorkshop.Application.Common.Interfaces;
using PotteryWorkshop.Domain.Entities;
using PotteryWorkshop.Domain.Enums;

namespace PotteryWorkshop.Infrastructure.Services;

public class BookingService : IBookingService
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<BookingService> _logger;

    public BookingService(IApplicationDbContext context, ILogger<BookingService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Booking?> CreateBookingAsync(
        Guid workshopId,
        Guid slotId,
        string customerName,
        string customerEmail,
        string customerPhone,
        int numberOfPeople,
        string? specialRequests,
        List<BookingParticipant>? additionalParticipants,
        string? couponCode)
    {
        try
        {
            var workshop = await _context.Workshops.FindAsync(workshopId);
            var slot = await _context.WorkshopSlots.FindAsync(slotId);

            if (workshop == null || slot == null)
            {
                _logger.LogWarning("Workshop or slot not found: Workshop {WorkshopId}, Slot {SlotId}", workshopId, slotId);
                return null;
            }

            // Check slot availability
            if (slot.AvailableCapacity < numberOfPeople)
            {
                _logger.LogWarning("Slot capacity exceeded for slot {SlotId}", slotId);
                return null;
            }

            // Calculate pricing
            var totalAmount = await CalculatePriceAsync(workshopId, numberOfPeople, couponCode);
            var discountAmount = 0m;

            // Apply coupon if provided
            if (!string.IsNullOrEmpty(couponCode))
            {
                var coupon = await _context.Coupons
                    .FirstOrDefaultAsync(c => c.Code == couponCode && c.IsActive && c.ValidFrom <= DateTime.UtcNow && c.ValidUntil >= DateTime.UtcNow);

                if (coupon != null && coupon.CurrentUses < coupon.MaxUses)
                {
                    if (coupon.DiscountPercentage > 0)
                    {
                        discountAmount = totalAmount * (coupon.DiscountPercentage / 100);
                    }
                    else if (coupon.DiscountAmount.HasValue && coupon.DiscountAmount.Value > 0)
                    {
                        discountAmount = coupon.DiscountAmount.Value;
                    }

                    coupon.CurrentUses++;
                    coupon.UpdatedAt = DateTime.UtcNow;
                }
            }

            var finalAmount = totalAmount - discountAmount;

            var booking = new Booking
            {
                Id = Guid.NewGuid(),
                BookingNumber = GenerateBookingNumber(),
                WorkshopId = workshopId,
                SlotId = slotId,
                CustomerName = customerName,
                CustomerEmail = customerEmail,
                CustomerPhone = customerPhone,
                NumberOfPeople = numberOfPeople,
                TotalAmount = totalAmount,
                DiscountAmount = discountAmount,
                FinalAmount = finalAmount,
                CouponCode = couponCode,
                Status = BookingStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };

            _context.Bookings.Add(booking);

            // Add additional participants
            if (additionalParticipants != null && additionalParticipants.Any())
            {
                foreach (var participant in additionalParticipants)
                {
                    participant.BookingId = booking.Id;
                    participant.CreatedAt = DateTime.UtcNow;
                    _context.BookingParticipants.Add(participant);
                }
            }

            // Update slot capacity
            slot.AvailableCapacity -= numberOfPeople;
            slot.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            _logger.LogInformation("Booking created successfully: {BookingNumber}", booking.BookingNumber);
            return booking;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating booking for workshop {WorkshopId}", workshopId);
            return null;
        }
    }

    public async Task<Booking?> GetBookingAsync(Guid bookingId)
    {
        return await _context.Bookings
            .Include(b => b.Workshop)
            .Include(b => b.Slot)
            .Include(b => b.Participants)
            .Include(b => b.Payments)
            .FirstOrDefaultAsync(b => b.Id == bookingId);
    }

    public async Task<bool> CancelBookingAsync(Guid bookingId, string reason)
    {
        try
        {
            var booking = await _context.Bookings
                .Include(b => b.Slot)
                .FirstOrDefaultAsync(b => b.Id == bookingId);

            if (booking == null)
            {
                return false;
            }

            // Update booking status
            booking.Status = BookingStatus.Cancelled;
            booking.UpdatedAt = DateTime.UtcNow;

            // Release slot capacity
            if (booking.Slot != null)
            {
                booking.Slot.AvailableCapacity += booking.NumberOfPeople;
                booking.Slot.UpdatedAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();

            _logger.LogInformation("Booking cancelled: {BookingNumber}, Reason: {Reason}", booking.BookingNumber, reason);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cancelling booking {BookingId}", bookingId);
            return false;
        }
    }

    public async Task<decimal> CalculatePriceAsync(Guid workshopId, int numberOfPeople, string? couponCode)
    {
        var workshop = await _context.Workshops.FindAsync(workshopId);
        if (workshop == null)
        {
            return 0;
        }

        // Determine price based on number of people
        decimal totalAmount;
        if (numberOfPeople == 2 && workshop.PriceForTwo > 0)
        {
            totalAmount = workshop.PriceForTwo;
        }
        else
        {
            totalAmount = workshop.PricePerPerson * numberOfPeople;
        }

        return totalAmount;
    }

    private string GenerateBookingNumber()
    {
        return $"BK{DateTime.UtcNow:yyyyMMddHHmmss}{Random.Shared.Next(1000, 9999)}";
    }
}
