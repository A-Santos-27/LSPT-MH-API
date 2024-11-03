using System;

namespace UrlShortenerAPI.Models
{
    public class URL
    {
        public string OriginalUrl { get; set; } 
        public string ShortUrl { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
