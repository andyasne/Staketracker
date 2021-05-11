using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using Staketracker.Core.ViewModels.Login;
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
        private string listDescription;

        private readonly IMvxNavigationService _navigationService;
        public IMvxCommand SearchCommand { get; }

        public EventsListViewModel(IMvxNavigationService navigationService)
        {

            this.listDescription = "Events";

            //this.eventsomers = new ObservableCollection<Events> { new Events("Tom"), new Events("Anna"), new Events("Peter"), new Events("Teodor"), new Events("Lorenzo"), new Events("Andrea"), new Events("Martin") };

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
        public string ListDescription
        {
            get => listDescription;
            private set => SetProperty(ref listDescription, value);
        }
        private async Task OnSearch()
        {
            if (Device.Idiom != TargetIdiom.Phone)
                return;

            await this._navigationService.Navigate<SearchResultsViewModel>();
        }

    }
}
