namespace Staketracker.Core.ViewModels.Root
{
    using System.Threading.Tasks;
    using MvvmCross.Navigation;
    using MvvmCross.ViewModels;
    using Staketracker.Core.Models;
    using Staketracker.Core.ViewModels.CommunicationList;
    using Staketracker.Core.ViewModels.EventsList;

    public class RootViewModel : BaseViewModel<AuthReply>
    {
        private readonly IMvxNavigationService _navigationService;

        private AuthReply authReply;

        public RootViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public override void Prepare(AuthReply authReply)
        {
            this.authReply = authReply;
        }

        public override async void ViewAppearing()
        {
            base.ViewAppearing();
            try
            {

                MvxNotifyTask.Create(async () => await this.InitializeViewModels());
            }
            catch (System.Exception)
            {


            }

        }

        private async Task InitializeViewModels()
        {
            try
            {


                await _navigationService.Navigate<Dashboard.DashboardViewModel>();
            }
            catch (System.Exception)
            {


            }

            try
            {

                await _navigationService.Navigate<SEventsListViewModel, AuthReply>(authReply);
            }
            catch (System.Exception)
            {


            }
            try
            {
                await _navigationService.Navigate<CommunicationListViewModel>();
            }
            catch (System.Exception)
            {


            }
            try
            {
                await _navigationService.Navigate<Stakeholders.StakeholdersListViewModel>();
            }
            catch (System.Exception)
            {


            }
            try
            {
                await _navigationService.Navigate<Tasks.TasksListViewModel>();
            }
            catch (System.Exception)
            {


            }
            try
            {
                await _navigationService.Navigate<Commitments.CommitmentsViewModel>();
            }
            catch (System.Exception)
            {


            }
        }
    }
}
