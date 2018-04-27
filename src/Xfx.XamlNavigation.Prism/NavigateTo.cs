using Prism.Navigation;
using Xamarin.Forms;

namespace Xfx.XamlNavigation.Prism
{
    [ContentProperty(nameof(Name))]
    public class NavigateTo : Navigation
    {
        public bool Animated { get; set; } = true;
        public string Name { get; set; }
        public bool? UseModalNavigation { get; set; } = null;

        public override async void Execute(object parameter)
        {
            var parameters = GetNavigationParametersFromCommandParameter(parameter);

            IsNavigating = true;
            RaiseCanExecuteChanged();
            parameters.Add("_prism", new NavigationParameters
            {
                {nameof(Animated), Animated},
                {nameof(Name), Name},
                {nameof(UseModalNavigation), UseModalNavigation}
            });

            await NavigationService.NavigateAsync(Name, parameters, UseModalNavigation, Animated);
            IsNavigating = false;
            RaiseCanExecuteChanged();
        }
    }
}