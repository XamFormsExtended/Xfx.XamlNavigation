using System;
using Prism.Navigation;
using Xamarin.Forms;

namespace Prism.Segue.Application.Prism
{
    [ContentProperty(nameof(GoBackType))]
    public class GoBack : NavigationMarkupExtension
    {
        private bool _navigating;

        public bool AllowDoubleTap { get; set; } = false;
        public bool Animated { get; set; } = true;
        public GoBackType GoBackType { get; set; } = GoBackType.Default;
        public bool? UseModalNavigation { get; set; } = null;

        public override bool CanExecute(object parameter)
        {
            return AllowDoubleTap || !_navigating;
        }

        public override async void Execute(object parameter)
        {
            parameter = parameter ?? new NavigationParameters();
            if (!(parameter is NavigationParameters parameters)) throw new ArgumentException(NavParameterMessage, nameof(parameter));

            _navigating = true;
            RaiseCanExecuteChanged();
            InitNavService();
            parameters.Add("_prism",new NavigationParameters
            {
                {nameof(Animated),Animated},
                {nameof(GoBackType),GoBackType},
                {nameof(UseModalNavigation),UseModalNavigation},
            });

            if (GoBackType == GoBackType.ToRoot)
                await NavigationService.GoBackToRootAsync(parameters);
            else
                await NavigationService.GoBackAsync(parameters, UseModalNavigation, Animated);
            _navigating = false;
            RaiseCanExecuteChanged();
        }
    }
}