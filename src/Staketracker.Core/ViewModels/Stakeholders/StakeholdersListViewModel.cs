using System.Collections.ObjectModel;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using Xamarin.Forms;

namespace Staketracker.Core.ViewModels.Stakeholders
{ 
    public class StakeholdersListViewModel : BaseViewModel

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


        public async override void Prepare()
        {
            base.Prepare();
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
