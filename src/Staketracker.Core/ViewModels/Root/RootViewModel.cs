using System.Globalization;
using System.Threading;
//using Staketracker.Core.Resources;
using Staketracker.Core.ViewModels.Menu;

namespace Staketracker.Core.ViewModels.Root
{
    using MvvmCross.Commands;
    using MvvmCross.Navigation;
    using MvvmCross.ViewModels;
    using Staketracker.Core.Models;
    using Staketracker.Core.ViewModels.CommunicationList;
    using Staketracker.Core.ViewModels.Dashboard;
    using Staketracker.Core.ViewModels.Events;
    using Staketracker.Core.ViewModels.Login;
    using Staketracker.Core.ViewModels.TwoStepVerification;
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using Xamarin.Forms;

    public class RootViewModel : BaseViewModel<AuthReply>
    {
        private readonly IMvxNavigationService _navigationService;

        private AuthReply authReply;

        public RootViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;
            CultureInfo info = new CultureInfo("am");
            Thread.CurrentThread.CurrentUICulture = info;

            //AppResources.Culture = info;

            MenuItemList = new MvxObservableCollection<string>()
            {
                "Dashboard",
                "Events",
                "Login"
            };

            ShowDetailPageAsyncCommand = new MvxAsyncCommand(ShowDetailPageAsync);
        }


        public IMvxAsyncCommand ShowDetailPageAsyncCommand { get; private set; }

        private ObservableCollection<string> _menuItemList;
        public ObservableCollection<string> MenuItemList
        {
            get => _menuItemList;
            set => SetProperty(ref _menuItemList, value);
        }

        private async Task ShowDetailPageAsync()
        {
            // Implement your logic here.
            switch (SelectedMenuItem)
            {
                case "Dashboard":
                    await _navigationService.Navigate<DashboardViewModel>();
                    break;
                case "Login":
                    await _navigationService.Navigate<LoginViewModel>();
                    break;
                case "Events":
                    await _navigationService.Navigate<SEventsListViewModel>();
                    break;
                case "Two Step Verification":
                    await _navigationService.Navigate<TwoStepVerificationViewModel>();
                    break;
                default:
                    break;
            }

            if (Application.Current.MainPage is MasterDetailPage masterDetailPage)
            {
                masterDetailPage.IsPresented = false;
            }
            else if (Application.Current.MainPage is NavigationPage navigationPage
                     && navigationPage.CurrentPage is MasterDetailPage nestedMasterDetail)
            {
                nestedMasterDetail.IsPresented = false;
            }
        }

        private string _selectedMenuItem;
        public string SelectedMenuItem
        {
            get => _selectedMenuItem;
            set => SetProperty(ref _selectedMenuItem, value);
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
                //try
                //{

                //    await _navigationService.Navigate<MenuViewModel>();
                //}
                //catch (System.Exception)
                //{


                //}

                await _navigationService.Navigate<Dashboard.DashboardViewModel>();

                await _navigationService.Navigate<SEventsListViewModel, AuthReply>(authReply);

                await _navigationService.Navigate<CommunicationListViewModel, AuthReply>(authReply);


                await _navigationService.Navigate<Stakeholders.StakeholderListViewModel, AuthReply>(authReply);

                //await _navigationService.Navigate<Tasks.TasksListViewModel>();

                //await _navigationService.Navigate<Commitments.CommitmentsViewModel>();
            }
            catch (System.Exception)
            {


            }
        }
    }
}
