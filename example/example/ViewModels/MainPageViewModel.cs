using System.Threading.Tasks;

namespace example.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private bool _canNavigate;

        public MainPageViewModel()
        {
            Title = "Main Page";
            CanNavigate = false;
            Task.Run(async () => await Task.Delay(2000)).ContinueWith(task => CanNavigate = true);
        }

        public bool CanNavigate
        {
            get => _canNavigate;
            set => SetProperty(ref _canNavigate, value);
        }
    }
}