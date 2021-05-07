using System.Threading.Tasks;
using System.Windows.Input;
using MvvmCross.Navigation;
using Staketracker.Core.ViewModels.Login;
using Xamarin.Forms;

namespace Staketracker.Core.ViewModels.Dashboard
{
    public class DashboardViewModel : BaseViewModel

    {
        private readonly IMvxNavigationService _navigationService;

        public ICommand SignOutCommand { get; set; }

        public DashboardViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;

            SignOutCommand = new Command(async () => await SignOut());
        }

        private async Task SignOut()
        {
            await _navigationService.Navigate<LoginViewModel>();
        }
    }
}
