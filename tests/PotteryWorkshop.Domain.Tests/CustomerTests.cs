using FluentAssertions;
using PotteryWorkshop.Domain.Entities;
using Xunit;

namespace PotteryWorkshop.Domain.Tests;

public class CustomerTests
{
    [Fact]
    public void Customer_ShouldBeCreated_WithValidData()
    {
        // Arrange & Act
        var customer = new Customer
        {
            FirstName = "Jane",
            LastName = "Smith",
            Email = "jane.smith@example.com",
            PhoneNumber = "123-456-7890"
        };

        // Assert
        customer.FirstName.Should().Be("Jane");
        customer.LastName.Should().Be("Smith");
        customer.Email.Should().Be("jane.smith@example.com");
    }

    [Fact]
    public void Customer_ShouldHaveEmptyBookings_WhenCreated()
    {
        // Arrange & Act
        var customer = new Customer();

        // Assert
        customer.Bookings.Should().BeEmpty();
    }
}
