using MRWMO.Helpers;
using MRWMO.Models;

namespace MRWMO
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Sinhala : ContentPage
    {
        public Sinhala()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        protected override void OnAppearing()
        {
            var books = ApplicationHelper.GetAllSinhalaBooks();

            BindingContext = books;

            base.OnAppearing();
        }

        private void listView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var book = e.SelectedItem as Book;
            Navigation.PushAsync(new Chapters(book));
        }

        private void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var collectionView = sender as CollectionView;
            var book = e.CurrentSelection.FirstOrDefault() as Book;
            if (book != null)
            {
                collectionView.SelectedItem = null; // Clear selection immediately
                Navigation.PushAsync(new Chapters(book));
            }
        }
    }
}