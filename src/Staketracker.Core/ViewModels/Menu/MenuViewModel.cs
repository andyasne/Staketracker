using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Staketracker.Core.Models;
using Staketracker.Core.ViewModels.Dashboard;
using Staketracker.Core.ViewModels.Events;
using Staketracker.Core.ViewModels.ForgetPassword;
using Staketracker.Core.ViewModels.Home;
using Staketracker.Core.ViewModels.Issues;
using Staketracker.Core.ViewModels.Login;
using Staketracker.Core.ViewModels.ProjectTeam;
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
    public class MenuViewModel : BaseViewModel<AuthReply>
    {
        public ICommand UserProfileCommand { get; set; }

        public ICommand ProjectTeamCommand { get; set; }
        public ICommand TopicsCommand { get; set; }
        public ICommand SettingsCommand { get; set; }
        public ICommand HelpCommand { get; set; }
        public ICommand SignOutCommand { get; set; }



        public MenuViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;

            SettingsCommand = new Command(OpenSettingsPage);

            UserProfileCommand = new Command(OpenUserProfile);

            ProjectTeamCommand = new Command(OpenProjectTeam);

            TopicsCommand = new Command(OpenIssues);

            SettingsCommand = new Command(OpenSettingsPage);

            HelpCommand = new Command(() =>
            {
                OnDevelopment().Start();


            });
            SignOutCommand = new Command(SignOut);



        }

        public override void Prepare(AuthReply authReply)
        {
            this.authReply = authReply;
        }
        private async void OpenProjectTeam()
        {

            await _navigationService.Navigate<ProjectTeamListViewModel, AuthReply>(authReply);

            hideMainMenu();

        }
        private async void OpenIssues()
        {

            await _navigationService.Navigate<IssuesListViewModel, AuthReply>(authReply);

            hideMainMenu();

        }

        private async void OpenUserProfile()
        {

            await _navigationService.Navigate<UserProfileViewModel>();
            hideMainMenu();



        }

        readonly IMvxNavigationService _navigationService;

        private async void OpenSettingsPage()
        {
            await this._navigationService.Navigate<SettingsViewModel, AuthReply>(authReply);



            hideMainMenu();
        }

        private static void hideMainMenu()
        {
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

        private async void SignOut()
        {
            MasterDetailPage masterDetailRootPage = (MasterDetailPage)Application.Current.MainPage;
            masterDetailRootPage.IsPresented = false;

            await _navigationService.Navigate<LoginViewModel>();


            //if (Application.Current.MainPage is MasterDetailPage masterDetailPage)
            //{
            //    masterDetailPage.IsPresented = false;
            //}
            //else if (Application.Current.MainPage is NavigationPage navigationPage
            //         && navigationPage.CurrentPage is MasterDetailPage nestedMasterDetail)
            //{
            //    nestedMasterDetail.IsPresented = false;
            //}
        }

    }

}
