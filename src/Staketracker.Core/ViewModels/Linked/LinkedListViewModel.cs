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
            List<Team> selectedEvents = new List<Staketracker.Core.Models.Communication.Team>();
            List<Staketracker.Core.Models.Communication.Team> selectedComms = new List<Staketracker.Core.Models.Communication.Team>();
            List<Staketracker.Core.Models.Communication.Team> selectedIssues = new List<Staketracker.Core.Models.Communication.Team>();
            List<Staketracker.Core.Models.Communication.Team> selectedTeam = new List<Staketracker.Core.Models.Communication.Team>();
            List<LandParcelStakeholder> selectedStakeholder = new List<Staketracker.Core.Models.Communication.LandParcelStakeholder>();

            if (communicationReply_ != null)
                foreach (Staketracker.Core.Models.Communication.D dCommunicationObject in communicationReply_.d)
                {
                    if (dCommunicationObject.IsChecked)
                    {
                        Staketracker.Core.Models.Communication.Team team = new Team();
                        team.PrimaryKey = dCommunicationObject.PrimaryKey;
                        selectedComms.Add(team);
                    }
                }
            if (selectedComms.Any())
                authReply.Linked_SelectedCommunications = selectedComms;

            if (IssuesList != null)
                foreach (Models.Issues.D topicsObj in IssuesList.d)
                {
                    if (topicsObj.IsChecked)
                    {
                        Staketracker.Core.Models.Communication.Team key = new Team();
                        key.PrimaryKey = topicsObj.PrimaryKey;
                        selectedIssues.Add(key);
                    }
                }
            if (selectedIssues.Any())
                authReply.Linked_SelectedTopics = selectedIssues;

            if (projectTeamList != null)
                foreach (Staketracker.Core.Models.ProjectTeam.D obj in projectTeamList.d)
                {
                    if (obj.IsChecked)
                    {
                        Staketracker.Core.Models.Communication.Team key = new Team();
                        key.PrimaryKey = obj.PrimaryKey;
                        selectedTeam.Add(key);
                    }
                }
            if (selectedTeam.Any())
                authReply.Linked_SelectedTeam = selectedTeam;

            if (EventsReply_ != null)
                foreach (Staketracker.Core.Models.Events.D obj in EventsReply_.d)
                {
                    if (obj.IsChecked)
                    {
                        Staketracker.Core.Models.Communication.Team key = new Team();
                        key.PrimaryKey = obj.PrimaryKey;
                        selectedEvents.Add(key);
                    }
                }
            if (selectedEvents.Any())
                authReply.Linked_SelectedEvents = selectedEvents;

            if (allStakeholders != null)
                foreach (Staketracker.Core.Models.Stakeholders.LandParcelStakeholder obj in allStakeholders.d.LandParcelStakeholders)
                {
                    if (obj.IsChecked)
                    {
                        Staketracker.Core.Models.Communication.LandParcelStakeholder key = new LandParcelStakeholder();
                        key.StakeHolderKey = obj.PrimaryKey;
                        selectedStakeholder.Add(key);
                    }
                }
            if (selectedStakeholder.Any())
                authReply.Linked_SelectedStakeholder = (selectedStakeholder);


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

                case "Issue":
                    _navigationService.ChangePresentation(new MvxPopPresentationHint(typeof(IssuesDetailViewModel), true));
                    break;

                case "Project Teams":
                    _navigationService.ChangePresentation(new MvxPopPresentationHint(typeof(ProjectTeamDetailViewModel), true));
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
                    CommunicationVisible = true;
                    RunSafe(GetCommunication(authReply), true, "Loading " + linkedObj.buttonLabel).

                       ContinueWith((a) =>
                       {

                           foreach (Team selected in authReply.Linked_SelectedCommunications)
                           {
                               communicationReply_.d.Where(a => a.PrimaryKey == selected.PrimaryKey).FirstOrDefault().IsChecked = true;
                           }
                           OnPropertyChanged("communicationReply_");



                       }
                       );

                    break;

                case "Events":
                case "Event":
                    EventVisible = true;
                    RunSafe(GetEvents(authReply), true, "Loading " + linkedObj.buttonLabel).

                       ContinueWith((a) =>
                       {
                           foreach (Team selected in authReply.Linked_SelectedEvents)
                           {
                               EventsReply_.d.Where(a => a.PrimaryKey == selected.PrimaryKey).FirstOrDefault().IsChecked = true;
                           }
                           OnPropertyChanged("EventsReply_");

                       }
                       );
                    break;

                case "Team members":
                case "Project Team":

                    ProjectTeamsVisible = true;
                    RunSafe(GetProjectList(authReply), true, "Loading " + linkedObj.buttonLabel).
                       ContinueWith((a) =>
                       {
                           foreach (Team selected in authReply.Linked_SelectedTeam)
                           {
                               projectTeamList.d.Where(a => a.PrimaryKey == selected.PrimaryKey).FirstOrDefault().IsChecked = true;
                           }
                           OnPropertyChanged("projectTeamList");

                       }
                       );
                    break;

                case "Topics":
                    TopicsVisible = true;
                    RunSafe(GetIssuesList(authReply), true, "Loading " + linkedObj.buttonLabel).
                       ContinueWith((a) =>
                       {
                           foreach (Team selected in authReply.Linked_SelectedTopics)
                           {
                               IssuesList.d.Where(a => a.PrimaryKey == selected.PrimaryKey).FirstOrDefault().IsChecked = true;
                           }
                           OnPropertyChanged("IssuesList");

                       }
                       );
                    break;

                case "Stakeholders":
                case "Groups":
                case "Land parcels":
                case "Individuals":

                    StakeholdersVisible = true;
                    RunSafe(GetLandParcelStakeholderDetails(authReply), true, "Loading " + linkedObj.buttonLabel).
                       ContinueWith((a) =>
                       {
                           foreach (Staketracker.Core.Models.Communication.LandParcelStakeholder selected in authReply.Linked_SelectedStakeholder)
                           {
                               allStakeholders.d.LandParcelStakeholders.Where(a => a.PrimaryKey == selected.StakeHolderKey).FirstOrDefault().IsChecked = true;
                           }
                           OnPropertyChanged("IssuesList");

                       }
                       );
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
