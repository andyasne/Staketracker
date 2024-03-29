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
using Staketracker.Core.Models.ProjectTeam;
using D = Staketracker.Core.Models.Events.D;
using PresentationMode = Staketracker.Core.Models.PresentationMode;
using Staketracker.Core.Models.Stakeholders;
using Staketracker.Core.Models.EventsFormValue;
using Staketracker.Core.Models.Issues;

namespace Staketracker.Core.ViewModels.Issues
{

    public class IssuesListViewModel : BaseViewModel<AuthReply>
    {

        private readonly IMvxNavigationService navigationService;

        internal AuthReply authReply;


        public IssuesListViewModel(IMvxNavigationService navigationService)
        {
            this.navigationService = navigationService;

        }


        public override void Prepare(AuthReply authReply)
        {
            base.Prepare();
            this.authReply = authReply;
            RunSafe(GetIssuesList(authReply), true, "Loading Issues");

        }
        private Staketracker.Core.Models.Issues.D selectedIssues;

        public Staketracker.Core.Models.Issues.D SelectedIssues
        {
            get => selectedIssues;
            set
            {
                if (SetProperty(ref selectedIssues, value) && value != null)
                {

                    OnSelectedEventChanged(selectedIssues);
                }

            }
        }

        private void OnSelectedEventChanged(Staketracker.Core.Models.Issues.D Issues)
        {
            if (Device.Idiom != TargetIdiom.Phone)
                return;
            string IssuesSubject = "";
            if (Issues.Name != null)
            {
                IssuesSubject = Issues.Name.ToString();
            }
            navigationService.Navigate<IssuesDetailViewModel, PresentationContext<AuthReply>>(
                new PresentationContext<AuthReply>(authReply, PresentationMode.Read, int.Parse(Issues.PrimaryKey), IssuesSubject));

        }





    }
}
