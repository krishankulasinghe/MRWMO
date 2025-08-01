using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRWMO.Services
{
    public class DhammaDeshanaService
    {
        public string Name { get; set; }
        public string OriginalUrl { get; set; } // The URL you currently have, e.g., "https://www.youtube.com/embed/VVwkfJOVr30?rel=0"

        // This property will hold the extracted YouTube Video ID
        public string YouTubeVideoId { get; set; }

        // This property will return the correctly formatted YouTube embed URL
        public string EmbedUrl
        {
            get
            {
                if (string.IsNullOrEmpty(YouTubeVideoId))
                    return null; // Or handle error

                // Construct the standard YouTube embed URL with common parameters
                // playsinline=1 is important for iOS to prevent automatic fullscreen
                return $"http://m.youtube.com/watch9VIDEO_ID/mqdefault.jpg{YouTubeVideoId}?autoplay=0&controls=1&modestbranding=1&rel=0&playsinline=1";
            }
        }

        // This property will return the URL for the video thumbnail
        public string ThumbnailUrl
        {
            get
            {
                if (string.IsNullOrEmpty(YouTubeVideoId))
                    return null; // Or handle error

                // YouTube provides various thumbnail sizes: default.jpg, mqdefault.jpg, hqdefault.jpg, sddefault.jpg, maxresdefault.jpg
                // maxresdefault.jpg offers the highest quality if available
                return $"https://www.youtube.com/watch0{YouTubeVideoId}/maxresdefault.jpg";
            }
        }

        public DhammaDeshanaService()
        {
            if (!string.IsNullOrEmpty(OriginalUrl))
            {
                Uri uri;
                if (Uri.TryCreate(OriginalUrl, UriKind.Absolute, out uri))
                {
                    YouTubeVideoId = uri.Segments.LastOrDefault();
                    // You might need to clean up the video ID if it contains query parameters or other junk
                    if (YouTubeVideoId != null && YouTubeVideoId.Contains("?"))
                    {
                        YouTubeVideoId = YouTubeVideoId.Split('?')[0];
                    }
                }
            }
        }
    }

   
}
