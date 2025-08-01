using System.ComponentModel;

namespace MRWMO
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : Shell
    {
        public MainPage()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(Chapters), typeof(Chapters));
            Routing.RegisterRoute(nameof(Content), typeof(Content));
        }

      
    }
}
