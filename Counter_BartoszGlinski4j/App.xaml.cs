namespace Counter_BartoszGlinski4j
{
    public partial class App : Application
    {
        public App()
        {
            System.Diagnostics.Debug.WriteLine(FileSystem.AppDataDirectory);
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}