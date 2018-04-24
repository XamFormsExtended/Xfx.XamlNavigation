using Prism.Ioc;
using Prism.Segue.Application.ViewModels;
using Prism.Segue.Application.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MainPage = Prism.Segue.Application.Views.MainPage;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Prism.Segue.Application
{
    public partial class App
    {
        /* 
         * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
         * This imposes a limitation in which the App class must have a default constructor. 
         * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
         */
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer)
        {
           //  MainPage = new NavigationPage(new MainPage());
        }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            await NavigationService.NavigateAsync("NavigationPage/MainPage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage,MainPageViewModel>();
            containerRegistry.RegisterForNavigation<About,AboutPageModel>();
            containerRegistry.RegisterForNavigation<Secret,SecretPageModel>();
        }
    }
}
