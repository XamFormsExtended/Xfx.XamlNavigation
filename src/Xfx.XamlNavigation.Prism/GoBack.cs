using Prism.Navigation;
using Xamarin.Forms;

namespace Xfx.XamlNavigation.Prism
{
    [ContentProperty(nameof(GoBackType))]
    public class GoBack : Navigation
    {
        public bool Animated { get; set; } = true;
        public GoBackType GoBackType { get; set; } = GoBackType.Default;
        public bool? UseModalNavigation { get; set; } = null;

        public override async void Execute(object parameter)
        {
            var parameters = parameter.ToNavigationParameters();

            IsNavigating = true;
            RaiseCanExecuteChanged();
            parameters.Add("_prism", new NavigationParameters
            {
                {nameof(Animated), Animated},
                {nameof(GoBackType), GoBackType},
                {nameof(UseModalNavigation), UseModalNavigation}
            });

            if (GoBackType == GoBackType.ToRoot)
                await NavigationService.GoBackToRootAsync(parameters);
            else
                await NavigationService.GoBackAsync(parameters, UseModalNavigation, Animated);
            IsNavigating = false;
            RaiseCanExecuteChanged();
        }
    }
}