using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Newtonsoft.Json;
using UrlShortenerAPI.Data;
using UrlShortenerAPI.DTOs;
using UrlShortenerAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace UrlShortenerAPI.Services
{
    public class URLShortenerService : IURLShortenerService
    {
        private readonly URLDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly HttpClient _httpClient;

        public URLShortenerService(URLDbContext dbContext, IMapper mapper, HttpClient httpClient)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _httpClient = httpClient;
        }

        public async Task<ShortUrlResponseDto> ShortenUrlAsync(CreateShortUrlDto urlDto)
        {
            var existingUrl = await _dbContext.URLs.FirstOrDefaultAsync(u => u.OriginalUrl == urlDto.OriginalUrl);
            if (existingUrl != null)
            {
                return _mapper.Map<ShortUrlResponseDto>(existingUrl);
            }

            var shortUrl = await CreateShortUrlOnUlvisNetAsync(urlDto.OriginalUrl);

            var newUrl = new URL
            {
                OriginalUrl = urlDto.OriginalUrl,
                ShortUrl = shortUrl,
                CreatedAt = DateTime.UtcNow
            };

            _dbContext.URLs.Add(newUrl);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<ShortUrlResponseDto>(newUrl);
        }
        public async Task<List<ShortUrlResponseDto>> GetAllShortenedUrlsAsync()
        {
            var urls = await _dbContext.URLs.ToListAsync();
            return urls.Select(url => _mapper.Map<ShortUrlResponseDto>(url)).ToList();
        }



        private async Task<string> CreateShortUrlOnUlvisNetAsync(string originalUrl)
        {
            var requestUrl = "https://ulvis.net/API/write/post";

            var content = new MultipartFormDataContent
            {
                { new StringContent(originalUrl), "url" },
                { new StringContent("json"), "type" }
            };

            try
            {
                var response = await _httpClient.PostAsync(requestUrl, content);

                var responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Response: {responseBody}");

                response.EnsureSuccessStatusCode();

                dynamic result = JsonConvert.DeserializeObject(responseBody);

                if (result.success == true)
                {
                    return result.data.url; 
                }
                else
                {
                    throw new HttpRequestException($"Failed to create short URL: {result.error.msg}");
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
                throw;
            }
        }
    }
}
