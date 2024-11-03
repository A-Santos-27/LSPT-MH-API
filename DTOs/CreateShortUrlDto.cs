namespace UrlShortenerAPI.DTOs
{
    public class CreateShortUrlDto
    {
        public string OriginalUrl { get; set; }
    }

    public class ShortUrlResponseDto
    {
        public string ShortUrl { get; set; }
    }
}
