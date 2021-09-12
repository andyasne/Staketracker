using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Staketracker.Core.Models;
using Staketracker.Core.ViewModels.Dashboard;
using Staketracker.Core.ViewModels.Events;
using Staketracker.Core.ViewModels.ForgetPassword;
using Staketracker.Core.ViewModels.Home;
using Staketracker.Core.ViewModels.Login;
using Staketracker.Core.ViewModels.Root;
using Staketracker.Core.ViewModels.Settings;
using Staketracker.Core.ViewModels.TwoStepVerification;
using Staketracker.Core.ViewModels.UserProfile;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Staketracker.Core.ViewModels.Menu
{
    public class MenuViewModel : BaseViewModel
    {
        public ICommand UserProfileCommand { get; set; }


        public MenuViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;
            MenuItemList = new MvxObservableCollection<string>()
            {
                "Project Team",
                "Topics",
                "Settings",
                "Help",
                "Sign Out"

            };

            ShowDetailPageAsyncCommand = new MvxAsyncCommand(ShowDetailPageAsync);
            UserProfileCommand = new Command(OpenUserProfile);


        }

        private async void OpenUserProfile()
        {
            MasterDetailPage masterDetailRootPage = (MasterDetailPage)Application.Current.MainPage;
            masterDetailRootPage.IsPresented = false;

            await _navigationService.Navigate<UserProfileViewModel>();


        }

        readonly IMvxNavigationService _navigationService;

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
                case "Project Team":
                    await _navigationService.Navigate<HomeViewModel>();
                    break;
                case "Sign Out":
                    await _navigationService.Navigate<LoginViewModel>();
                    break;
                case "Settings":
                    await _navigationService.Navigate<SettingsViewModel>();
                    break;
                case "Test":
                    {
                        _navigationService.Navigate<SEventDetailViewModel, PresentationContext<AuthReply>>(
                   new PresentationContext<AuthReply>(new AuthReply() { }, Models.PresentationMode.Create));
                    }
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
