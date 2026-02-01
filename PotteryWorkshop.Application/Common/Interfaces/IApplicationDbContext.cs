using Microsoft.EntityFrameworkCore;
using PotteryWorkshop.Domain.Entities;

namespace PotteryWorkshop.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Workshop> Workshops { get; }
    DbSet<WorkshopSlot> WorkshopSlots { get; }
    DbSet<Booking> Bookings { get; }
    DbSet<BookingParticipant> BookingParticipants { get; }
    DbSet<Payment> Payments { get; }
    DbSet<PaymentLog> PaymentLogs { get; }
    DbSet<Coupon> Coupons { get; }
    DbSet<User> Users { get; }
    DbSet<HeroImage> HeroImages { get; }
    DbSet<Product> Products { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
