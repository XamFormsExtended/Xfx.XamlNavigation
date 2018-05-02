using System.Diagnostics;
using Prism.Navigation;

namespace example.ViewModels
{
    public class AboutPageModel : ViewModelBase
    {
        private NavigationParameters _secretPageParameters;

        public AboutPageModel()
        {
            SecretPageParameters = new NavigationParameters()
            {
                {nameof(AboutPageModel),this}
            };
            Title = "About Page";
        }

        public NavigationParameters SecretPageParameters
        {
            get => _secretPageParameters;
            set => SetProperty(ref _secretPageParameters, value);
        }

        public override void OnNavigatingTo(NavigationParameters parameters)
        {
            if (parameters.TryGetValue(nameof(MainPageViewModel), out MainPageViewModel sender))
            {
                Debug.Write("We passed the parameter from xaml");
                Debug.Write(sender?.Title);
            }
        }
    }
}