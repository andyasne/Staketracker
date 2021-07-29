using System.Net.Http;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using Staketracker.Core.Helpers;
using Staketracker.Core.Models;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Staketracker.Core.Models.ApiRequestBody;
using Staketracker.Core.Models.Communication;
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
            RunSafe(GetGroupStakeholderDetails(authReply), true, "Loading Group Stakeholders");
            RunSafe(GetIndividualStakeholderDetails(authReply), true, "Loading Individual Stakeholders");
            RunSafe(GetLandParcelStakeholderDetails(authReply), true, "Loading Land Parcel");
            this.IsBusy = false;

        }
        public async override Task Initialize()
        {
            await base.Initialize();

            GetFormandDropDownFields(authReply, FormType.GroupedStakeholders);




        }

        private CommunicationReply communicationReply;
        public CommunicationReply communicationReply_
        {
            get => communicationReply;
            private set => SetField(ref communicationReply, value);
        }

        internal async Task GetGroupStakeholderDetails(AuthReply authReply)
        {

            var apiReq = new APIRequestBody(authReply);
            HttpResponseMessage communications = await ApiManager.GetGroupStakeholderDetails(apiReq, authReply.d.sessionId);

            if (communications.IsSuccessStatusCode)
            {
                var response = await communications.Content.ReadAsStringAsync();
                communicationReply_ = await Task.Run(() => JsonConvert.DeserializeObject<CommunicationReply>(response));


            }
            else
                await PageDialog.AlertAsync("API Error While retrieving", "API Response Error", "Ok");

        }

        internal async Task GetIndividualStakeholderDetails(AuthReply authReply)
        {

            var apiReq = new APIRequestBody(authReply);
            HttpResponseMessage communications = await ApiManager.GetIndividualStakeholderDetails(apiReq, authReply.d.sessionId);

            if (communications.IsSuccessStatusCode)
            {
                var response = await communications.Content.ReadAsStringAsync();
                communicationReply_ = await Task.Run(() => JsonConvert.DeserializeObject<CommunicationReply>(response));


            }
            else
                await PageDialog.AlertAsync("API Error While retrieving", "API Response Error", "Ok");

        }
        internal async Task GetLandParcelStakeholderDetails(AuthReply authReply)
        {

            var apiReq = new APIRequestBody(authReply);
            HttpResponseMessage communications = await ApiManager.GetLandParcelStakeholderDetails(apiReq, authReply.d.sessionId);

            if (communications.IsSuccessStatusCode)
            {
                var response = await communications.Content.ReadAsStringAsync();
                communicationReply_ = await Task.Run(() => JsonConvert.DeserializeObject<CommunicationReply>(response));


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
