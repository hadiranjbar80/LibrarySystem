using Application.Category.Dtos;
using AutoMapper;

namespace Application.Core
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Domain.Entities.Category, CategoryDto>();
        }
    }
}
