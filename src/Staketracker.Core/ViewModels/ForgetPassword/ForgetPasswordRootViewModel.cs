using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Staketracker.Core.Models;
using Staketracker.Core.ViewModels.Login;
using System.Threading.Tasks;
using Staketracker.Core.ViewModels.Menu;

namespace Staketracker.Core.ViewModels.Root
{
    public class ForgetPasswordRootViewModel : BaseViewModel
    {
        private readonly IMvxNavigationService _navigationService;
        private AuthReply authReply;
        public ForgetPasswordRootViewModel(IMvxNavigationService navigationService)
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



            await _navigationService.Navigate<ForgetPassword.ForgetPasswordViewModel>();

        }
    }
}
