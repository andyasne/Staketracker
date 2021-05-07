
namespace Staketracker.Core.ViewModels.TwoStepVerification
{
    using System;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using MvvmCross.Navigation;
    using Newtonsoft.Json;
    using Staketracker.Core.Models;
    using Staketracker.Core.ViewModels.Dashboard;
    using Xamarin.Forms;

    public class TwoStepVerificationViewModel : BaseViewModel
    {

        public ICommand OpenDashboardPageCommand { get; set; }

        internal readonly IMvxNavigationService _navigationService;

        public TwoStepVerificationViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;

            OpenDashboardPageCommand = new Command(async () => await RunSafe(OpenDashboardPage()));

        }

        internal async Task OpenDashboardPage()
        {

            await _navigationService.Navigate<DashboardViewModel>();

        }
    }
}
