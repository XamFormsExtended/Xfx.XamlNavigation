using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using Prism;
using Prism.Common;
using Prism.Ioc;
using Prism.Navigation;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Xfx.XamlNavigation.Prism
{
    public abstract class Navigation : IMarkupExtension, ICommand
    {
        internal static readonly BindableProperty RaiseCanExecuteChangedInternalProperty =
            BindableProperty.CreateAttached("RaiseCanExecuteChangedInternal",
                typeof(Action),
                typeof(Navigation),
                default(Action));

        public static readonly BindableProperty CanNavigateProperty =
            BindableProperty.CreateAttached("CanNavigate",
                typeof(bool),
                typeof(Navigation),
                default(bool), propertyChanged: OnCanNavigatePropertyChanged);

        protected const string NavParameterMessage = "Command Parameter must be of type NavigationParameter";


        protected BindableObject Bindable;
        protected bool IsNavigating;
        protected INavigationService NavigationService;
        protected Page Page;
        private IRootObjectProvider _rootObjectProvider;
        private IProvideValueTarget _valueTargetProvider;

        public bool AllowDoubleTap { get; set; } = false;

        public bool CanExecute(object parameter)
        {
            var canNavigate = GetCanNavigateProperty(Bindable);
            return canNavigate && (AllowDoubleTap || !IsNavigating);
        }

        public event EventHandler CanExecuteChanged;
        public abstract void Execute(object parameter);

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null) throw new ArgumentNullException(nameof(serviceProvider));
            _valueTargetProvider = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;
            _rootObjectProvider = serviceProvider.GetService(typeof(IRootObjectProvider)) as IRootObjectProvider;
            InitNavService();
            return this;
        }

        public static bool GetCanNavigateProperty(BindableObject view)
        {
            return (bool) view.GetValue(CanNavigateProperty);
        }

        public static void SetCanNavigateProperty(BindableObject view, bool value)
        {
            view.SetValue(CanNavigateProperty, value);
        }

        protected NavigationParameters GetNavigationParametersFromCommandParameter(object parameter)
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

        protected void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        internal static Action GetRaiseCanExecuteChangedInternalProperty(BindableObject view)
        {
            return (Action) view.GetValue(RaiseCanExecuteChangedInternalProperty);
        }

        internal static void SetRaiseCanExecuteChangedInternalProperty(BindableObject view, Action value)
        {
            view.SetValue(RaiseCanExecuteChangedInternalProperty, value);
        }

        private void InitNavService()
        {
            if (NavigationService != null) return;
            // if XamlCompilation is active, IRootObjectProvider is not available, but SimpleValueTargetProvider is available
            // if XamlCompilation is inactive, IRootObjectProvider is available, but SimpleValueTargetProvider is not available
            object rootObject;
            //object bindable;
            if (_rootObjectProvider == null && _valueTargetProvider == null)
                throw new ArgumentException("serviceProvider does not provide an IRootObjectProvider or SimpleValueTargetProvider");
            if (_rootObjectProvider == null)
            {
                PropertyInfo propertyInfo = _valueTargetProvider.GetType().GetTypeInfo().DeclaredProperties.FirstOrDefault(dp => dp.Name.Contains("ParentObjects"));
                if (propertyInfo == null) throw new ArgumentNullException("ParentObjects");

                var parentObjects = (propertyInfo.GetValue(_valueTargetProvider) as IEnumerable<object>).ToList();
                var parentObject = parentObjects.FirstOrDefault(pO => pO.GetType().GetTypeInfo().IsSubclassOf(typeof(Page)));

                Bindable = (BindableObject) parentObjects.FirstOrDefault();
                rootObject = parentObject ?? throw new ArgumentNullException("parentObject");
            }
            else
            {
                rootObject = _rootObjectProvider.RootObject;
                Bindable = (BindableObject) _valueTargetProvider.TargetObject;
            }

            SetRaiseCanExecuteChangedInternalProperty(Bindable, RaiseCanExecuteChanged);

            if (rootObject is Page page)
            {
                Page = page;
                var context = (PrismApplicationBase) Application.Current;
                NavigationService = context.Container.Resolve<INavigationService>("PageNavigationService");
                if (NavigationService is IPageAware pageAware) pageAware.Page = page;
            }
        }

        private static void OnCanNavigatePropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            var action = GetRaiseCanExecuteChangedInternalProperty(bindable);
            action?.Invoke();
        }
    }
}