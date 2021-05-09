using System.Threading.Tasks;
using System.Windows.Input;
using MvvmCross.Navigation;
using Staketracker.Core.ViewModels.Login;
using Xamarin.Forms;

namespace Staketracker.Core.ViewModels.EventsList
{
    public class EventsListViewModel : BaseViewModel

    {
        private readonly IMvxNavigationService _navigationService;

      
        public EventsListViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;

         }
         
    }
}
