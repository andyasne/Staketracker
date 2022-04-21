using MvvmCross.Navigation;
using System.Net.Http;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using Staketracker.Core.Helpers;
using Staketracker.Core.Models;
using Staketracker.Core.Models.Communication;
using Staketracker.Core.Models.EventsFormValue;
using Staketracker.Core.ViewModels.CommunicationList;
using PresentationMode = Staketracker.Core.Models.PresentationMode;
using Staketracker.Core.ViewModels;
using Staketracker.Core.ViewModels.ProjectTeam;
using Staketracker.Core.Res;
using Staketracker.Core.Models.ProjectTeamDetail;
using Staketracker.Core.Models.Issues;

namespace Staketracker.Core.ViewModels.ProjectTeam
{
    public class ProjectTeamDetailViewModel : BaseViewModel<PresentationContext<AuthReply>>
    {

        public ProjectTeamDetailViewModel(IMvxNavigationService navigationService)
        {
            this.navigationService = navigationService;

        }

        public override void Prepare(PresentationContext<AuthReply> parameter)
        {
            authReply = parameter.Model;
            Mode = parameter.Mode;
            primaryKey = parameter.PrimaryKey;
            name = parameter.Name;
        }

        public override async void ViewAppearing()
        {
            if (mode == PresentationMode.Edit || mode == PresentationMode.Read)
            {
                IssuesDetailReq body = new IssuesDetailReq()
                {
                    projectId = authReply.d.projectId,
                    userId = authReply.d.userId,
                    PrimaryKey = primaryKey.ToString()
                };

                HttpResponseMessage ProjectTeams = await ApiManager.GetProjectTeamMemberDetails(new jsonTextObj(body), authReply.d.sessionId);
                PopulateForm(authReply, primaryKey, ProjectTeams);
            }
        }

        public override async Task Initialize()
        {
            PageTitle = AppRes.project_team;
            await base.Initialize();
            RunSafe(BuildUIControls(authReply, FormType.TeamMembers), true, "Building Form Controls");
            UpdateTitle();
        }

        private async Task NavigateToList()
        {
            await navigationService.ChangePresentation(
            new MvvmCross.Presenters.Hints.MvxPopPresentationHint(typeof(ProjectTeamListViewModel)));
            return;
        }




    }
}
