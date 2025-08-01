using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace MRWMO.Models;

public class PansilMaluwa : INotifyPropertyChanged
{
    public string Name { get; set; }
    public string Url { get; set; }

    private string VideoId => ExtractVideoId(Url);

    // NEW: Thumbnail URL property
    public string ThumbnailUrl => $"https://img.youtube.com/vi/{VideoId}/0.jpg";

    public string EmbedUrl => $"https://www.youtube.com/embed/{VideoId}?autoplay=1";

    // NEW: Property to control visibility
    private bool isVideoVisible;
    public bool IsVideoVisible
    {
        get => isVideoVisible;
        set
        {
            if (isVideoVisible != value)
            {
                isVideoVisible = value;
                OnPropertyChanged(); // Notify the UI of the change
            }
        }
    }

    // NEW: Command to trigger showing the video
    public ICommand PlayCommand => new Command(() => IsVideoVisible = true);

    private string ExtractVideoId(string url)
    {
        if (string.IsNullOrEmpty(url))
            return string.Empty;

        var regex = new Regex(@"(?:https?:\/\/)?(?:www\.)?(?:(?:youtube\.com\/(?:[^\/\n\s]+\/\S+\/|(?:v|e(?:mbed)?)\/|\S*?[?&]v=))|youtu\.be\/)([a-zA-Z0-9_-]{11})");
        var match = regex.Match(url);
        return match.Success ? match.Groups[1].Value : string.Empty;
    }

    // NEW: INotifyPropertyChanged implementation
    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string name = "") =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}