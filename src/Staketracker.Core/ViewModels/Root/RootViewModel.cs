using System.Globalization;
using System.Threading;
using Staketracker.Core.Resources;
using Staketracker.Core.ViewModels.Menu;

namespace Staketracker.Core.ViewModels.Root
{
    using MvvmCross.Navigation;
    using MvvmCross.ViewModels;
    using Staketracker.Core.Models;
    using Staketracker.Core.ViewModels.CommunicationList;
    using Staketracker.Core.ViewModels.Events;
    using System.Threading.Tasks;

    public class RootViewModel : BaseViewModel<AuthReply>
    {
        private readonly IMvxNavigationService _navigationService;

        private AuthReply authReply;

        public RootViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;
            CultureInfo info = new CultureInfo("am");
            Thread.CurrentThread.CurrentUICulture = info;

            AppResources.Culture = info;
        }

        public override void Prepare(AuthReply authReply)
        {
            this.authReply = authReply;
        }

        public override async void ViewAppearing()
        {
            base.ViewAppearing();

            MvxNotifyTask.Create(async () => RunSafe(InitializeViewModels(), true, "Loading Pages"));






        }

        private async Task InitializeViewModels()
        {
            try
            {


                await _navigationService.Navigate<Dashboard.DashboardViewModel>();

                await _navigationService.Navigate<SEventsListViewModel, AuthReply>(authReply);

                await _navigationService.Navigate<CommunicationListViewModel, AuthReply>(authReply);

                await _navigationService.Navigate<Stakeholders.StakeholderListViewModel, AuthReply>(authReply);

                await _navigationService.Navigate<Tasks.TasksListViewModel>();

                await _navigationService.Navigate<Commitments.CommitmentsViewModel>();
            }
            catch (System.Exception)
            {


            }
        }
    }
}
