using System.Collections.Generic;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using Staketracker.Core.Helpers;
using Staketracker.Core.Models;
using System.ComponentModel;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Staketracker.Core.Models.ApiRequestBody;
using Staketracker.Core.Models.Communication;
using Staketracker.Core.Models.Events;
using Xamarin.Forms;
using System.Linq;
using Staketracker.Core.ViewModels.Communication;
using Staketracker.Core.ViewModels.Events;
using D = Staketracker.Core.Models.Events.D;
using PresentationMode = Staketracker.Core.Models.PresentationMode;
using Staketracker.Core.Res;

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
        public IMvxCommand AddCommunicationCommand { get; }

        private readonly IMvxNavigationService _navigationService;
        public IMvxCommand SearchCommand { get; }

        public CommunicationListViewModel(IMvxNavigationService navigationService)
        {
            this.PageTitle = AppRes.communication;

            _navigationService = navigationService;

            this.SearchCommand = new MvxAsyncCommand(OnSearch);
            AddCommunicationCommand = new MvxCommand(OnCreateCommunication);

        }

        private void OnCreateCommunication() =>
            _navigationService.Navigate<CommunicationDetailViewModel, PresentationContext<AuthReply>>(
                new PresentationContext<AuthReply>(authReply, PresentationMode.Create));


        public override void Prepare(AuthReply parameter)
        {
            base.Prepare();

            this.IsBusy = true;
            this.authReply = parameter;
            //this.Mode = parameter.Mode;

            this.IsBusy = false;

        }

        public override void ViewAppearing()
        {
            RunSafe(GetCommunication(authReply), true, "Loading Communication");
        }
        private bool isSearchEmpty, isBusy;
        public bool IsBusy
        {
            get => isBusy;
            set => SetProperty(ref isBusy, value);
        }

        public event PropertyChangedEventHandler PropertyChanged;


        private Staketracker.Core.Models.Communication.D selectedCommunication;

        public Staketracker.Core.Models.Communication.D SelectedCommunication
        {
            get => selectedCommunication;
            set
            {
                if (SetProperty(ref selectedCommunication, value) && value != null)
                {

                    OnSelectedEventChanged(selectedCommunication);
                }

            }
        }

        private void OnSelectedEventChanged(Staketracker.Core.Models.Communication.D communication)
        {
            if (Device.Idiom != TargetIdiom.Phone)
                return;
            string communicationSubject = "";
            if (communication.CommunicationSubject != null)
            {
                communicationSubject = communication.CommunicationSubject.ToString();
            }
            _navigationService.Navigate<CommunicationDetailViewModel, PresentationContext<AuthReply>>(
                new PresentationContext<AuthReply>(authReply, PresentationMode.Read, int.Parse(communication.PrimaryKey), communicationSubject));

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
