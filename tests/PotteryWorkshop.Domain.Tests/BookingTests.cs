using FluentAssertions;
using PotteryWorkshop.Domain.Entities;
using Xunit;

namespace PotteryWorkshop.Domain.Tests;

public class BookingTests
{
    [Fact]
    public void Booking_ShouldBeCreated_WithValidData()
    {
        // Arrange & Act
        var booking = new Booking
        {
            WorkshopId = 1,
            CustomerId = 1,
            BookingDate = DateTime.UtcNow,
            NumberOfParticipants = 2,
            TotalAmount = 200.00m,
            Status = BookingStatus.Pending
        };

        // Assert
        booking.WorkshopId.Should().Be(1);
        booking.CustomerId.Should().Be(1);
        booking.NumberOfParticipants.Should().Be(2);
        booking.Status.Should().Be(BookingStatus.Pending);
    }

    [Theory]
    [InlineData(BookingStatus.Pending)]
    [InlineData(BookingStatus.Confirmed)]
    [InlineData(BookingStatus.Cancelled)]
    [InlineData(BookingStatus.Completed)]
    public void Booking_ShouldAcceptAllBookingStatuses(BookingStatus status)
    {
        // Arrange & Act
        var booking = new Booking { Status = status };

        // Assert
        booking.Status.Should().Be(status);
    }
}
