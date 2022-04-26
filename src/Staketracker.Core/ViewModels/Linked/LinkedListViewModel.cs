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
using System;
using Staketracker.Core.Models.LinkedTo;
using Staketracker.Core.ViewModels.CommunicationList;
using Staketracker.Core.ViewModels.ProjectTeam;
using Staketracker.Core.ViewModels.Issues;
using Staketracker.Core.ViewModels.Stakeholders;
using MvvmCross.Presenters.Hints;
using Staketracker.Core.ViewModels.Root;
using MvvmCross.ViewModels;
using Staketracker.Core.ViewModels.Stakeholder;

namespace Staketracker.Core.ViewModels.Linked.Communication
{

    public class Communication
    {
        public string Name { get; set; }
        public string Date { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
    }

    public class LinkedListViewModel : BaseViewModel<AuthReply>

    {
        public IMvxCommand AddCommunicationCommand { get; }

        private string linkName;

        public string LinkName
        {

            get { return linkName; }
            set
            {
                SetField(ref linkName, value);

                linkName = value;

            }
        }

        private readonly IMvxNavigationService _navigationService;
        public IMvxCommand SearchCommand { get; }
        Staketracker.Core.Models.LinkedTo.LinkedTo linkedObj;
        public IMvxCommand NavigateBackCommand { get; }
        public bool CommunicationVisible { get; set; }
        public bool ProjectTeamsVisible { get; set; }
        public bool TopicsVisible { get; set; }
        public bool StakeholdersVisible { get; set; }



        private bool _EventVisible;
        public LinkedListViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;

            AddCommunicationCommand = new MvxCommand(OnCreateCommunication);
            NavigateBackCommand = new MvxCommand(NavigateBack);
        }

        private void OnCreateCommunication() =>
            _navigationService.Navigate<CommunicationDetailViewModel, PresentationContext<AuthReply>>(
                new PresentationContext<AuthReply>(authReply, PresentationMode.Create));

        private void NavigateBack()
        {
            List<Staketracker.Core.Models.Communication.Team> selectedComm = new List<Staketracker.Core.Models.Communication.Team>();

            foreach (object communicationObject in SelectedCommunications)
            {
                Staketracker.Core.Models.Communication.D communication = (Staketracker.Core.Models.Communication.D)communicationObject;
                Staketracker.Core.Models.Communication.Team team = new Team();
                team.PrimaryKey = communication.PrimaryKey;
                selectedComm.Add(team);
            }

            ////  authReply.CommunicationReply.d.Team = selectedComm;
            //  authReply.SelectedCommunications = "[" + string.Join("{\"PrimaryKey\":\"", selectedComm) + "]";

            switch (authReply.fromPage)
            {
                case "Events":
                    _navigationService.ChangePresentation(new MvxPopPresentationHint(typeof(SEventDetailViewModel), true));
                    break;

                case "Communication":
                    _navigationService.ChangePresentation(new MvxPopPresentationHint(typeof(CommunicationDetailViewModel), true));
                    break;

                case "Stakeholder":
                    _navigationService.ChangePresentation(new MvxPopPresentationHint(typeof(StakeholderDetailViewModel), true));
                    break;

            }
        }
        private Type senderType;

        public override async void Prepare(AuthReply parameter)
        {

            this.authReply = parameter;
            KeyValuePair<String, LinkedTo> _linkedTo = (KeyValuePair<String, LinkedTo>)authReply.attachment;
            linkedObj = _linkedTo.Value;
            PopulateAsync();

            base.Prepare();
        }

        private async Task PopulateAsync()
        {
            CommunicationVisible = false;
            EventVisible = false;
            ProjectTeamsVisible = false;
            TopicsVisible = false;
            StakeholdersVisible = false;
            switch (linkedObj.buttonLabel)
            {
                case "Communications":
                case "Communication":
                    RunSafe(GetCommunication(authReply), true, "Loading " + linkedObj.buttonLabel);
                    CommunicationVisible = true;
                    break;

                case "Events":
                case "Event":
                    EventVisible = true;
                    RunSafe(GetEvents(authReply), true, "Loading " + linkedObj.buttonLabel);
                    break;

                case "Team members":
                case "Project Team":

                    ProjectTeamsVisible = true;
                    RunSafe(GetProjectList(authReply), true, "Loading " + linkedObj.buttonLabel);
                    break;

                case "Topics":
                    TopicsVisible = true;
                    RunSafe(GetIssuesList(authReply), true, "Loading " + linkedObj.buttonLabel);
                    break;

                case "Stakeholders":
                case "Groups":
                case "Land parcels":
                case "Individuals":

                    StakeholdersVisible = true;
                    RunSafe(GetLandParcelStakeholderDetails(authReply), true, "Loading " + linkedObj.buttonLabel);
                    break;



                default:
                    break;
            }
        }

        public override async void ViewAppearing()
        {
            base.ViewAppearing();
            this.PageTitle = linkedObj.buttonLabel;
            this.LinkName = "Link to " + this.PageTitle;

            //Populate();
        }


        private bool isSearchEmpty, isBusy;
        public bool IsBusy
        {
            get => isBusy;
            set => SetProperty(ref isBusy, value);
        }

        public bool EventVisible
        {
            get => _EventVisible;
            set => SetProperty(ref _EventVisible, value);
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

                    // OnSelectedEventChanged(selectedCommunication);
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





    }
}
