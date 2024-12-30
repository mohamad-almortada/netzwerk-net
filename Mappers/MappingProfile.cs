    using AutoMapper;
    using Netzwerk.DTOs;
    using Netzwerk.Model;

namespace Netzwerk.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Keyword, KeywordDto>();
        }
    }
}