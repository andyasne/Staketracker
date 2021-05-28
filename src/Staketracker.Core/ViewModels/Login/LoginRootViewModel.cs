using System.Threading.Tasks;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Staketracker.Core.Models;
using Staketracker.Core.ViewModels.CommunicationList;
using Staketracker.Core.ViewModels.Events;
using Staketracker.Core.ViewModels.Login;
using Staketracker.Core.ViewModels.Menu;
using Staketracker.Core.ViewModels.TwoStepVerification;

namespace Staketracker.Core.ViewModels.Root
{
    public class LoginRootViewModel : BaseViewModel
    {
        private readonly IMvxNavigationService _navigationService;
        private AuthReply authReply;
        public LoginRootViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public override async void ViewAppearing()
        {
            base.ViewAppearing();

            MvxNotifyTask.Create(async () => await this.InitializeViewModels());

        }


        private async Task InitializeViewModels()
        {


            await _navigationService.Navigate<LoginViewModel>();

        }
    }
}
