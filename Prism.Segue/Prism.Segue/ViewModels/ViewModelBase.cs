using Prism.Mvvm;
using Prism.Navigation;
using Xfx.XamlNavigation.Prism;

namespace Prism.Segue.Application.ViewModels
{
    public class ViewModelBase : BindableBase, INavigationAware, IDestructible, INavigatable
    {
        public INavigationService NavigationService { get; }

        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public ViewModelBase(INavigationService navigationService)
        {
            NavigationService = navigationService;
        }

        public virtual void OnNavigatedFrom(NavigationParameters parameters) { }

        public virtual void OnNavigatedTo(NavigationParameters parameters){ }

        public virtual void OnNavigatingTo(NavigationParameters parameters){ }

        public virtual void Destroy(){ }
    }
}
