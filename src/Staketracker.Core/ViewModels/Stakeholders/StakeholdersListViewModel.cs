using MvvmCross.Commands;
using MvvmCross.Navigation;
using Staketracker.Core.Helpers;
using Staketracker.Core.Models;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Staketracker.Core.ViewModels.Stakeholders
{
    public class StakeholdersListViewModel : BaseViewModel<AuthReply>

    {

        private string headerTitle;

        private readonly IMvxNavigationService _navigationService;
        public IMvxCommand SearchCommand { get; }

        public StakeholdersListViewModel(IMvxNavigationService navigationService)
        {
            this.headerTitle = "Stakeholder";

            _navigationService = navigationService;

            this.SearchCommand = new MvxAsyncCommand(OnSearch);
        }

        public AuthReply authReply;
        public override void Prepare(AuthReply parameter)
        {
            this.authReply = parameter;
        }
        public async override Task Initialize()
        {
            await base.Initialize();

            GetFormandDropDownFields(authReply, FormType.GroupedStakeholders);




        }




        public async Task Refresh()
        {
        }

        public string HeaderTitle
        {
            get => headerTitle;
            private set => SetProperty(ref headerTitle, value);
        }

        private async Task OnSearch()
        {
            if (Device.Idiom != TargetIdiom.Phone)
                return;

            await this._navigationService.Navigate<SearchResultsViewModel>();
        }
    }
}
