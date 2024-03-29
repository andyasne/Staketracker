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
using Staketracker.Core.Res;
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

        private LayoutMode currentLayoutMode;
        private string draftSearchTerm, listDescription, currentUserName;

        private ObservableCollection<SEvent> events;

        private ObservableCollection<SEvent> eventsomers;
        private bool isSearchEmpty, isBusy;

        private Staketracker.Core.Models.Events.D selectedStakeholders, selectedStakeholdersDetail;

        public SEventsListViewModel(IMvxNavigationService navigationService)
        {
            this.navigationService = navigationService;

            SearchCommand = new MvxAsyncCommand(OnSearch);
            AddEventsCommand = new MvxCommand(OnCreateEvent);
            PageTitle = AppRes.event_;

        }

        public IMvxCommand SearchCommand { get; }

        public IMvxCommand AddEventsCommand { get; }
        public override void ViewAppearing()
        {
            RunSafe(GetEvents(authReply), true, "Loading Events");
        }


        public ObservableCollection<SEvent> Events
        {
            get => eventsomers;
            private set => SetField(ref eventsomers, value);
        }

        private Staketracker.Core.Models.Events.D selectedStakeholder;

        public Staketracker.Core.Models.Events.D SelectedEvent
        {
            get => selectedStakeholder;
            set
            {
                if (SetProperty(ref selectedStakeholder, value) && value != null)
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
        public ICommand AboutCommand { get; }

        public override void Prepare(AuthReply authReply) => this.authReply = authReply;




        public async Task Refresh()
        {
        }

        private async Task FetchData()
        {

        }

        private void OnSelectedEventChanged(Staketracker.Core.Models.Events.D _event)
        {

            navigationService.Navigate<SEventDetailViewModel, PresentationContext<AuthReply>>(
                new PresentationContext<AuthReply>(authReply, PresentationMode.Read, int.Parse(_event.PrimaryKey), _event.Name));

            SelectedEvent = null;
        }

        private void ChangeLayoutMode(LayoutMode? mode) =>
            CurrentLayoutMode = mode ?? (CurrentLayoutMode == LayoutMode.Linear ? LayoutMode.Grid : LayoutMode.Linear);

        private async Task DoSeach(string term)
        {
            ObservableCollection<SEvent> newEvents;

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
