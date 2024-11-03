using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UrlShortenerAPI.DTOs;
using UrlShortenerAPI.Services;

namespace UrlShortenerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class URLController : ControllerBase
    {
        private readonly IURLShortenerService _urlShortenerService;

        public URLController(IURLShortenerService urlShortenerService)
        {
            _urlShortenerService = urlShortenerService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllShortenedUrls()
        {
            var allUrls = await _urlShortenerService.GetAllShortenedUrlsAsync();
            return Ok(allUrls);
        }


        [HttpPost("shorten")]
        public async Task<IActionResult> CreateShortUrl([FromBody] CreateShortUrlDto urlDto)
        {
            var shortUrl = await _urlShortenerService.ShortenUrlAsync(urlDto);
            return Ok(shortUrl);
        }

    }
}
