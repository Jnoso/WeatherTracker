using WeatherTracker.Pages;

namespace WeatherTracker
{
    public partial class App : Application
    {
        [Obsolete]
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new SearchPage());
        }

    }
}