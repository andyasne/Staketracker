using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.Plugin.Messenger;
using MvvmCross.ViewModels;
using Newtonsoft.Json;
using Staketracker.Core.Models;
using Staketracker.Core.Models.ApiRequestBody;
using Staketracker.Core.Models.Events;
using Staketracker.Core.Res;
using Xamarin.Forms;
using System.Linq;

using D = Staketracker.Core.Models.Events.D;
using PresentationMode = Staketracker.Core.Models.PresentationMode;
using Staketracker.Core.Models.Stakeholders;
using Staketracker.Core.Models.EventsFormValue;
using Staketracker.Core.Models.ProjectTeam;

namespace Staketracker.Core.ViewModels.ProjectTeam
{

    public class ProjectTeamListViewModel : BaseViewModel<AuthReply>
    {

        private readonly IMvxNavigationService navigationService;

        internal AuthReply authReply;


        public ProjectTeamListViewModel(IMvxNavigationService navigationService)
        {
            this.navigationService = navigationService;

        }


        public override void Prepare(AuthReply authReply)
        {
            base.Prepare();
            this.authReply = authReply;
            RunSafe(GetProjectList(authReply), true, "Loading Project Team");

        }
        private Staketracker.Core.Models.ProjectTeam.D selectedProjectTeam;

        public Staketracker.Core.Models.ProjectTeam.D SelectedProjectTeam
        {
            get => selectedProjectTeam;
            set
            {
                if (SetProperty(ref selectedProjectTeam, value) && value != null)
                {

                    OnSelectedEventChanged(selectedProjectTeam);
                }

            }
        }

        private void OnSelectedEventChanged(Staketracker.Core.Models.ProjectTeam.D ProjectTeam)
        {
            if (Device.Idiom != TargetIdiom.Phone)
                return;
            string ProjectTeamSubject = "";
            if (ProjectTeam.Name != null)
            {
                ProjectTeamSubject = ProjectTeam.Name.ToString();
            }
            navigationService.Navigate<ProjectTeamDetailViewModel, PresentationContext<AuthReply>>(
                new PresentationContext<AuthReply>(authReply, PresentationMode.Read, int.Parse(ProjectTeam.PrimaryKey), ProjectTeamSubject));

        }

        private ProjectTeamReply _projectTeamList;
        public ProjectTeamReply projectTeamList
        {
            get => _projectTeamList;
            set => SetField(ref _projectTeamList, value);
        }
        internal async Task GetProjectList(AuthReply authReply)
        {

            StakeholderBody body = new StakeholderBody();
            body.projectId = authReply.d.projectId;
            body.userId = authReply.d.userId;

            var apiReq = new jsonTextObj(body);
            HttpResponseMessage projectTeamListRespMessage = await ApiManager.GetProjectTeam(apiReq, authReply.d.sessionId);

            if (projectTeamListRespMessage.IsSuccessStatusCode)
            {
                var response = await projectTeamListRespMessage.Content.ReadAsStringAsync();
                ProjectTeamReply _projectTeamList = await Task.Run(() => JsonConvert.DeserializeObject<ProjectTeamReply>(response));
                _projectTeamList.d.Sort((x, y) => { return string.Compare(x.FullName, y.FullName); });
                projectTeamList = _projectTeamList;
            }
            else
                await PageDialog.AlertAsync("API Error While retrieving", "API Response Error", "Ok");

        }


    }
}
