using MRWMO.Models;
using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace MRWMO
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BookMarks : ContentPage
    {
        private string _bookMarks;
        private ObservableCollection<Chapter> _bookMarkList;
        public BookMarks()
        {
            
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            if (Preferences.ContainsKey("Bookmarks"))
                _bookMarks = Preferences.Get("Bookmarks", string.Empty);

            if (!string.IsNullOrWhiteSpace(_bookMarks))
            {
                var bookmarks = JsonConvert.DeserializeObject<ObservableCollection<Chapter>>(_bookMarks);
                _bookMarkList = bookmarks;
            }
          
            bookmarkList.ItemsSource = _bookMarkList;

        }

        private void bookmarkList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var chapter = e.SelectedItem as Chapter;
            Navigation.PushAsync(new Content(chapter));
        }

        private void MenuItem_Clicked(object sender, EventArgs e)
        {
            var menuItem = sender as MenuItem;
            
            var chapter = menuItem.CommandParameter as Chapter;

           _bookMarkList.Remove(chapter);

            if (Preferences.ContainsKey("Bookmarks"))
            {
                Preferences.Remove("Bookmarks");
            }
            var bookmarksJson = JsonConvert.SerializeObject(_bookMarkList);
            Preferences.Set("Bookmarks", bookmarksJson);
        }
    }
}