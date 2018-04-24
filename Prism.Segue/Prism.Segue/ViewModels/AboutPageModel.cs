using Prism.Navigation;

namespace Prism.Segue.Application.ViewModels
{
    internal class AboutPageModel : ViewModelBase
    {
        private NavigationParameters _secretPageParameters;

        public AboutPageModel(INavigationService navigationService) : base(navigationService)
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
    }
}