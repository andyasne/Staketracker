using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Staketracker.Core.ViewModels.Dashboard;
using Staketracker.Core.ViewModels.EventsList;
using Staketracker.Core.ViewModels.Home;
using Staketracker.Core.ViewModels.Login;
using Staketracker.Core.ViewModels.TwoStepVerification;
using Xamarin.Forms;

namespace Staketracker.Core.ViewModels.Menu
{
    public class MenuViewModel : BaseViewModel
    {
        readonly IMvxNavigationService _navigationService;

        public IMvxAsyncCommand ShowDetailPageAsyncCommand { get; private set; }

        public MenuViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;
            MenuItemList = new MvxObservableCollection<string>()
            {
                "Dashboard",
                "Events",
                "Two Step Verification",
                "Login"
            };

            ShowDetailPageAsyncCommand = new MvxAsyncCommand(ShowDetailPageAsync);
        }

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
                    await _navigationService.Navigate<EventsListViewModel>();
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
    }
}
