using MvvmCross.Commands;
using MvvmCross.Navigation;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Staketracker.Core.ViewModels.Tasks
{
    public class Tasks
    {
        public string Name { get; set; }
        public string Date { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
    }

    public class TasksListViewModel : BaseViewModel

    {
        private Tasks selectedTasks, selectedTasksDetail;
        private ObservableCollection<Tasks> Tasks_;

        private readonly IMvxNavigationService _navigationService;
        public IMvxCommand SearchCommand { get; }

        public TasksListViewModel(IMvxNavigationService navigationService)
        {
            this.HeaderTitle = "Tasks";

            _navigationService = navigationService;

            this.SearchCommand = new MvxAsyncCommand(OnSearch);
        }

        private async Task FetchData()
        {
            Tasks events = new Tasks();
            events.Name = "Sugan Event";
            events.Date = "5/6/2020 :12:30 PM";
            events.Type = "Type 1";
            events.Status = "In - Progress / On - Going";

            Tasks events1 = new Tasks();
            events1.Name = "CLonning Project";
            events1.Date = "7/6/2020 :12:30 PM";
            events1.Type = "Type 4";
            events1.Status = "In - Progress / On - Going";
            Tasks events2 = new Tasks();
            events2.Name = "Hope Event";
            events2.Date = "7/6/2020 :12:30 PM";
            events2.Type = "Type 2";
            events2.Status = "Completed";
            this.Tasks_ = new ObservableCollection<Tasks>();
            Tasks_.Add(events);
            Tasks_.Add(events1);
            Tasks_.Add(events2);
        }

        public async override void Prepare()
        {
            base.Prepare();
            await this.FetchData();
        }

        public ObservableCollection<Tasks> Tasks
        {
            get => Tasks_;
            private set => SetField(ref Tasks_, value);
        }

        public Tasks SelectedTasks
        {
            get => selectedTasks;
            set
            {
                if (SetProperty(ref selectedTasks, value) && value != null)
                {
                    SetField(ref selectedTasks, value);
                }
            }
        }

        public async Task Refresh()
        {
        }


        private async Task OnSearch()
        {
            if (Device.Idiom != TargetIdiom.Phone)
                return;

            await this._navigationService.Navigate<SearchResultsViewModel>();
        }
    }
}
