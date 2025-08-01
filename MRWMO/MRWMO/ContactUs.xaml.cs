namespace MRWMO
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ContactUs : ContentPage
    {
       // WebView webviewvideo;
        public ContactUs()
        {
            InitializeComponent();

        }

        protected override void OnAppearing()
        {
          //  webviewvideo = new WebView
          //  {
          //      Source = "http://www.youtube.com/embed/_g4DUvumjjU?rel=0&autoplay=1",
          //      HeightRequest = Application.Current.MainPage.Height / 2
          //  };

          //  webviewvideo.BackgroundColor = Color.Transparent;
          //  var videoStack = new StackLayout
          //  {
          //      Children = {
          //    webviewvideo,
          //},
          //  };

          //  Content = videoStack;


        }

    }
}