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
using Staketracker.Core.ViewModels.Issues;
using Staketracker.Core.Res;
using Staketracker.Core.Models.Issues;
using System.Linq;
using System.Collections.Generic;
using Staketracker.Core.Validators;

namespace Staketracker.Core.ViewModels.Issues
{
    public class IssuesDetailViewModel : BaseViewModel<PresentationContext<AuthReply>>
    {

        public IssuesDetailViewModel(IMvxNavigationService navigationService)
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

                HttpResponseMessage Issuess = await ApiManager.GetIssueDetails(new jsonTextObj(body), authReply.d.sessionId);
                PopulateControlsWithData(authReply, primaryKey, Issuess);
            }
        }

        public override async Task Initialize()
        {
            PageTitle = AppRes.issues;
            await base.Initialize();
            RunSafe(GetFormUIControls(authReply, FormType.Issuess), true, "Building Form Controls");
            UpdateTitle();
        }



        private async Task NavigateToList()
        {
            await navigationService.ChangePresentation(
            new MvvmCross.Presenters.Hints.MvxPopPresentationHint(typeof(IssuesListViewModel)));
            return;
        }




    }
}
