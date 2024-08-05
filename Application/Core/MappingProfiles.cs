using Application.Book.Dtos;
using Application.Category.Dtos;
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
        }
    }
}
