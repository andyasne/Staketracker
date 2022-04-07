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
using System.Windows.Input;
using Staketracker.Core.Models.EventsFormValue;

namespace Staketracker.Core.ViewModels.Linked.CustomMultiselect
{

    public class Communication
    {
        public string Name { get; set; }
        public string Date { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
    }

    public class CustomMultiselectViewModel : BaseViewModel<AuthReply>

    {
        public IMvxCommand AddCommunicationCommand { get; }
        public ICommand OpenCustomMultiselect { get; set; }

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

        public CustomMultiselectViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;

            //   OpenCustomMultiselect = new MvxCommand(OpenCustomMultiselect);

        }

        private void OnOpenCustomMultiselect() =>
            _navigationService.Navigate<CommunicationDetailViewModel, PresentationContext<AuthReply>>(
                new PresentationContext<AuthReply>(authReply, PresentationMode.Create));


        public override async void Prepare(AuthReply parameter)
        {

            this.IsBusy = true;
            this.authReply = parameter;
            //      KeyValuePair<String, LinkedTo> _linkedTo = (KeyValuePair<String, LinkedTo>)authReply.attachment;
            // linkedObj = _linkedTo.Value;


            CommunicationDetailReq body = new CommunicationDetailReq()
            {
                projectId = authReply.d.projectId,
                userId = authReply.d.userId,
                ID = primaryKey
            };

            HttpResponseMessage communications = await ApiManager.GetCommunicationDetails(new jsonTextObj(body), authReply.d.sessionId);
            if (communications.IsSuccessStatusCode)
            {
                var response = await communications.Content.ReadAsStringAsync();
                communicationReply_ = await Task.Run(() => JsonConvert.DeserializeObject<CommunicationReply>(response));

            }
            //  RunSafe(GetCommunication(authReply), true, "Loading ");
            this.IsBusy = false;
            base.Prepare();


        }
        private EventsReply eventsReply;
        public EventsReply EventsReply_
        {
            get => eventsReply;
            private set => SetField(ref eventsReply, value);
        }
        internal async Task GetEvents(AuthReply authReply)
        {

            var apiReq = new APIRequestBody(authReply);
            HttpResponseMessage events = await ApiManager.GetEvents(apiReq, authReply.d.sessionId);

            if (events.IsSuccessStatusCode)
            {
                var response = await events.Content.ReadAsStringAsync();
                EventsReply_ = await Task.Run(() => JsonConvert.DeserializeObject<EventsReply>(response));
            }
            else
                await PageDialog.AlertAsync("API Error While retrieving Events", "API Response Error", "Ok");

        }

        public override async void ViewAppearing()
        {
            base.ViewAppearing();
            this.PageTitle = linkedObj.buttonLabel;
            this.LinkName = "Link to " + this.PageTitle;
            RunSafe(GetEvents(authReply), true, "Loading Events");



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
