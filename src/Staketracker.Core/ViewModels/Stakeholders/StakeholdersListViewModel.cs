using System.Net.Http;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using Staketracker.Core.Helpers;
using Staketracker.Core.Models;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Staketracker.Core.Models.ApiRequestBody;
using Staketracker.Core.Models.Communication;
using Staketracker.Core.Models.EventsFormValue;
using Staketracker.Core.Models.Stakeholders;
using Xamarin.Forms;

namespace Staketracker.Core.ViewModels.Stakeholders
{
    public class StakeholdersListViewModel : BaseViewModel<AuthReply>

    {



        private readonly IMvxNavigationService _navigationService;
        public IMvxCommand SearchCommand { get; }

        public StakeholdersListViewModel(IMvxNavigationService navigationService)
        {
            this.HeaderTitle = "Stakeholder";

            _navigationService = navigationService;

            this.SearchCommand = new MvxAsyncCommand(OnSearch);
        }

        public AuthReply authReply;

        public override void Prepare(AuthReply parameter)
        {
            base.Prepare();

            this.IsBusy = true;
            this.authReply = parameter;
            //this.Mode = parameter.Mode;
            RunSafe(GetLandParcelStakeholderDetails(authReply), true, "Loading Stakeholders");
            this.IsBusy = false;

        }
        public async override Task Initialize()
        {
            await base.Initialize();

            GetFormandDropDownFields(authReply, FormType.GroupedStakeholders);




        }

        private Models.Stakeholders.Stakeholders _allStakeholders;
        public Models.Stakeholders.Stakeholders allStakeholders
        {
            get => _allStakeholders;
            private set => SetField(ref _allStakeholders, value);
        }



        private Models.Stakeholders.Stakeholders stakeholders;
        internal async Task GetLandParcelStakeholderDetails(AuthReply authReply)
        {


            StakeholderBody body = new StakeholderBody();
            body.projectId = authReply.d.projectId;
            body.userId = authReply.d.userId;

            var apiReq = new jsonTextObj(body);
            HttpResponseMessage stakeholders = await ApiManager.GetAllStakeholders(apiReq, authReply.d.sessionId);

            if (stakeholders.IsSuccessStatusCode)
            {
                var response = await stakeholders.Content.ReadAsStringAsync();
                allStakeholders = await Task.Run(() => JsonConvert.DeserializeObject<Models.Stakeholders.Stakeholders>(response));


            }
            else
                await PageDialog.AlertAsync("API Error While retrieving", "API Response Error", "Ok");

        }


        public async Task Refresh()
        {
        }


        private async Task OnSearch()
        {
            if (Device.Idiom != TargetIdiom.Phone)
                return;

            await this._navigationService.Navigate<SearchResultsViewModel>();
        }
    }
}
