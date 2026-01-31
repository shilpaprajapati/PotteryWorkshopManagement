using Microsoft.EntityFrameworkCore;
using PotteryWorkshop.Application.Common.Interfaces;
using PotteryWorkshop.Domain.Entities;

namespace PotteryWorkshop.Infrastructure.Data;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Workshop> Workshops => Set<Workshop>();
    public DbSet<WorkshopSlot> WorkshopSlots => Set<WorkshopSlot>();
    public DbSet<Booking> Bookings => Set<Booking>();
    public DbSet<BookingParticipant> BookingParticipants => Set<BookingParticipant>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<PaymentLog> PaymentLogs => Set<PaymentLog>();
    public DbSet<Coupon> Coupons => Set<Coupon>();
    public DbSet<User> Users => Set<User>();
    public DbSet<HeroImage> HeroImages => Set<HeroImage>();
    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Workshop Configuration
        modelBuilder.Entity<Workshop>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).IsRequired();
            entity.Property(e => e.PricePerPerson).HasColumnType("decimal(18,2)");
            entity.Property(e => e.PriceForTwo).HasColumnType("decimal(18,2)");
            entity.HasQueryFilter(e => !e.IsDeleted);
        });

        // WorkshopSlot Configuration
        modelBuilder.Entity<WorkshopSlot>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Workshop)
                .WithMany(w => w.Slots)
                .HasForeignKey(e => e.WorkshopId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasQueryFilter(e => !e.IsDeleted);
        });

        // Booking Configuration
        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.BookingNumber).IsRequired().HasMaxLength(50);
            entity.Property(e => e.CustomerName).IsRequired().HasMaxLength(200);
            entity.Property(e => e.CustomerEmail).IsRequired().HasMaxLength(200);
            entity.Property(e => e.CustomerPhone).IsRequired().HasMaxLength(20);
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(18,2)");
            entity.Property(e => e.DiscountAmount).HasColumnType("decimal(18,2)");
            entity.Property(e => e.FinalAmount).HasColumnType("decimal(18,2)");
            
            entity.HasOne(e => e.Workshop)
                .WithMany(w => w.Bookings)
                .HasForeignKey(e => e.WorkshopId)
                .OnDelete(DeleteBehavior.Restrict);
            
            entity.HasOne(e => e.Slot)
                .WithMany(s => s.Bookings)
                .HasForeignKey(e => e.SlotId)
                .OnDelete(DeleteBehavior.Restrict);
            
            entity.HasQueryFilter(e => !e.IsDeleted);
        });

        // BookingParticipant Configuration
        modelBuilder.Entity<BookingParticipant>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Booking)
                .WithMany(b => b.Participants)
                .HasForeignKey(e => e.BookingId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasQueryFilter(e => !e.IsDeleted);
        });

        // Payment Configuration
        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Amount).HasColumnType("decimal(18,2)");
            entity.HasOne(e => e.Booking)
                .WithMany(b => b.Payments)
                .HasForeignKey(e => e.BookingId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasQueryFilter(e => !e.IsDeleted);
        });

        // PaymentLog Configuration
        modelBuilder.Entity<PaymentLog>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Payment)
                .WithMany(p => p.Logs)
                .HasForeignKey(e => e.PaymentId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasQueryFilter(e => !e.IsDeleted);
        });

        // Coupon Configuration
        modelBuilder.Entity<Coupon>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Code).IsRequired().HasMaxLength(50);
            entity.HasIndex(e => e.Code).IsUnique();
            entity.Property(e => e.DiscountPercentage).HasColumnType("decimal(5,2)");
            entity.Property(e => e.DiscountAmount).HasColumnType("decimal(18,2)");
            entity.HasQueryFilter(e => !e.IsDeleted);
        });

        // User Configuration
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(200);
            entity.HasIndex(e => e.Email).IsUnique();
            entity.HasQueryFilter(e => !e.IsDeleted);
        });

        // HeroImage Configuration
        modelBuilder.Entity<HeroImage>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ImageUrl).IsRequired();
            entity.HasQueryFilter(e => !e.IsDeleted);
        });

        // Product Configuration
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Price).HasColumnType("decimal(18,2)");
            entity.HasQueryFilter(e => !e.IsDeleted);
        });
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    entry.Entity.Id = Guid.NewGuid();
                    break;
                case EntityState.Modified:
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    break;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}
