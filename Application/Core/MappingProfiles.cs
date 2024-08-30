using Application.Book.Dtos;
using Application.Category.Dtos;
using Application.Lending.Dtos;
using Application.Subscription.Dtos;
using AutoMapper;

namespace Application.Core
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Domain.Entities.Category, CategoryDto>();
            CreateMap<Domain.Entities.Book, BookListDto>();
            CreateMap<Domain.Entities.Book, BookDto>()
                .ForMember(x=>x.CategoryId, o=>o.MapFrom(s=>s.Category.Id));
            CreateMap<BookDto ,Domain.Entities.Book>();
            CreateMap<Domain.Entities.Subscription, SubscriptionDto>();
            CreateMap<SubscriptionDto, Domain.Entities.Subscription>();
            CreateMap<Domain.Entities.Lending, LendingListDto>()
                .ForMember(x => x.BookName, o => o.MapFrom(s => s.Book.Name))
                .ForMember(x => x.BookCode, o => o.MapFrom(s => s.Book.Code))
                .ForMember(x => x.UserFirstName, o => o.MapFrom(s => s.AppUser.FirstName))
                .ForMember(x => x.UserLasttName, o => o.MapFrom(s => s.AppUser.LastName));
        }
    }
}
