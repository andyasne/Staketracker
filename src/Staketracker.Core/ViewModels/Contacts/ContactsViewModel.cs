using System.Threading.Tasks;
using System.Windows.Input;
using MvvmCross.Navigation;
using Staketracker.Core.ViewModels.Login;
using Xamarin.Forms;

namespace Staketracker.Core.ViewModels.Contacts
{

    public class ContactsViewModel : BaseViewModel

    {
        readonly IMvxNavigationService _navigationService;

        public ICommand SignOutCommand { get; set; }
        public ContactsViewModel(IMvxNavigationService navigationService)
        {

            _navigationService = navigationService;

            SignOutCommand = new Command(async () => await SignOut());
        }

        async Task SignOut()
        {
            await _navigationService.Navigate<LoginViewModel>();
        }

    }
}
