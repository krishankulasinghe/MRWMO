using MRWMO.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace MRWMO;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class QuotesPage : ContentPage
{

    public ObservableCollection<Quote> Quotes { get; set; }
    public ICommand CopyCommand { get; }
    public ICommand ShareCommand { get; }
    public QuotesPage()
	{
        InitializeComponent();

        // Initialize Commands
        CopyCommand = new Command<Quote>(async (quote) => await OnCopy(quote));
        ShareCommand = new Command<Quote>(async (quote) => await OnShare(quote));

        // Load the quote data
        LoadQuotes();

        // Set the binding context for the page to itself
        this.BindingContext = this;
        QuotesCollectionView.ItemsSource = Quotes;
    }

    private void LoadQuotes()
    {
        // Here you would typically load quotes from a database or API
        Quotes = new ObservableCollection<Quote>
        {
            new Quote { Text = "The only way to do great work is to love what you do.", Author = "Steve Jobs" },
            new Quote { Text = "The purpose of our lives is to be happy.", Author = "Dalai Lama" },
            new Quote { Text = "Get busy living or get busy dying.", Author = "Stephen King" },
            new Quote { Text = "You only live once, but if you do it right, once is enough.", Author = "Mae West" },
            new Quote { Text = "The future belongs to those who believe in the beauty of their dreams.", Author = "Eleanor Roosevelt" }
        };
    }

    private async Task OnCopy(Quote quoteToCopy)
    {
        if (quoteToCopy == null) return;

        await Clipboard.SetTextAsync(quoteToCopy.FormattedQuote);
        await DisplayAlert("Copied", "The quote has been copied to your clipboard.", "OK");
    }

    private async Task OnShare(Quote quoteToShare)
    {
        if (quoteToShare == null) return;

        await Share.RequestAsync(new ShareTextRequest
        {
            Text = quoteToShare.FormattedQuote,
            Title = "Share Quote"
        });
    }
}