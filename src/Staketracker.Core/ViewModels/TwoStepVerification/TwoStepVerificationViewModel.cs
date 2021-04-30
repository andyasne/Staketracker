
namespace Staketracker.Core.ViewModels.TwoStepVerification
{
    using System;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using MvvmCross.Navigation;
    using Newtonsoft.Json;
    using Staketracker.Core.Models;
    using Staketracker.Core.ViewModels.Contacts;
    using Xamarin.Forms;

    public class TwoStepVerificationViewModel : BaseViewModel
    {


        public TwoStepVerificationViewModel(IMvxNavigationService navigationService)
        {
        }

        internal async Task AuthenticateUser(LoginAPIBody loginApiBody)
        {

        }
    }
}
