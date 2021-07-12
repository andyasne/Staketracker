using System.Collections.Generic;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using Staketracker.Core.Helpers;
using Staketracker.Core.Models;
using System.ComponentModel;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Staketracker.Core.Models.ApiRequestBody;
using Staketracker.Core.Models.Communication;
using Staketracker.Core.Models.Events;
using Xamarin.Forms;
using System.Linq;
using D = Staketracker.Core.Models.Events.D;


namespace Staketracker.Core.ViewModels.CommunicationList
{


    public class Communication
    {
        public string Name { get; set; }
        public string Date { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
    }


    public class CommunicationListViewModel : BaseViewModel<AuthReply>

    {


        private readonly IMvxNavigationService _navigationService;
        public IMvxCommand SearchCommand { get; }

        public CommunicationListViewModel(IMvxNavigationService navigationService)
        {
            this.HeaderTitle = "Communication";

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
            RunSafe(GetCommunication(authReply), true, "Loading Communication");
            this.IsBusy = false;

        }
        private bool isSearchEmpty, isBusy;
        public bool IsBusy
        {
            get => isBusy;
            set => SetProperty(ref isBusy, value);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private Staketracker.Core.Models.Communication.D selectedCommunication;


        public Staketracker.Core.Models.Communication.D SelectedCommunication
        {
            get => selectedCommunication;
            set
            {
                if (SetProperty(ref selectedCommunication, value) && value != null)
                { }
                //   OnSelectedEventChanged(value);
            }
        }

        //public override Task Initialize()
        //{
        //    //  base.Initialize();


        //}

        private CommunicationReply communicationReply;
        public CommunicationReply communicationReply_
        {
            get => communicationReply;
            private set => SetField(ref communicationReply, value);
        }

        internal async Task GetCommunication(AuthReply authReply)
        {

            var apiReq = new APIRequestBody(authReply);
            HttpResponseMessage communications = await ApiManager.GetAllCommunications(apiReq, authReply.d.sessionId);

            if (communications.IsSuccessStatusCode)
            {
                var response = await communications.Content.ReadAsStringAsync();
                communicationReply_ = await Task.Run(() => JsonConvert.DeserializeObject<CommunicationReply>(response));


            }
            else
                await PageDialog.AlertAsync("API Error While retrieving Communication", "API Response Error", "Ok");

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
