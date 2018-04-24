using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using Prism.Navigation;
using Prism.Segue.Application.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Internals;

namespace Prism.Segue.Application.Segue
{
    public enum UseModalNavigation
    {
        Auto,Modal,Hierarchical,GoBack,BackToRoot
    }

    [ContentProperty(nameof(UseModalNavigation))]
    public class UriExtension : IMarkupExtension, ICommand
    {
        private bool _navigating;
        private INavigationService _navService;
        private IRootObjectProvider _rootObjectProvider;
        private SimpleValueTargetProvider _valueTargetProvider;
        public bool AllowDoubleTap { get; set; } = false;
        public bool Animated { get; set; } = true;
        public UseModalNavigation UseModalNavigation { get; set; } = UseModalNavigation.Auto;

        public bool CanExecute(object parameter)
        {
            return AllowDoubleTap || !_navigating;
        }

        public event EventHandler CanExecuteChanged;

        public async void Execute(object parameter)
        {
            _navigating = true;
            RaiseCanExecuteChanged();
            InitNavService();


            if (_navService != null)
            {
                bool? useModalNavigation = null;
                switch (UseModalNavigation)
                {
                    case UseModalNavigation.Modal:
                        useModalNavigation = true;
                        break;
                    case UseModalNavigation.Hierarchical:
                        useModalNavigation = false;
                        break;
                    case UseModalNavigation.BackToRoot:
                        await _navService.GoBackToRootAsync();
                        return;
                    case UseModalNavigation.GoBack:
                        await _navService.GoBackAsync();
                        return;
                }

                switch (parameter)
                {
                    case Uri uri:
                        await _navService.NavigateAsync(uri, useModalNavigation: useModalNavigation, animated: Animated);
                        break;
                    default:
                        await _navService.NavigateAsync(parameter?.ToString(), useModalNavigation: useModalNavigation, animated: Animated);
                        break;
                }
            }
            _navigating = false;
            RaiseCanExecuteChanged();
        }

        private void InitNavService()
        {
            // if XamlCompilation is active, IRootObjectProvider is not available, but SimpleValueTargetProvider is available
            // if XamlCompilation is inactive, IRootObjectProvider is available, but SimpleValueTargetProvider is not available
            object rootObject;
            if (_rootObjectProvider == null && _valueTargetProvider == null) throw new ArgumentException("serviceProvider does not provide an IRootObjectProvider or SimpleValueTargetProvider");
            if (_rootObjectProvider == null)
            {
                PropertyInfo propertyInfo = _valueTargetProvider.GetType().GetTypeInfo().DeclaredProperties.FirstOrDefault(dp => dp.Name.Contains("ParentObjects"));
                if (propertyInfo == null) throw new ArgumentNullException("ParentObjects");

                var parentObjects = propertyInfo.GetValue(_valueTargetProvider) as IEnumerable<object>;
                var parentObject = parentObjects.FirstOrDefault(pO => pO.GetType().GetTypeInfo().IsSubclassOf(typeof(Page)));

                rootObject =  parentObject ?? throw new ArgumentNullException("parentObject");
            }
            else
            {
                rootObject = _rootObjectProvider.RootObject ;
            }

            if (_navService == null && rootObject is Page page && page.BindingContext is ViewModelBase vm)
            {
                _navService = vm.NavigationService;
            }
        }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null) throw new ArgumentNullException("serviceProvider");

             _rootObjectProvider = serviceProvider.GetService(typeof(IRootObjectProvider)) as IRootObjectProvider;
             _valueTargetProvider = serviceProvider.GetService(typeof(IProvideValueTarget)) as SimpleValueTargetProvider;

            return this;
        }

        private void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}