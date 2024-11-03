using AutoMapper;
using UrlShortenerAPI.DTOs;
using UrlShortenerAPI.Models;

namespace UrlShortenerAPI
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<URL, ShortUrlResponseDto>();
            CreateMap<CreateShortUrlDto, URL>();
        }
    }
}
