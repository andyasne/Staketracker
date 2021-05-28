namespace Staketracker.Core.ViewModels.Events
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

    public class SEvent
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Date { get; set; }

        public string Type { get; set; }

        public string Status { get; set; }
        public string Description { get; set; }


        public SEvent Copy()
        {
            SEvent newS = new SEvent()
            {
            };
            return newS;
        }
    }


    public class EventUpdatedMessage : MvxMessage
    {
        public EventUpdatedMessage(object sender, SEvent entity)
            : base(sender)
        {
            SEvent = entity;
        }

        public SEvent SEvent { get; }
    }
    public class EventDeletedMessage : MvxMessage
    {
        public EventDeletedMessage(object sender, SEvent entity)
            : base(sender)
        {
            SEvent = entity;
        }

        public SEvent SEvent { get; }
    }

    public class SEventsListViewModel : BaseViewModel<AuthReply>
    {
        private SEvent selectedEvents, selectedEventsDetail;

        private ObservableCollection<SEvent> eventsomers;

        private string headerTitle;

        private IMvxNavigationService navigationService;

        public IMvxCommand SearchCommand { get; }

        public IMvxCommand AddEventsCommand { get; }

        public SEventsListViewModel(IMvxNavigationService navigationService)
        {
            this.headerTitle = "Events";

            this.navigationService = navigationService;

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
            this.eventsomers = new ObservableCollection<SEvent>();

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
                    SEvent _events = new SEvent();
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

        public ObservableCollection<SEvent> Events { get => eventsomers; private set => SetField(ref eventsomers, value); }



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
        //private IErpService service;
        private SEvent selectedEvent;
        private ObservableCollection<SEvent> events;
        private LayoutMode currentLayoutMode;
        private bool isSearchEmpty, isBusy;
        private string draftSearchTerm, listDescription, currentUserName;
        private readonly MvxSubscriptionToken eventUpdatedMessageToken, eventDeletedMessageToken;

        //public ObservableCollection<Events> Events
        //{
        //    get => events;
        //    private set => SetProperty(ref events, value);
        //}

        public SEvent SelectedEvent
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

        private void OnSelectedEventChanged(SEvent _event)
        {
            if (Device.Idiom != TargetIdiom.Phone)
                return;

            this.navigationService.Navigate<SEventDetailViewModel, PresentationContext<SEvent>>(new PresentationContext<SEvent>(_event, Models.PresentationMode.Read));

            this.SelectedEvent = null;
        }

        private void ChangeLayoutMode(LayoutMode? mode)
        {
            this.CurrentLayoutMode = mode ?? (this.CurrentLayoutMode == LayoutMode.Linear ?
                LayoutMode.Grid : LayoutMode.Linear);
        }

        private async Task DoSeach(string term)
        {
            ObservableCollection<SEvent> newEvents;
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
            //var found = this.events.SingleOrDefault(c => c.Id == message.SEvent.Id);
            //if (found != null)
            //{
            //    this.events.Remove(found);
            //    this.IsSearchEmpty = !this.events.Any();
            //    ApplyEventIndexing(this.events);
            //}
        }

        private void OnCreateEvent()
        {
            this.navigationService.Navigate<SEventDetailViewModel, PresentationContext<SEvent>>(new PresentationContext<SEvent>(new SEvent(), Models.PresentationMode.Create));

        }

        private void OnEditEvent(SEvent _event)
        {
            if (_event == null)
                return;
            this.navigationService.Navigate<SEventDetailViewModel, PresentationContext<SEvent>>(new PresentationContext<SEvent>(_event, Models.PresentationMode.Edit));

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

        private async Task OnDeleteEvent(SEvent _event)
        {
            if (_event == null)
                return;

            //bool result = await  DisplayAlert("Delete product", $"Are you sure you want to delete SEvent {SEvent.Name}?", "Yes", "No");
            //if (!result)
            //    return;

            //     await this.service.RemoveEventAsync(SEvent);
        }

        private static void ApplyEventIndexing(IEnumerable<SEvent> events)
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

