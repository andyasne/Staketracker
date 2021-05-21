namespace Staketracker.Core.ViewModels.EventsList
{
    using System.Collections.ObjectModel;
    using System.Net.Http;
    using System.Threading.Tasks;
    using MvvmCross.Commands;
    using MvvmCross.Navigation;
    using Newtonsoft.Json;
    using Staketracker.Core.Models;
    using Staketracker.Core.Models.Events;
    using Xamarin.Forms;

    public class Events
    {
        public string Name { get; set; }

        public string Date { get; set; }

        public string Type { get; set; }

        public string Status { get; set; }
    }

    public class EventsListViewModel : BaseViewModel<AuthReply>
    {
        private Events selectedEvents, selectedEventsDetail;

        private ObservableCollection<Events> eventsomers;

        private string headerTitle;

        private readonly IMvxNavigationService _navigationService;

        public IMvxCommand SearchCommand { get; }

        public IMvxCommand AddEventsCommand { get; }

        public EventsListViewModel(IMvxNavigationService navigationService)
        {
            this.headerTitle = "Events";

            _navigationService = navigationService;

            this.SearchCommand = new MvxAsyncCommand(OnSearch);
            this.AddEventsCommand = new MvxAsyncCommand(OnAddEvents);
        }

        internal AuthReply authReply;

        public override void Prepare(AuthReply authReply)
        {
            this.authReply = authReply;
        }

        public override Task Initialize()
        {
            this.eventsomers = new ObservableCollection<Events>();

            return GetEvents(authReply);
        }

        internal async Task GetEvents(AuthReply authReply)
        {

            EventsReply eventsReply;
            Models.ApiRequestBody.APIRequestBody apiReq = new Models.ApiRequestBody.APIRequestBody(authReply);
            HttpResponseMessage events = await ApiManager.GetEvents(apiReq, authReply.d.sessionId);

            if (events.IsSuccessStatusCode)
            {
                var response = await events.Content.ReadAsStringAsync();
                EventsReply eventsRep = await Task.Run(() => JsonConvert.DeserializeObject<EventsReply>(response));
                // return eventsReply;

                foreach (Models.Events.D d in eventsRep.d)
                {
                    Events _events = new Events();
                    _events.Name = d.Name;
                    _events.Date = d.EventDate.ToShortDateString();
                    _events.Type = d.Type;
                    eventsomers.Add(_events);

                }
            }
            else
            {
                await PageDialog.AlertAsync("API Error While retrieving Email address for the user", "API Response Error", "Ok");
                //  return null;
            }
        }

        public ObservableCollection<Events> Events { get => eventsomers; private set => SetField(ref eventsomers, value); }

        public Events SelectedEvents
        {
            get => selectedEvents;
            set
            {
                if (SetProperty(ref selectedEvents, value) && value != null)
                {
                    SetField(ref selectedEvents, value);
                }
            }
        }

        public async Task Refresh()
        {
        }

        public string HeaderTitle { get => headerTitle; private set => SetProperty(ref headerTitle, value); }

        private async Task OnSearch()
        {
            if (Device.Idiom != TargetIdiom.Phone)
                return;

            await this._navigationService.Navigate<SearchResultsViewModel>();
        }

        private async Task OnAddEvents()
        {
            await this._navigationService.Navigate<EventsEditViewModel>();
        }
    }
}
