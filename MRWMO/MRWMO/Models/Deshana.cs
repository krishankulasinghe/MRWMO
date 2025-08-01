using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace MRWMO.Models
{
    public class Deshana : INotifyPropertyChanged
    {
        public string Url { get; set; }
        public string Location { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }

        private string VideoId => ExtractVideoId(Url);

        // Use the standard YouTube thumbnail URL
        public string ThumbnailUrl => $"https://img.youtube.com/vi/{VideoId}/0.jpg";

        // Create a dedicated embed URL for the WebView
        public string EmbedUrl => $"https://www.youtube.com/embed/{VideoId}?autoplay=1";

        private bool isVideoVisible;
        public bool IsVideoVisible
        {
            get => isVideoVisible;
            set
            {
                if (isVideoVisible != value)
                {
                    isVideoVisible = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand PlayCommand => new Command(() => IsVideoVisible = true);

        private string ExtractVideoId(string url)
        {
            if (string.IsNullOrEmpty(url))
                return string.Empty;

            // This regex is more robust and handles multiple YouTube URL formats
            var regex = new Regex(@"(?:https?:\/\/)?(?:www\.)?(?:(?:youtube\.com\/(?:[^\/\n\s]+\/\S+\/|(?:v|e(?:mbed)?)\/|\S*?[?&]v=))|youtu\.be\/)([a-zA-Z0-9_-]{11})");
            var match = regex.Match(url);
            return match.Success ? match.Groups[1].Value : string.Empty;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
