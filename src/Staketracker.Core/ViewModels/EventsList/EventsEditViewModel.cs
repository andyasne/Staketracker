using System.Collections.ObjectModel;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using Staketracker.Core.Models;
using Xamarin.Forms;

namespace Staketracker.Core.ViewModels.EventsList
{

    public class EventsEditViewModel : BaseViewModel<PresentationContext<string>>

    {

        public EventsEditViewModel(IMvxNavigationService navigationService)
        {

        }

        public override void Prepare(PresentationContext<string> parameter)
        {
        }
    }


}
