namespace Staketracker.Core.ViewModels.EventsList
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using MvvmCross.Commands;
    using MvvmCross.Navigation;
    using MvvmCross.Plugin.Messenger;
    using MvvmCross.ViewModels;
    using Newtonsoft.Json;
    using Staketracker.Core.Models;
    using Staketracker.Core.Models.Events;
    using Xamarin.Forms;

    public class Events
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Date { get; set; }

        public string Type { get; set; }

        public string Status { get; set; }
    }


    public class EventUpdatedMessage : MvxMessage
    {
        public EventUpdatedMessage(object sender, Events entity)
            : base(sender)
        {
            Event = entity;
        }

        public Events Event { get; }
    }
    public class EventDeletedMessage : MvxMessage
    {
        public EventDeletedMessage(object sender, Events entity)
            : base(sender)
        {
            Event = entity;
        }

        public Events Event { get; }
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
            this.AddEventsCommand = new MvxCommand(OnCreateEvent);
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
        //------------------------------------
        //public async override void Prepare()
        //{
        //    base.Prepare();

        //    this.IsBusy = true;
        //    await FetchData();
        //    //if (await this.service.SyncIfNeededAsync())
        //    //{
        //    //    await FetchData();
        //    //}
        //    this.IsBusy = false;
        //}
        private IMvxNavigationService navigationService;
        //private IErpService service;
        private Events selectedEvent;
        private ObservableCollection<Events> events;
        private LayoutMode currentLayoutMode;
        private bool isSearchEmpty, isBusy;
        private string draftSearchTerm, listDescription, currentUserName;
        private readonly MvxSubscriptionToken eventUpdatedMessageToken, eventDeletedMessageToken;

        //public ObservableCollection<Events> Events
        //{
        //    get => events;
        //    private set => SetProperty(ref events, value);
        //}

        public Events SelectedEvent
        {
            get => selectedEvent;
            set
            {
                if (SetProperty(ref selectedEvent, value) && value != null)
                {
                    OnSelectedEventChanged(value);
                }
            }
        }

        public LayoutMode CurrentLayoutMode
        {
            get => currentLayoutMode;
            private set => SetProperty(ref currentLayoutMode, value);
        }

        public string DraftSearchTerm
        {
            get => this.draftSearchTerm;
            set
            {
                if (SetProperty(ref this.draftSearchTerm, value))
                {
                    MvxNotifyTask.Create(async () => await DoSeach(value));
                }
            }
        }

        public string CurrentUserName
        {
            get => this.currentUserName;
            private set => SetProperty(ref this.currentUserName, value);
        }

        public string ListDescription
        {
            get => listDescription;
            private set => SetProperty(ref listDescription, value);
        }

        public bool IsSearchEmpty
        {
            get => isSearchEmpty;
            set => SetProperty(ref isSearchEmpty, value);
        }

        public bool IsBusy
        {
            get => isBusy;
            set => SetProperty(ref isBusy, value);
        }

        public ICommand ToggleLayoutModeCommand { get; }
        public ICommand CreateEventCommand { get; }
        public ICommand EditEventCommand { get; }
        public ICommand DeleteEventCommand { get; }
        //public ICommand SearchCommand { get; }
        public ICommand AboutCommand { get; }

        //public async Task Refresh()
        //{
        //    await this.service.RefreshCustomers();
        //    await this.FetchData();
        //}

        private async Task FetchData()
        {
            //var target = await this.service.GetEventsAsync();
            //ApplyEventIndexing(target);
            //this.Events = target;
        }

        private void OnSelectedEventChanged(Events _event)
        {
            if (Device.Idiom != TargetIdiom.Phone)
                return;

            this.navigationService.Navigate<EventsEditViewModel, PresentationContext<string>>(new PresentationContext<string>(_event.Id, Models.PresentationMode.Read));
            this.SelectedEvent = null;
        }

        private void ChangeLayoutMode(LayoutMode? mode)
        {
            this.CurrentLayoutMode = mode ?? (this.CurrentLayoutMode == LayoutMode.Linear ?
                LayoutMode.Grid : LayoutMode.Linear);
        }

        private async Task DoSeach(string term)
        {
            ObservableCollection<Events> newEvents;
            //if (string.IsNullOrEmpty(term))
            //    newEvents = await this.service.GetEventsAsync();
            //else
            //    newEvents = (await this.service.GetEventsAsync(term));
            //ApplyEventIndexing(newEvents);
            //this.Events = newEvents;
            //ListDescription = string.IsNullOrEmpty(term) ? "All Events" : term;
            //this.IsSearchEmpty = newEvents == null || !newEvents.Any();
        }

        private async void OnEventUpdated(EventUpdatedMessage message)
        {
            //    var updatedEvents = (await this.service.GetEventsAsync());
            //    ApplyEventIndexing(updatedEvents);
            //    Device.BeginInvokeOnMainThread(() => this.Events = updatedEvents);
        }

        private void OnEventDeleted(EventDeletedMessage message)
        {
            //var found = this.events.SingleOrDefault(c => c.Id == message.Event.Id);
            //if (found != null)
            //{
            //    this.events.Remove(found);
            //    this.IsSearchEmpty = !this.events.Any();
            //    ApplyEventIndexing(this.events);
            //}
        }

        private void OnCreateEvent()
        {
            this.navigationService.Navigate<EventsEditViewModel, PresentationContext<string>>(new PresentationContext<string>(null, Models.PresentationMode.Create));
        }

        private void OnEditEvent(Events _event)
        {
            if (_event == null)
                return;

            this.navigationService.Navigate<EventsEditViewModel, PresentationContext<string>>(new PresentationContext<string>(_event.Id, Models.PresentationMode.Edit));
        }

        //private void ShowAboutPage()
        //{
        //    this.navigationService.Navigate<AboutPageViewModel>();
        //}

        private async Task OnSearch()
        {
            if (Device.Idiom != TargetIdiom.Phone)
                return;

            await this.navigationService.Navigate<SearchResultsViewModel, SearchRequest>(new SearchRequest(SearchResultsViewModel.EventsContext, this.GetType()));
        }

        private async Task OnDeleteEvent(Events _event)
        {
            if (_event == null)
                return;

            //bool result = await  DisplayAlert("Delete product", $"Are you sure you want to delete event {event.Name}?", "Yes", "No");
            //if (!result)
            //    return;

            //     await this.service.RemoveEventAsync(event);
        }

        private static void ApplyEventIndexing(IEnumerable<Events> events)
        {
            int index = 0;
            foreach (var item in events)
            {
                //  item.Index = index;
                index++;
            }
        }
    }
}

