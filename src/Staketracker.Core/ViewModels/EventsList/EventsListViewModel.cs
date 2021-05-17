using System.Collections.ObjectModel;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using Xamarin.Forms;

namespace Staketracker.Core.ViewModels.EventsList
{
    public class Events
    {
        public string Name { get; set; }
        public string Date { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
    }

    public class EventsListViewModel : BaseViewModel

    {
        private Events selectedEvents, selectedEventsDetail;
        private ObservableCollection<Events> eventsomers;
        private string headerTitle;

        private readonly IMvxNavigationService _navigationService;
        public IMvxCommand SearchCommand { get; }

        public EventsListViewModel(IMvxNavigationService navigationService)
        {
            this.headerTitle = "Events";

            _navigationService = navigationService;

            this.SearchCommand = new MvxAsyncCommand(OnSearch);
        }

        private async Task FetchData()
        {
            Events events = new Events();
            events.Name = "Sugan Event";
            events.Date = "5/6/2020 :12:30 PM";
            events.Type = "Type 1";
            events.Status = "In - Progress / On - Going";

            Events events1 = new Events();
            events1.Name = "CLonning Project";
            events1.Date = "7/6/2020 :12:30 PM";
            events1.Type = "Type 4";
            events1.Status = "In - Progress / On - Going";
            Events events2 = new Events();
            events2.Name = "Hope Event";
            events2.Date = "7/6/2020 :12:30 PM";
            events2.Type = "Type 2";
            events2.Status = "Completed";
            this.eventsomers = new ObservableCollection<Events>();
            eventsomers.Add(events);
            eventsomers.Add(events1);
            eventsomers.Add(events2);
        }

        public async override void Prepare()
        {
            base.Prepare();
            await this.FetchData();
        }

        public ObservableCollection<Events> Events
        {
            get => eventsomers;
            private set => SetField(ref eventsomers, value);
        }

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

        public string HeaderTitle
        {
            get => headerTitle;
            private set => SetProperty(ref headerTitle, value);
        }

        private async Task OnSearch()
        {
            if (Device.Idiom != TargetIdiom.Phone)
                return;

            await this._navigationService.Navigate<SearchResultsViewModel>();
        }
    }
}
