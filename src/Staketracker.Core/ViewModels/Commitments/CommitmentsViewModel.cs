using MvvmCross.Commands;
using MvvmCross.Navigation;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Staketracker.Core.ViewModels.Commitments
{
    public class Commitments
    {
        public string Name { get; set; }
        public string Date { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
    }

    public class CommitmentsViewModel : BaseViewModel

    {
        private Commitments selectedCommitments, selectedCommitmentsDetail;
        private ObservableCollection<Commitments> Commitments_;
        private string headerTitle;

        private readonly IMvxNavigationService _navigationService;
        public IMvxCommand SearchCommand { get; }

        public CommitmentsViewModel(IMvxNavigationService navigationService)
        {
            this.headerTitle = "Commitments";

            _navigationService = navigationService;

            this.SearchCommand = new MvxAsyncCommand(OnSearch);
        }

        private async Task FetchData()
        {
            Commitments events = new Commitments();
            events.Name = "Sugan Event";
            events.Date = "5/6/2020 :12:30 PM";
            events.Type = "Type 1";
            events.Status = "In - Progress / On - Going";

            Commitments events1 = new Commitments();
            events1.Name = "CLonning Project";
            events1.Date = "7/6/2020 :12:30 PM";
            events1.Type = "Type 4";
            events1.Status = "In - Progress / On - Going";
            Commitments events2 = new Commitments();
            events2.Name = "Hope Event";
            events2.Date = "7/6/2020 :12:30 PM";
            events2.Type = "Type 2";
            events2.Status = "Completed";
            this.Commitments_ = new ObservableCollection<Commitments>();
            Commitments_.Add(events);
            Commitments_.Add(events1);
            Commitments_.Add(events2);
        }

        public async override void Prepare()
        {
            base.Prepare();
            await this.FetchData();
        }

        public ObservableCollection<Commitments> Commitments
        {
            get => Commitments_;
            private set => SetField(ref Commitments_, value);
        }

        public Commitments SelectedCommitments
        {
            get => selectedCommitments;
            set
            {
                if (SetProperty(ref selectedCommitments, value) && value != null)
                {
                    SetField(ref selectedCommitments, value);
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
