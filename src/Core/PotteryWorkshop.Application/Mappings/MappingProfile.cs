using AutoMapper;
using PotteryWorkshop.Application.DTOs;
using PotteryWorkshop.Domain.Entities;

namespace PotteryWorkshop.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Workshop mappings
        CreateMap<Workshop, WorkshopDto>()
            .ForMember(dest => dest.AvailableSlots, 
                opt => opt.MapFrom(src => src.MaxParticipants - src.Bookings.Sum(b => b.NumberOfParticipants)));
        CreateMap<CreateWorkshopDto, Workshop>()
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(_ => true));
        CreateMap<UpdateWorkshopDto, Workshop>()
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));
        
        // Customer mappings
        CreateMap<Customer, CustomerDto>();
        CreateMap<CreateCustomerDto, Customer>()
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));
        CreateMap<UpdateCustomerDto, Customer>()
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));
        
        // Booking mappings
        CreateMap<Booking, BookingDto>()
            .ForMember(dest => dest.WorkshopTitle, opt => opt.MapFrom(src => src.Workshop.Title))
            .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => $"{src.Customer.FirstName} {src.Customer.LastName}"))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
        CreateMap<CreateBookingDto, Booking>()
            .ForMember(dest => dest.BookingDate, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(_ => BookingStatus.Pending));
    }
}
