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
            List<string> selectedComm = new List<string>();

            foreach (object communicationObject in SelectedCommunications)
            {
                Staketracker.Core.Models.Communication.D communication = (Staketracker.Core.Models.Communication.D)communicationObject;

                selectedComm.Add(communication.PrimaryKey);
            }

            authReply.SelectedCommunications = "[" + string.Join(",", selectedComm) + "]";


            _navigationService.ChangePresentation(new MvxPopPresentationHint(typeof(CommunicationDetailViewModel), true));
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

            switch (linkedObj.buttonLabel)
            {
                case "Stakeholders":
                    RunSafe(GetCommunication(authReply), true, "Loading " + linkedObj.buttonLabel);
                    CommunicationVisible = true;
                    break;

                case "Topics":
                    RunSafe(GetEvents(authReply), true, "Loading " + linkedObj.buttonLabel);
                    EventVisible = true;
                    break;

                case "Project Team":
                    ProjectTeam.ProjectTeamListViewModel projectTeamListViewModel = new ProjectTeamListViewModel(navigationService);
                    RunSafe(projectTeamListViewModel.GetProjectList(authReply), true, "Loading " + linkedObj.buttonLabel);
                    break;

                case "Topichs":
                    Issues.IssuesListViewModel issuesListViewModel = new IssuesListViewModel(navigationService);
                    RunSafe(issuesListViewModel.GetProjectList(authReply), true, "Loading " + linkedObj.buttonLabel);
                    break;

                case "Individuals":
                    StakeholderListViewModel stakeholderListViewModel = new StakeholderListViewModel(navigationService);
                    RunSafe(stakeholderListViewModel.GetLandParcelStakeholderDetails(authReply), true, "Loading " + linkedObj.buttonLabel);
                    break;

                //case "Groups":
                //    CommunicationListViewModel communicationListViewModel = new CommunicationListViewModel(navigationService);
                //    RunSafe(communicationListViewModel.GetCommunication(authReply), true, "Loading " + linkedObj.buttonLabel);
                //    break;

                //case "Land Parcels":
                //    CommunicationListViewModel communicationListViewModel = new CommunicationListViewModel(navigationService);
                //    RunSafe(communicationListViewModel.GetCommunication(authReply), true, "Loading " + linkedObj.buttonLabel);
                //    break;


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


        public bool CommunicationVisible { get; set; }
        private bool _EventVisible;


    }
}
