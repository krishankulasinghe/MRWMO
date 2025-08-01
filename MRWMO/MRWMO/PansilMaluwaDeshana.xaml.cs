using MRWMO.Helpers;
using MRWMO.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace MRWMO
{
    public partial class PansilMaluwaDeshana : ContentPage, INotifyPropertyChanged
    {
        private ObservableCollection<PansilMaluwa> _allItems;
        private List<PansilMaluwa> _sourceItems; // To hold all items for searching

        // --- Properties for pagination ---
        private const int PageSize = 15;
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
                    OnPropertyChanged(); // Notify UI to show/hide loading spinner
                }
            }
        }

        public ICommand LoadMoreItemsCommand { get; }

        public PansilMaluwaDeshana()
        {
            InitializeComponent();
            _allItems = new ObservableCollection<PansilMaluwa>();
            videoLinks.ItemsSource = _allItems; // Bind to the new CollectionView
            LoadMoreItemsCommand = new Command(async () => await LoadMoreItems());
            BindingContext = this; // Important for command and IsLoading bindings
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (_sourceItems == null) // Load initial data only once
            {
                _sourceItems = await ApplicationHelper.GetVideoLinks();
                await ResetAndLoadInitial();
            }
        }

        private async Task ResetAndLoadInitial()
        {
            _currentPage = 1;
            _isDataExhausted = false;
            _allItems.Clear();
            await LoadMoreItems();
        }

        private async Task LoadMoreItems()
        {
            if (IsLoading || _isDataExhausted)
                return;

            IsLoading = true;

            // Simulate fetching a "page" from the full list
            var newItems = _sourceItems.Skip((_currentPage - 1) * PageSize).Take(PageSize).ToList();

            // Simulate a network delay
            await Task.Delay(250);

            if (newItems.Any())
            {
                foreach (var item in newItems)
                {
                    _allItems.Add(item);
                }
                _currentPage++;
            }
            else
            {
                _isDataExhausted = true;
            }

            IsLoading = false;
        }

        private async void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = e.NewTextValue?.Trim().ToLower() ?? "";

            if (string.IsNullOrWhiteSpace(searchText))
            {
                // If search is cleared, reset to the paginated list
                await ResetAndLoadInitial();
            }
            else
            {
                // For search, filter the entire source list and display results
                // Note: This disables pagination during search.
                IsLoading = false; // Ensure loading spinner is hidden
                _isDataExhausted = true; // Stop infinite scroll during search
                var filteredItems = _sourceItems
                    .Where(c => c.Name.ToLower().Contains(searchText))
                    .ToList();

                _allItems.Clear();
                foreach (var item in filteredItems)
                {
                    _allItems.Add(item);
                }
            }
        }

        // --- INotifyPropertyChanged implementation ---
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}