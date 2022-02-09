namespace Staketracker.Core.ViewModels.SwitchProject
{
    using MvvmCross.Navigation;
    using MvvmCross.ViewModels;
    using Newtonsoft.Json;
    using Plugin.Settings;
    using Staketracker.Core.Models;
    using Staketracker.Core.Models.EventsFormValue;
    using Staketracker.Core.Models.RequestSwitchProject;
    using Staketracker.Core.Models.ResponseProjectList;
    using Staketracker.Core.Validators;
    using Staketracker.Core.Validators.Rules;
    using Staketracker.Core.ViewModels.Login;
    using Staketracker.Core.ViewModels.Root;
    using Staketracker.Core.ViewModels.TwoStepVerification;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Xamarin.Essentials;
    using Xamarin.Forms;

    public class ComboListModel
    {
        public string Name { get; set; }
    }
    public class SwitchProjectViewModel : BaseViewModel<AuthReply>
    {
        private string email;
        private AuthReply authReply;
        private ResponseProjectList ResponseProjectList;
        internal readonly IMvxNavigationService _navigationService;


        private ObservableCollection<BusinessUnit> _BusinessUnit;
        public ObservableCollection<BusinessUnit> BusinessUnit
        {
            get => _BusinessUnit;
            set => SetField(ref _BusinessUnit, value);

        }
        public ObservableCollection<Project> project;
        public ObservableCollection<Project> Project
        {
            get => project;
            set => SetField(ref project, value);

        }


        public Project SelectedProject { get; set; }
        public BusinessUnit selectedBusinessUnit;

        public BusinessUnit SelectedBusinessUnit
        {
            get
            {
                return this.selectedBusinessUnit;
            }
            set
            {
                if (this.selectedBusinessUnit != value)
                {
                    this.selectedBusinessUnit = value;

                    Project = new ObservableCollection<Project>();
                    foreach (Project proj in value.projects)
                    {
                        Project.Add(proj);
                    }

                }
            }
        }
        public String DomainSelected { get; set; }

        public ICommand OpenProjectCommand { get; set; }

        public async override void Prepare(AuthReply parameter)
        {
            authReply = parameter;
            GetProjectList(authReply);

        }



        public SwitchProjectViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;

            OpenProjectCommand = new Command(OpenProject);


        }

        private async void OpenProject()
        {
            DomainSelected = SelectedProject.name;
            CrossSettings.Current.AddOrUpdateValue("ProjectName", DomainSelected);
            SetField(ref domainSelected, DomainSelected);
            SwitchProject(authReply);

        }


        internal async Task GetProjectList(AuthReply authReply)
        {

            RequestProjectList body = new RequestProjectList();
            body.userId = authReply.d.userId;

            var apiReq = new jsonTextObj(body);
            HttpResponseMessage projList = await ApiManager.GetProjectList(apiReq, authReply.d.sessionId);

            if (projList.IsSuccessStatusCode)
            {
                var response = await projList.Content.ReadAsStringAsync();
                ResponseProjectList = await Task.Run(() => JsonConvert.DeserializeObject<ResponseProjectList>(response));
                this.BusinessUnit = new ObservableCollection<BusinessUnit>();
                foreach (var bunit in ResponseProjectList.d.businessUnits)
                {
                    this.BusinessUnit.Add(bunit);
                }

            }
            else
                await PageDialog.AlertAsync("API Error While Getting Project List", "API Response Error", "Ok");

        }


        internal async Task SwitchProject(AuthReply authReply)
        {
            if (SelectedProject == null)
                return;

            RequestSwitchProject body = new RequestSwitchProject();
            body.UserId = authReply.d.userId;
            body.ProjectId = SelectedProject.projectId;
            var apiReq = new jsonTextObj(body);
            HttpResponseMessage respMsg = await ApiManager.SwitchProject(apiReq, authReply.d.sessionId);

            if (respMsg.IsSuccessStatusCode)
            {
                var response = await respMsg.Content.ReadAsStringAsync();
                ResponseSwitchProject responseSwitchProject = await Task.Run(() => JsonConvert.DeserializeObject<ResponseSwitchProject>(response));

                if (responseSwitchProject.d.Status == "OK")
                {
                    await PageDialog.AlertAsync("Changed Project to " + SelectedProject.name, "Project Changed", "Ok");

                    _navigationService.Navigate(new LoginViewModel(navigationService));

                }
                else
                {

                    await PageDialog.AlertAsync("API Error While Trying to Change Project", "API Response Error", "Ok");

                }


            }
            else
                await PageDialog.AlertAsync("API Error While Trying to Change Project", "API Response Error", "Ok");

        }


    }
}
