using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Staketracker.Core.Models;
using Staketracker.Core.ViewModels.Home;
using Staketracker.Core.ViewModels.Menu;
using Staketracker.Core.ViewModels.Root;

namespace Staketracker.Core.ViewModels
{

    public class Rvm : BaseViewModel<AuthReply>
    {
        private readonly IMvxNavigationService _navigationService;
        public Rvm(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;
        }


        public override void ViewAppearing()
        {
            base.ViewAppearing();

            MvxNotifyTask.Create(async () => await this.InitializeViewModels());
        }

        private async Task InitializeViewModels()
        {
            await _navigationService.Navigate<MenuViewModel, AuthReply>(authReply);
            await _navigationService.Navigate<RootViewModel, AuthReply>(authReply);
        }
        AuthReply authReply;


        public override void Prepare(AuthReply authReply)
        {
            this.authReply = authReply;
        }
    }
}
