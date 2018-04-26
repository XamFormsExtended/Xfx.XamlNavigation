using Prism.Navigation;

namespace Prism.Segue.Application.ViewModels
{
    public class SecretPageModel : ViewModelBase
    {
        private bool _isHierarchical;
        private bool _isModal;
        private string _senderTitle;

        public SecretPageModel(INavigationService navigationService) : base(navigationService)
        {
        }

        public bool IsHierarchical
        {
            get => _isHierarchical;
            set => SetProperty(ref _isHierarchical, value);
        }

        public bool IsModal
        {
            get => _isModal;
            set => SetProperty(ref _isModal, value);
        }

        public string SenderTitle
        {
            get => _senderTitle;
            set => SetProperty(ref _senderTitle, value);
        }

        public override void OnNavigatingTo(NavigationParameters parameters)
        {
            if (parameters.TryGetValue("_prism", out NavigationParameters prism)
                && prism.TryGetValue("UseModalNavigation", out bool isModal))
            {
                IsModal = isModal;
                IsHierarchical = !isModal;
            }
            else
            {
                IsModal = false;
                IsHierarchical = true;
            }

            var from = "unknown";
            if (parameters.TryGetValue(nameof(AboutPageModel), out AboutPageModel sender)) from = sender.Title;

            SenderTitle = $"Sent from {from}";
        }
    }
}