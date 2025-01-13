using Microsoft.Maui.Controls;

namespace MauiApp2
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new Login()); 
        }

        public void NavigateToMainPage()
        {
            MainPage = new AppShell();
        }
    }
}
