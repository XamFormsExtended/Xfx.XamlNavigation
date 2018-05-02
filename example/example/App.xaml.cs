using System;
using System.Threading.Tasks;
using example.ViewModels;
using example.Views;
using Prism;
using Prism.Ioc;
using Prism.Logging;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using example.Logging;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace example
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
        }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            try
            {
                TaskScheduler.UnobservedTaskException += (sender, e) => {
                    Logger.Log(e.Exception.ToString(), Category.Exception, Priority.High);
                };
                await NavigationService.NavigateAsync("MainPage/NavigationPage/About");
            }
            catch(Exception e)
            {
                Logger.Log(e, Category.Exception, Priority.High);
            }
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
