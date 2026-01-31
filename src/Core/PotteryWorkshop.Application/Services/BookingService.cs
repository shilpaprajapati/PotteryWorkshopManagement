using AutoMapper;
using PotteryWorkshop.Application.DTOs;
using PotteryWorkshop.Domain.Entities;
using PotteryWorkshop.Domain.Interfaces;

namespace PotteryWorkshop.Application.Services;

public class BookingService : IBookingService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public BookingService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<BookingDto>> GetAllBookingsAsync()
    {
        var bookings = await _unitOfWork.Bookings.GetAllAsync();
        return _mapper.Map<IEnumerable<BookingDto>>(bookings);
    }

    public async Task<BookingDto?> GetBookingByIdAsync(int id)
    {
        var booking = await _unitOfWork.Bookings.GetByIdAsync(id);
        return booking == null ? null : _mapper.Map<BookingDto>(booking);
    }

    public async Task<IEnumerable<BookingDto>> GetBookingsByWorkshopIdAsync(int workshopId)
    {
        var bookings = await _unitOfWork.Bookings.GetBookingsByWorkshopIdAsync(workshopId);
        return _mapper.Map<IEnumerable<BookingDto>>(bookings);
    }

    public async Task<IEnumerable<BookingDto>> GetBookingsByCustomerIdAsync(int customerId)
    {
        var bookings = await _unitOfWork.Bookings.GetBookingsByCustomerIdAsync(customerId);
        return _mapper.Map<IEnumerable<BookingDto>>(bookings);
    }

    public async Task<BookingDto> CreateBookingAsync(CreateBookingDto createBookingDto)
    {
        // Validate workshop exists and has available slots
        var workshop = await _unitOfWork.Workshops.GetByIdAsync(createBookingDto.WorkshopId);
        if (workshop == null)
        {
            throw new KeyNotFoundException($"Workshop with ID {createBookingDto.WorkshopId} not found.");
        }

        var existingBookings = await _unitOfWork.Bookings.GetBookingsByWorkshopIdAsync(createBookingDto.WorkshopId);
        var bookedSlots = existingBookings.Sum(b => b.NumberOfParticipants);
        var availableSlots = workshop.MaxParticipants - bookedSlots;

        if (createBookingDto.NumberOfParticipants > availableSlots)
        {
            throw new InvalidOperationException($"Not enough available slots. Only {availableSlots} slots available.");
        }

        var booking = _mapper.Map<Booking>(createBookingDto);
        booking.TotalAmount = workshop.Price * createBookingDto.NumberOfParticipants;
        
        var createdBooking = await _unitOfWork.Bookings.AddAsync(booking);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<BookingDto>(createdBooking);
    }

    public async Task UpdateBookingAsync(UpdateBookingDto updateBookingDto)
    {
        var booking = await _unitOfWork.Bookings.GetByIdAsync(updateBookingDto.Id);
        if (booking == null)
        {
            throw new KeyNotFoundException($"Booking with ID {updateBookingDto.Id} not found.");
        }

        booking.NumberOfParticipants = updateBookingDto.NumberOfParticipants;
        booking.Notes = updateBookingDto.Notes;
        booking.UpdatedAt = DateTime.UtcNow;
        
        if (Enum.TryParse<BookingStatus>(updateBookingDto.Status, out var status))
        {
            booking.Status = status;
        }

        await _unitOfWork.Bookings.UpdateAsync(booking);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task CancelBookingAsync(int id)
    {
        var booking = await _unitOfWork.Bookings.GetByIdAsync(id);
        if (booking == null)
        {
            throw new KeyNotFoundException($"Booking with ID {id} not found.");
        }

        booking.Status = BookingStatus.Cancelled;
        booking.UpdatedAt = DateTime.UtcNow;
        
        await _unitOfWork.Bookings.UpdateAsync(booking);
        await _unitOfWork.SaveChangesAsync();
    }
}
