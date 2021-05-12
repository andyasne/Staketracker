using MvvmCross.Navigation;
using Staketracker.Core.ViewModels.EventsList;
using Staketracker.Core.ViewModels.Login;
using Staketracker.Core.ViewModels.Menu;
using Staketracker.Core.ViewModels.TwoStepVerification;

namespace Staketracker.Core.ViewModels.Root
{
    public class RootViewModel : BaseViewModel
    {
        private readonly IMvxNavigationService _navigationService;

        public RootViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public override async void ViewAppearing()
        {
            base.ViewAppearing();

            await _navigationService.Navigate<MenuViewModel>();
            await _navigationService.Navigate<TwoStepVerificationViewModel>();
        }
    }
}
