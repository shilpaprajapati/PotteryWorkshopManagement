using FluentAssertions;
using PotteryWorkshop.Domain.Entities;
using Xunit;

namespace PotteryWorkshop.Domain.Tests;

public class WorkshopTests
{
    [Fact]
    public void Workshop_ShouldBeCreated_WithValidData()
    {
        // Arrange & Act
        var workshop = new Workshop
        {
            Title = "Pottery Basics",
            Description = "Learn the basics of pottery",
            Instructor = "John Doe",
            StartDate = DateTime.UtcNow.AddDays(7),
            EndDate = DateTime.UtcNow.AddDays(8),
            MaxParticipants = 10,
            Price = 100.00m,
            Location = "Studio A",
            IsActive = true
        };

        // Assert
        workshop.Title.Should().Be("Pottery Basics");
        workshop.Instructor.Should().Be("John Doe");
        workshop.MaxParticipants.Should().Be(10);
        workshop.IsActive.Should().BeTrue();
    }

    [Fact]
    public void Workshop_ShouldHaveEmptyBookings_WhenCreated()
    {
        // Arrange & Act
        var workshop = new Workshop();

        // Assert
        workshop.Bookings.Should().BeEmpty();
    }
}
