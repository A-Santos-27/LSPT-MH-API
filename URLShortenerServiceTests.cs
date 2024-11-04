using Microsoft.EntityFrameworkCore;
using System;
using Xunit;
using Moq;
using AutoMapper;
using System.Net.Http;
using System.Threading.Tasks;
using UrlShortenerAPI.Data;
using UrlShortenerAPI.DTOs;
using UrlShortenerAPI.Models;
using UrlShortenerAPI.Services;
using System.Net;
using System.Threading;
using Moq.Protected;

public class URLShortenerServiceTests
{
    private readonly URLShortenerService _urlShortenerService;
    private readonly URLDbContext _dbContext; 
    private readonly Mock<IMapper> _mapperMock = new Mock<IMapper>();
    private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;

    public URLShortenerServiceTests()
    {
        var options = new DbContextOptionsBuilder<URLDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) 
            .Options;

        _dbContext = new URLDbContext(options); 

        _httpMessageHandlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        var httpClient = new HttpClient(_httpMessageHandlerMock.Object);

        _urlShortenerService = new URLShortenerService(
            _dbContext,
            _mapperMock.Object,
            httpClient
        );
    }

    [Fact]
    public async Task ShortenUrlAsync_ReturnsExistingShortUrl_WhenUrlAlreadyExists()
    {
        var existingUrl = new URL { OriginalUrl = "https://example.com", ShortUrl = "https://ulvis.net/abc123" };
        await _dbContext.URLs.AddAsync(existingUrl);
        await _dbContext.SaveChangesAsync();

        var urlDto = new CreateShortUrlDto { OriginalUrl = "https://example.com" };

        _mapperMock.Setup(m => m.Map<ShortUrlResponseDto>(existingUrl))
            .Returns(new ShortUrlResponseDto { ShortUrl = existingUrl.ShortUrl });

        var result = await _urlShortenerService.ShortenUrlAsync(urlDto);

        Assert.Equal("https://ulvis.net/abc123", result.ShortUrl);
    }

    [Fact]
    public async Task ShortenUrlAsync_CreatesNewShortUrl_WhenUrlDoesNotExist()
    {
        var urlDto = new CreateShortUrlDto { OriginalUrl = "https://newsite.com" };
        var newShortUrl = "https://ulvis.net/xyz789";

        _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent($"{{\"success\": true, \"data\": {{\"url\": \"{newShortUrl}\"}}}}")
            });

        var newUrlEntity = new URL
        {
            OriginalUrl = urlDto.OriginalUrl,
            ShortUrl = newShortUrl,
            CreatedAt = DateTime.UtcNow
        };

        _mapperMock
            .Setup(m => m.Map<URL>(It.IsAny<CreateShortUrlDto>()))
            .Returns(newUrlEntity);

        _mapperMock
            .Setup(m => m.Map<ShortUrlResponseDto>(It.IsAny<URL>()))
            .Returns(new ShortUrlResponseDto { ShortUrl = newShortUrl });

        var result = await _urlShortenerService.ShortenUrlAsync(urlDto);

        Assert.NotNull(result);
        Assert.Equal(newShortUrl, result.ShortUrl);
    }

    [Fact]
    public async Task ShortenUrlAsync_ThrowsException_WhenHttpClientFails()
    {
        var urlDto = new CreateShortUrlDto { OriginalUrl = "https://failingsite.com" };

        _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest,
                Content = new StringContent("{\"success\": false, \"error\": {\"msg\": \"Invalid request\"}}")
            });

        await Assert.ThrowsAsync<HttpRequestException>(() => _urlShortenerService.ShortenUrlAsync(urlDto));
    }
}
