using AutoMapper;
using Netzwerk.DTOs;
using Netzwerk.Model;

namespace Netzwerk.Mappers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, UsersDto>()
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
            .ForMember(dest => dest.Email,
                opt => opt.MapFrom(src =>
                    src.Email));
        CreateMap<MarkerDto, Marker>();
        CreateMap<Marker, MarkerDto>();
        CreateMap<Map, MapDto>();
        CreateMap<MapDto, Map>();
        CreateMap<CategoryDto, Category>();
        CreateMap<Category, CategoryDto>();
        CreateMap<Vote, VoteDto>();
        CreateMap<UsersDto, User>();
        CreateMap<VoteDto, Vote>();
    }
}