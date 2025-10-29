namespace Counter_BartoszGlinski4j
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(Views.CounterPage), typeof(Views.CounterPage));
        }
    }
}
