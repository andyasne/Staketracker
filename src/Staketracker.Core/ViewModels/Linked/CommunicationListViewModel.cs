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
using Staketracker.Core.ViewModels.Communication;
using Staketracker.Core.ViewModels.Events;
using D = Staketracker.Core.Models.Events.D;
using PresentationMode = Staketracker.Core.Models.PresentationMode;
using Staketracker.Core.Res;
using System;
using Staketracker.Core.Models.LinkedTo;

namespace Staketracker.Core.ViewModels.Linked.Communication
{

    public class Communication
    {
        public string Name { get; set; }
        public string Date { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
    }

    public class LinkedListViewModel : BaseViewModel<AuthReply>

    {
        public IMvxCommand AddCommunicationCommand { get; }

        private string linkName;

        public string LinkName
        {

            get { return linkName; }
            set
            {
                SetField(ref linkName, value);

                linkName = value;

            }
        }

        private readonly IMvxNavigationService _navigationService;
        public IMvxCommand SearchCommand { get; }
        Staketracker.Core.Models.LinkedTo.LinkedTo linkedObj;

        public LinkedListViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;

            AddCommunicationCommand = new MvxCommand(OnCreateCommunication);

        }

        private void OnCreateCommunication() =>
            _navigationService.Navigate<CommunicationDetailViewModel, PresentationContext<AuthReply>>(
                new PresentationContext<AuthReply>(authReply, PresentationMode.Create));


        public override void Prepare(AuthReply parameter)
        {

            this.IsBusy = true;
            this.authReply = parameter;
            KeyValuePair<String, LinkedTo> _linkedTo = (KeyValuePair<String, LinkedTo>)authReply.attachment;
            linkedObj = _linkedTo.Value;



            RunSafe(GetCommunication(authReply), true, "Loading " + linkedObj.buttonLabel);

            this.IsBusy = false;
            base.Prepare();

            this.PageTitle = linkedObj.buttonLabel;
            this.LinkName = "Link to " + this.PageTitle;


        }

        public override async void ViewAppearing()
        {
            base.ViewAppearing();
            this.PageTitle = linkedObj.buttonLabel;
            this.LinkName = "Link to " + this.PageTitle;


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
                {

                    // OnSelectedEventChanged(selectedCommunication);
                }

            }
        }

        private void OnSelectedEventChanged(Staketracker.Core.Models.Communication.D communication)
        {
            if (Device.Idiom != TargetIdiom.Phone)
                return;
            string communicationSubject = "";
            if (communication.CommunicationSubject != null)
            {
                communicationSubject = communication.CommunicationSubject.ToString();
            }
            _navigationService.Navigate<CommunicationDetailViewModel, PresentationContext<AuthReply>>(
                new PresentationContext<AuthReply>(authReply, PresentationMode.Read, int.Parse(communication.PrimaryKey), communicationSubject));

        }

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
    }
}
