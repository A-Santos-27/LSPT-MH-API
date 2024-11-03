using System.Threading.Tasks;
using UrlShortenerAPI.DTOs;

namespace UrlShortenerAPI.Services
{
    public interface IURLShortenerService
    {
        Task<ShortUrlResponseDto> ShortenUrlAsync(CreateShortUrlDto urlDto);

        Task<List<ShortUrlResponseDto>> GetAllShortenedUrlsAsync();

    }
}
