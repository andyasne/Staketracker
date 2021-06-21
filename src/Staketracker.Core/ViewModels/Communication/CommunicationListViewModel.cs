using MvvmCross.Commands;
using MvvmCross.Navigation;
using Staketracker.Core.Helpers;
using Staketracker.Core.Models;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Staketracker.Core.ViewModels.CommunicationList
{


    public class Communication
    {
        public string Name { get; set; }
        public string Date { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
    }


    public class CommunicationListViewModel : BaseViewModel<AuthReply>

    {


        private readonly IMvxNavigationService _navigationService;
        public IMvxCommand SearchCommand { get; }

        public CommunicationListViewModel(IMvxNavigationService navigationService)
        {
            this.HeaderTitle = "Communication";

            _navigationService = navigationService;

            this.SearchCommand = new MvxAsyncCommand(OnSearch);
        }
        public AuthReply authReply;


        public override void Prepare(AuthReply parameter)
        {
            this.authReply = parameter;
            //this.Mode = parameter.Mode;
        }


        public event PropertyChangedEventHandler PropertyChanged;



        public async override Task Initialize()
        {
            await base.Initialize();

            GetFormandDropDownFields(authReply, FormType.Communications);


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
