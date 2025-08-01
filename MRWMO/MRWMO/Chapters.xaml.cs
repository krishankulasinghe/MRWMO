using MRWMO.Enums;
using MRWMO.Helpers;
using MRWMO.Models;
using System;

namespace MRWMO
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Chapters : ContentPage
    {
        private Book _book;
        public Chapters(Book book)
        {
            if (book == null)
            {
                throw new ArgumentException();
            }
            _book = book;
            BindingContext = book;

            _book.Chapters = GetAllChapters();
            var lastReadId = Preferences.Get($"last_read_book_{_book.Id}", 0);
            int total = _book.Chapters.Count;
            int index = _book.Chapters.ToList().FindIndex(c => c.Id == lastReadId);
            _book.Progress = (int)(total > 0 && index >= 0 ? (index + 1) / (double)total : 0);
            InitializeComponent();
            if (_book.LanguageId == (int)LanguageEnum.Sinhala)
            {
                chapterTitleLabel.Text = "පටුන";
            }
            else
            {
                chapterTitleLabel.Text = "Chapters";
            }
            NavigationPage.SetHasNavigationBar(this, false);
        }


        private IList<Chapter> GetAllChapters(string searchText = null)
        {
            IList<Chapter> chapters = null;

            if (_book.LanguageId == (int)LanguageEnum.Sinhala)
            {
                chapters = ApplicationHelper.GetChaptersBySinhalaBookId(_book.Id);
            }
            else
            {
                chapters = ApplicationHelper.GetChaptersByEnglishBookId(_book.Id);
            }


            if (string.IsNullOrWhiteSpace(searchText))
            {
                return chapters;
            }

            return chapters.Where(c => c.Name.ToLower().Contains(searchText.Trim().ToLower())).ToList();
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            _book.Chapters = GetAllChapters(e.NewTextValue);
        }

        //private void chapterList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        //{
        //    var chapter = e.SelectedItem as Chapter;
        //    if (chapter != null)
        //    {
        //        // Save last read chapter
        //        Preferences.Set($"last_read_book_{_book.Id}", chapter.Id);
        //        Navigation.PushAsync(new Content(chapter));
        //    }
        //}

        private void ChapterSelected(object sender, SelectionChangedEventArgs e)
        {
            var chapter = e.CurrentSelection.FirstOrDefault() as Chapter;

            if (sender is CollectionView collectionView)
                collectionView.SelectedItem = null;

            if (chapter != null)
            {
                // Save last read chapter
                Preferences.Set($"last_read_book_{_book.Id}", chapter.Id);

                // Navigate to chapter content page
                Navigation.PushAsync(new Content(chapter));
            }
        }

        private void ContinueReading_Clicked(object sender, EventArgs e)
        {
            var lastId = Preferences.Get($"last_read_book_{_book.Id}", 0);
            var lastChapter = _book.Chapters.FirstOrDefault(c => c.Id == lastId)
                              ?? _book.Chapters.FirstOrDefault(); // fallback

            if (lastChapter != null)
            {
                Navigation.PushAsync(new Content(lastChapter));
            }
        }

        protected override bool OnBackButtonPressed()
        {
            base.OnBackButtonPressed();
            return false;
        }
    }
}