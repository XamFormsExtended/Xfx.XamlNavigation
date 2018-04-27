using System;
using Prism.Navigation;

namespace Xfx.XamlNavigation.Prism
{
    public static class Extensions
    {
        private const string NavParameterMessage = "Command Parameter must be of type NavigationParameters, XamlNavigationParameter, or XamlNavigationParameters";

        public static NavigationParameters ToNavigationParameters(this object parameter)
        {
            parameter = parameter ?? new NavigationParameters();
            switch (parameter)
            {
                case NavigationParameters parameters:
                    return parameters;
                case XamlNavigationParameter xamlParameter:
                    return new NavigationParameters {{xamlParameter.Key, xamlParameter.Value}};
                case XamlNavigationParameters xamlParameters:
                    return xamlParameters.ToNavigationParameters();
            }
            
            throw new ArgumentException(NavParameterMessage, nameof(parameter));
        }
    }
}