using MvvmCross.Navigation;
using Staketracker.Core.ViewModels.Login;

namespace Staketracker.Core.ViewModels.Root
{
    public class RootViewModel : BaseViewModel
    {
        readonly IMvxNavigationService _navigationService;

        public RootViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;
        }


        public override async void ViewAppearing()
        {
            base.ViewAppearing();

            //await _navigationService.Navigate<MenuViewModel>();
            await _navigationService.Navigate<LoginViewModel>();
        }
    }
}
