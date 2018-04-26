using Prism.Mvvm;
using Prism.Navigation;

namespace example.ViewModels
{
    public class ViewModelBase : BindableBase, INavigationAware, IDestructible
    {
        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public virtual void OnNavigatedFrom(NavigationParameters parameters) { }

        public virtual void OnNavigatedTo(NavigationParameters parameters){ }

        public virtual void OnNavigatingTo(NavigationParameters parameters){ }

        public virtual void Destroy(){ }
    }
}
