using Microsoft.EntityFrameworkCore;
using PotteryWorkshop.Domain.Entities;
using PotteryWorkshop.Domain.Enums;

namespace PotteryWorkshop.Infrastructure.Data;

public static class DbInitializer
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        await context.Database.MigrateAsync();

        // Check if data already exists
        if (await context.Workshops.AnyAsync())
        {
            return; // DB has been seeded
        }

        // Seed Workshops
        var workshops = new List<Workshop>
        {
            new Workshop
            {
                Id = Guid.NewGuid(),
                Name = "⭐ 7.0 Premium Experience",
                Description = "Complete pottery experience with professional photography and videography. Perfect for couples and friends looking for a memorable creative escape.",
                DurationInMinutes = 180,
                MaxCapacity = 2,
                PricePerPerson = 7000,
                PriceForTwo = 7000,
                ImageUrl = "https://images.unsplash.com/photo-1493106641515-6b5631de4bb9?w=800",
                InstagramReelUrl = "https://www.instagram.com/reel/DL6pQacRGPE/",
                Inclusions = "• Pro photos + cinematic video\n• 2 reels | 50–70 DSLR shots\n• Ceramic painting\n• Pottery wheel + hand-building\n• Fired & glazed product",
                IsActive = true,
                Status = WorkshopStatus.Scheduled,
                CreatedAt = DateTime.UtcNow
            },
            new Workshop
            {
                Id = Guid.NewGuid(),
                Name = "⭐ 5.0 Deluxe Experience",
                Description = "Professional pottery experience with photos and video documentation. Includes personal team guidance throughout the session.",
                DurationInMinutes = 180,
                MaxCapacity = 2,
                PricePerPerson = 5000,
                PriceForTwo = 5000,
                ImageUrl = "https://images.unsplash.com/photo-1578749556568-bc2c40e68b61?w=800",
                InstagramReelUrl = "https://www.instagram.com/reel/DAxrw3VSjM2/",
                Inclusions = "• Pro photos + video\n• 1 reel | 40–50 DSLR shots\n• Pottery wheel + hand-building\n• Personal team guidance\n• Fired & finished product",
                IsActive = true,
                Status = WorkshopStatus.Scheduled,
                CreatedAt = DateTime.UtcNow
            },
            new Workshop
            {
                Id = Guid.NewGuid(),
                Name = "⭐ 4.0 Artistic Experience",
                Description = "Creative pottery session with artistic photography. Great introduction to pottery making with beautiful memories captured.",
                DurationInMinutes = 120,
                MaxCapacity = 2,
                PricePerPerson = 4000,
                PriceForTwo = 4000,
                ImageUrl = "https://images.unsplash.com/photo-1610701596007-11502861dcfa?w=800",
                InstagramReelUrl = "https://www.instagram.com/reel/C_Kpu0SypnT/",
                Inclusions = "• Artistic photos + video\n• 1 reel | 35–40 DSLR shots\n• Pottery wheel + hand-building\n• Team guidance\n• Finished product",
                IsActive = true,
                Status = WorkshopStatus.Scheduled,
                CreatedAt = DateTime.UtcNow
            },
            new Workshop
            {
                Id = Guid.NewGuid(),
                Name = "2 Hours Basic Slot",
                Description = "Perfect introduction to pottery making. Learn the basics and create your first pottery pieces.",
                DurationInMinutes = 120,
                MaxCapacity = 2,
                PricePerPerson = 1200,
                PriceForTwo = 1700,
                ImageUrl = "https://images.unsplash.com/photo-1565193566173-7a0ee3dbe261?w=800",
                Inclusions = "• Basic introduction\n• Glass/Bowl\n• Chai Kulhad\n• Wheel cleaning",
                IsActive = true,
                Status = WorkshopStatus.Scheduled,
                CreatedAt = DateTime.UtcNow
            },
            new Workshop
            {
                Id = Guid.NewGuid(),
                Name = "3 Hours Advanced Slot",
                Description = "Extended pottery session with advanced techniques. Build on basics and create more complex pieces.",
                DurationInMinutes = 180,
                MaxCapacity = 2,
                PricePerPerson = 1900,
                PriceForTwo = 2200,
                ImageUrl = "https://images.unsplash.com/photo-1565123409695-7b5ef7589?w=800",
                Inclusions = "• Includes 2 hrs items\n• Medium sized Bowl\n• Mini flower pot\n• Advanced hand building",
                IsActive = true,
                Status = WorkshopStatus.Scheduled,
                CreatedAt = DateTime.UtcNow
            },
            new Workshop
            {
                Id = Guid.NewGuid(),
                Name = "1 Hour Kids Special",
                Description = "Fun and creative pottery session designed specifically for children. Let them explore their creativity!",
                DurationInMinutes = 60,
                MaxCapacity = 2,
                PricePerPerson = 750,
                PriceForTwo = 1100,
                ImageUrl = "https://images.unsplash.com/photo-1503454537195-1dcabb73ffb9?w=800",
                Inclusions = "• Basic introduction\n• 3 types of Diya 🪔\n• Wheel cleaning\n• Fun activities for kids",
                IsActive = true,
                Status = WorkshopStatus.Scheduled,
                CreatedAt = DateTime.UtcNow
            }
        };

        await context.Workshops.AddRangeAsync(workshops);
        await context.SaveChangesAsync();

        // Seed Hero Images
        var heroImages = new List<HeroImage>
        {
            new HeroImage
            {
                Id = Guid.NewGuid(),
                ImageUrl = "https://images.unsplash.com/photo-1565193566173-7a0ee3dbe261?w=1600",
                Title = "Discover the Art of Pottery",
                Description = "Create beautiful pottery pieces with expert guidance",
                DisplayOrder = 1,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new HeroImage
            {
                Id = Guid.NewGuid(),
                ImageUrl = "https://images.unsplash.com/photo-1578749556568-bc2c40e68b61?w=1600",
                Title = "Unleash Your Creativity",
                Description = "Experience the joy of making pottery",
                DisplayOrder = 2,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new HeroImage
            {
                Id = Guid.NewGuid(),
                ImageUrl = "https://images.unsplash.com/photo-1610701596007-11502861dcfa?w=1600",
                Title = "Create Lasting Memories",
                Description = "Perfect for couples, friends, and families",
                DisplayOrder = 3,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            }
        };

        await context.HeroImages.AddRangeAsync(heroImages);
        await context.SaveChangesAsync();

        // Seed Sample Coupons
        var coupons = new List<Coupon>
        {
            new Coupon
            {
                Id = Guid.NewGuid(),
                Code = "WELCOME10",
                Description = "Welcome discount - 10% off",
                DiscountPercentage = 10,
                ValidFrom = DateTime.UtcNow,
                ValidUntil = DateTime.UtcNow.AddMonths(3),
                MaxUses = 100,
                CurrentUses = 0,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new Coupon
            {
                Id = Guid.NewGuid(),
                Code = "FIRST500",
                Description = "First booking - Flat ₹500 off",
                DiscountPercentage = 0,
                DiscountAmount = 500,
                ValidFrom = DateTime.UtcNow,
                ValidUntil = DateTime.UtcNow.AddMonths(6),
                MaxUses = 50,
                CurrentUses = 0,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            }
        };

        await context.Coupons.AddRangeAsync(coupons);
        await context.SaveChangesAsync();

        // Create sample admin user (password: Admin@123)
        var adminUser = new User
        {
            Id = Guid.NewGuid(),
            Email = "admin@potteryworkshop.com",
            PasswordHash = "AQAAAAIAAYagAAAAEGxJ8V7zK8N8Y5qF9vH3yQ==", // This should be properly hashed in production
            FirstName = "Admin",
            LastName = "User",
            Phone = "+919876543210",
            IsAdmin = true,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        await context.Users.AddAsync(adminUser);
        await context.SaveChangesAsync();
    }
}
