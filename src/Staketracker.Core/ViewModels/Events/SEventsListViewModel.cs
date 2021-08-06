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
using Staketracker.Core.Models.ApiRequestBody;
using Staketracker.Core.Models.Events;
using Xamarin.Forms;
using D = Staketracker.Core.Models.Events.D;
using PresentationMode = Staketracker.Core.Models.PresentationMode;

namespace Staketracker.Core.ViewModels.Events
{
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
            var newS = new SEvent();
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
        private readonly MvxSubscriptionToken eventUpdatedMessageToken, eventDeletedMessageToken;

        private readonly IMvxNavigationService navigationService;

        internal AuthReply authReply;
        private LayoutMode currentLayoutMode;
        private string draftSearchTerm, listDescription, currentUserName;

        private ObservableCollection<SEvent> events;

        private ObservableCollection<SEvent> eventsomers;
        private bool isSearchEmpty, isBusy;

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
        private Staketracker.Core.Models.Events.D selectedEvents, selectedEventsDetail;

        public SEventsListViewModel(IMvxNavigationService navigationService)
        {
            this.navigationService = navigationService;

            SearchCommand = new MvxAsyncCommand(OnSearch);
            AddEventsCommand = new MvxCommand(OnCreateEvent);

            HeaderTitle = "Events";
        }

        public IMvxCommand SearchCommand { get; }

        public IMvxCommand AddEventsCommand { get; }

        public ObservableCollection<SEvent> Events
        {
            get => eventsomers;
            private set => SetField(ref eventsomers, value);
        }

        private Staketracker.Core.Models.Events.D selectedEvent;

        public Staketracker.Core.Models.Events.D SelectedEvent
        {
            get => selectedEvent;
            set
            {
                if (SetProperty(ref selectedEvent, value) && value != null)
                    OnSelectedEventChanged(value);
            }
        }

        public LayoutMode CurrentLayoutMode
        {
            get => currentLayoutMode;
            private set => SetProperty(ref currentLayoutMode, value);
        }

        public string DraftSearchTerm
        {
            get => draftSearchTerm;
            set
            {
                if (SetProperty(ref draftSearchTerm, value))
                    MvxNotifyTask.Create(async () => await DoSeach(value));
            }
        }

        public string CurrentUserName
        {
            get => currentUserName;
            private set => SetProperty(ref currentUserName, value);
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

        public override void Prepare(AuthReply authReply) => this.authReply = authReply;

        public override Task Initialize()
        {
            return RunSafe(GetEvents(authReply), true, "Loading Events");

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

        public async Task Refresh()
        {
        }

        private async Task FetchData()
        {
            //var target = await this.service.GetEventsAsync();
            //ApplyEventIndexing(target);
            //this.Events = target;
        }

        private void OnSelectedEventChanged(Staketracker.Core.Models.Events.D _event)
        {
            if (Device.Idiom != TargetIdiom.Phone)
                return;

            navigationService.Navigate<SEventDetailViewModel, PresentationContext<AuthReply>>(
                new PresentationContext<AuthReply>(authReply, PresentationMode.Edit, int.Parse(_event.PrimaryKey)));

            SelectedEvent = null;
        }

        private void ChangeLayoutMode(LayoutMode? mode) =>
            CurrentLayoutMode = mode ?? (CurrentLayoutMode == LayoutMode.Linear ? LayoutMode.Grid : LayoutMode.Linear);

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

        private void OnCreateEvent() =>
            navigationService.Navigate<SEventDetailViewModel, PresentationContext<AuthReply>>(
                new PresentationContext<AuthReply>(authReply, PresentationMode.Create));

        private void OnEditEvent(SEvent _event)
        {
            if (_event == null)
                return;
            navigationService.Navigate<SEventDetailViewModel, PresentationContext<AuthReply>>(
                new PresentationContext<AuthReply>(authReply, PresentationMode.Edit, int.Parse(_event.Id)));
        }

        //private void ShowAboutPage()
        //{
        //    this.navigationService.Navigate<AboutPageViewModel>();
        //}

        private async Task OnSearch()
        {
            if (Device.Idiom != TargetIdiom.Phone)
                return;

            await navigationService.Navigate<SearchResultsViewModel, SearchRequest>(
                new SearchRequest(SearchResultsViewModel.EventsContext, GetType()));
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
            var index = 0;
            foreach (SEvent item in events)
                //  item.Index = index;
                index++;
        }
    }
}
