using MRWMO.Helpers;
using MRWMO.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace MRWMO
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DhammaDeshana : ContentPage, INotifyPropertyChanged
    {
        private ObservableCollection<Deshana> _allDeshana;

        // --- New properties for pagination ---
        private const int PageSize = 5; // How many items to load at a time
        private int _currentPage = 1;
        private bool _isDataExhausted = false;

        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                if (_isLoading != value)
                {
                    _isLoading = value;
                    OnPropertyChanged(); // Notify the UI to show/hide the loading indicator
                }
            }
        }

        public ICommand LoadMoreItemsCommand { get; }

        public DhammaDeshana()
        {
            InitializeComponent();
            _allDeshana = new ObservableCollection<Deshana>();
            videoLinks.ItemsSource = _allDeshana;
            LoadMoreItemsCommand = new Command(async () => await LoadMoreItems());
            BindingContext = this; // Set binding context for the IsLoading property
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // --- Reset and load initial data ---
            if (_allDeshana.Count == 0)
            {
                _currentPage = 1;
                _isDataExhausted = false;
                _allDeshana.Clear();
                await LoadMoreItems();
            }
        }

        private async Task LoadMoreItems()
        {
            // Prevent multiple simultaneous loads or loading after all data is fetched
            if (IsLoading || _isDataExhausted)
                return;

            IsLoading = true;

            // Fetch the next page of data
            var newItems = await ApplicationHelper.GetDhammaDeshana(_currentPage, PageSize);

            if (newItems.Any())
            {
                foreach (var item in newItems)
                {
                    _allDeshana.Add(item);
                }
                _currentPage++;
            }
            else
            {
                // No more items to load
                _isDataExhausted = true;
            }

            IsLoading = false;
        }

        // You still need your search and tap gesture methods
        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Note: Searching will now only apply to the items that are already loaded.
            // A full server-side search would require a different approach.
            var query = e.NewTextValue?.Trim().ToLower() ?? "";
            if (string.IsNullOrEmpty(query))
            {
                videoLinks.ItemsSource = _allDeshana;
            }
            else
            {
                var filtered = _allDeshana
                    .Where(c => c.Date.ToLower().Contains(query) || c.Location.ToLower().Contains(query) || c.Time.ToLower().Contains(query))
                    .ToList();
                videoLinks.ItemsSource = filtered;
            }
        }

        private void Thumbnail_Tapped(object sender, EventArgs e)
        {
            if (sender is Image image && image.BindingContext is Deshana deshana)
            {
                deshana.IsVideoVisible = true;
            }
        }

        // --- INotifyPropertyChanged implementation ---
        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}