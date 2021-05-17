using System.Threading.Tasks;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Staketracker.Core.ViewModels.CommunicationList;
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

            MvxNotifyTask.Create(async () => await this.InitializeViewModels());

        }


        private async Task InitializeViewModels()
        {

            //await _navigationService.Navigate<MenuViewModel>();
            await _navigationService.Navigate<Dashboard.DashboardViewModel>();
            await _navigationService.Navigate<EventsListViewModel>();
            await _navigationService.Navigate<CommunicationListViewModel>();
            await _navigationService.Navigate<Stakeholders.StakeholdersListViewModel>();
            await _navigationService.Navigate<Tasks.TasksListViewModel>();

        }
    }
}
