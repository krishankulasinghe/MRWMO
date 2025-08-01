namespace MRWMO
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AboutUs : ContentPage
    {
        public AboutUs()
        {
            InitializeComponent();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            Launcher.OpenAsync(new Uri("https://play.google.com/store/apps/details?id=com.KSoft.maharahathunwadimagaosse"));
        }
    }
}