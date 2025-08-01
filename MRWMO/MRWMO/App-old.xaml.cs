namespace MRWMO
{
    public partial class App1 : Application
    {
        public App1()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new MainPage()) { Title = "MRWMO" };
        }
    }
}
