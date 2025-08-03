using MRWMO.Enums;
using MRWMO.Helpers;
using MRWMO.Models;
using Newtonsoft.Json;
using Microsoft.Maui.Storage;
#if ANDROID
using Android.Views;
using Microsoft.Maui.Platform;
using AndroidX.Core.View;
#endif

namespace MRWMO
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Content : ContentPage
    {
        private Chapter _chapter;
        private IList<Chapter> _bookMarkList = null;
        private CancellationTokenSource cts;
        private int _fontSize = 18;
        // A flag to keep track of the current screen state.
        private bool _isFullScreen = false;

        public Content(Chapter chapter)
        {
            _chapter = chapter;
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        /// This event is triggered when the main content area is tapped.
        /// </summary>
        private void OnContentTapped(object sender, TappedEventArgs e)
        {
            ToggleFullScreen();
        }

        /// <summary>
        /// Toggles the visibility of the navigation and status bars.
        /// </summary>
        private void ToggleFullScreen()
        {
            // Invert the fullscreen state
            _isFullScreen = !_isFullScreen;

            // Toggle the visibility of the MAUI Navigation Bar
            Shell.SetNavBarIsVisible(this, !_isFullScreen);

            // Platform-specific code to hide/show the system status bar
#if ANDROID
            var activity = Platform.CurrentActivity;
            if (activity?.Window != null)
            {
                var window = activity.Window;
                var insetsController = WindowCompat.GetInsetsController(window, window.DecorView);

                if (_isFullScreen)
                {
                    // Hide the status bar
                    insetsController.Hide(WindowInsetsCompat.Type.SystemBars());
                    insetsController.SystemBarsBehavior = WindowInsetsControllerCompat.BehaviorShowTransientBarsBySwipe;
                }
                else
                {
                    // Show the status bar
                    insetsController.Show(WindowInsetsCompat.Type.SystemBars());
                }
            }
         #endif
        }

        protected override void OnAppearing()
        {
            string content = (_chapter.Book.LanguageId == (int)LanguageEnum.Sinhala)
                ? ApplicationHelper.GetSinhalaContent(_chapter.BookId, _chapter.Id)
                : ApplicationHelper.GetEnglishContent(_chapter.BookId, _chapter.Id);

            _chapter.Content = content.Replace("z", Environment.NewLine);

            LoadBookmarkStatus();
            contents.FontSize = _fontSize;
            if (_chapter.Book.LanguageId == (int)LanguageEnum.Sinhala)
            {
                contents.FontFamily = "AbhayaLibreM";
            }
            else {
                contents.FontFamily = "OpenSansRegular";
            } 
            BindingContext = _chapter;
        }

        private void LoadBookmarkStatus()
        {
            if (Preferences.ContainsKey("Bookmarks"))
            {
                var bookMarks = Preferences.Get("Bookmarks", string.Empty);
                _bookMarkList = JsonConvert.DeserializeObject<List<Chapter>>(bookMarks);
            }

            bool isBookmarked = _bookMarkList?.Any(c => c.BookId == _chapter.BookId && c.Id == _chapter.Id) == true;
            BookMarkToolBarItem.Source = isBookmarked ? "ic_action_bookmark.png" : "ic_action_bookmark_border.png";
        }

        private void ToggleSettings_Clicked(object sender, EventArgs e)
        {
            SettingsPanel.IsVisible = !SettingsPanel.IsVisible;
        }

        private void ToolbarItem_Clicked_Plus(object sender, EventArgs e)
        {
            if (_fontSize < 30)
            {
                _fontSize += 2;
                contents.FontSize = _fontSize;
            }
        }

        private void ToolbarItem_Clicked_Minus(object sender, EventArgs e)
        {
            if (_fontSize > 12)
            {
                _fontSize -= 2;
                contents.FontSize = _fontSize;
            }
        }

        private void ToolbarItem_Clicked_BookMark(object sender, EventArgs e)
        {
            _bookMarkList ??= new List<Chapter>();
            var existing = _bookMarkList.FirstOrDefault(c => c.BookId == _chapter.BookId && c.Id == _chapter.Id);

            if (existing != null)
            {
                _bookMarkList.Remove(existing);
                BookMarkToolBarItem.Source = "ic_action_bookmark_border.png";
            }
            else
            {
                _bookMarkList.Add(_chapter);
                BookMarkToolBarItem.Source = "ic_action_bookmark.png";
            }

            var bookmarksJson = JsonConvert.SerializeObject(_bookMarkList);
            Preferences.Set("Bookmarks", bookmarksJson);
        }

        private void ToolbarItem_Clicked_Next(object sender, EventArgs e)
        {
            LoadChapterByOffset(+1);
        }

        private void ToolbarItem_Clicked_Back(object sender, EventArgs e)
        {
            LoadChapterByOffset(-1);
        }

        private void LoadChapterByOffset(int offset)
        {
            CancelSpeech();

            var chapters = (_chapter.Book.LanguageId == (int)LanguageEnum.Sinhala)
                ? ApplicationHelper.GetChaptersBySinhalaBookId(_chapter.BookId)
                : ApplicationHelper.GetChaptersByEnglishBookId(_chapter.BookId);

            var nextId = _chapter.Id + offset;
            var nextChapter = chapters.FirstOrDefault(c => c.Id == nextId);

            if (nextChapter == null) return;

            string content = (_chapter.Book.LanguageId == (int)LanguageEnum.Sinhala)
                ? ApplicationHelper.GetSinhalaContent(_chapter.BookId, nextId)
                : ApplicationHelper.GetEnglishContent(_chapter.BookId, nextId);

            nextChapter.Content = content.Replace("z", Environment.NewLine);
            _chapter = nextChapter;

            LoadBookmarkStatus();
            BindingContext = _chapter;
            contents.FontSize = _fontSize;
            if (_chapter.Book.LanguageId == (int)LanguageEnum.Sinhala)
            {
                contents.FontFamily = "AbhayaLibreM";
            }
            else
            {
                contents.FontFamily = "OpenSansRegular";
            }
        }

        private async void PlayToolBarItem_Clicked(object sender, EventArgs e)
        {
            var settings = new SpeechOptions { Volume = 1.0f, Pitch = 1.0f };

            if (_chapter.Book.LanguageId == (int)LanguageEnum.Sinhala)
            {
                var locales = await TextToSpeech.GetLocalesAsync();
                settings.Locale = locales.FirstOrDefault(c => c.Country == "LK");
            }

            if (cts?.IsCancellationRequested == false)
            {
                CancelSpeech();
                return;
            }

            cts = new CancellationTokenSource();
            PlayToolBarItem.Source = "ic_action_stop.png";
            await TextToSpeech.SpeakAsync(_chapter.Content, settings, cancelToken: cts.Token);
        }

        private void CancelSpeech()
        {
            if (cts?.IsCancellationRequested ?? true) return;

            cts.Cancel();
            PlayToolBarItem.Source = "ic_action_play_arrow.png";
        }

        protected override void OnDisappearing() => CancelSpeech();

        private async void ShareToolBarItem_Clicked(object sender, EventArgs e)
        {
            await Share.RequestAsync(new ShareTextRequest
            {
                Text = $"{_chapter.Name}\n\n{_chapter.Content}",
                Title = _chapter.Name
            });
        }
    }

}