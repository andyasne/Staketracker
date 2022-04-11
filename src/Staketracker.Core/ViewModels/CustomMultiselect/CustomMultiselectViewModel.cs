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
using System.Collections.ObjectModel;

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


        private readonly IMvxNavigationService _navigationService;
        public IMvxCommand SearchCommand { get; }
        Staketracker.Core.Models.LinkedTo.LinkedTo linkedObj;
        public event PropertyChangedEventHandler PropertyChanged;

        public CustomMultiselectViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;

            //   OpenCustomMultiselect = new MvxCommand(OpenCustomMultiselect);

        }
        protected void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        private void OnOpenCustomMultiselect() =>
            _navigationService.Navigate<CommunicationDetailViewModel, PresentationContext<AuthReply>>(
                new PresentationContext<AuthReply>(authReply, PresentationMode.Create));


        public override async void Prepare(AuthReply parameter)
        {

            this.authReply = parameter;
            base.Prepare();


        }

        private ObservableCollection<MultiSelectModel> multiSelectModels = new ObservableCollection<MultiSelectModel>();
        public ObservableCollection<MultiSelectModel> MultiSelectModels
        {
            get => multiSelectModels;
            set
            {
                SetField(ref multiSelectModels, value);

            }
        }



        private ObservableCollection<MultiSelectModel> selectedRecords = new ObservableCollection<MultiSelectModel>();
        public ObservableCollection<MultiSelectModel> SelectedRecords
        {
            get => selectedRecords;
            set
            {
                SetField(ref selectedRecords, value);

            }
        }


        private EventsReply eventsReply;
        public EventsReply EventsReply_
        {
            get => eventsReply;
            set => SetField(ref eventsReply, value);
        }

        internal async Task GetEvents(AuthReply authReply)
        {

            var apiReq = new APIRequestBody(authReply);
            HttpResponseMessage events = await ApiManager.GetEvents(apiReq, authReply.d.sessionId);

            if (events.IsSuccessStatusCode)
            {
                var response = await events.Content.ReadAsStringAsync();
                EventsReply_ = await Task.Run(() => JsonConvert.DeserializeObject<EventsReply>(response));
                int index = 0;
                foreach (Staketracker.Core.Models.Events.D eventReply in EventsReply_.d)
                {
                    MultiSelectModel multiSelectObj = new MultiSelectModel(index, eventReply.Name, false, eventsReply);
                    MultiSelectModels.Add(multiSelectObj);
                    index++;
                }


            }
            else
                await PageDialog.AlertAsync("API Error While retrieving Events", "API Response Error", "Ok");

        }

        public override async void ViewAppearing()
        {
            base.ViewAppearing();

            RunSafe(GetEvents(authReply), true, "Loading Events");



        }

    }
}
