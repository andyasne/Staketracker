namespace Staketracker.Core.ViewModels.Communication
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using MvvmCross.Commands;
    using MvvmCross.Navigation;
    using Newtonsoft.Json;
    using Staketracker.Core.Helpers;
    using Staketracker.Core.Models;
    using Staketracker.Core.Models.AddEventsReply;
    using Staketracker.Core.Models.Communication;
    using Staketracker.Core.Models.DelRec;
    using Staketracker.Core.Models.EventsFormValue;
    using Staketracker.Core.Res;
    using Staketracker.Core.ViewModels.CommunicationList;
    using PresentationMode = Staketracker.Core.Models.PresentationMode;

    public class CommunicationDetailViewModel : BaseViewModel<PresentationContext<AuthReply>>
    {
        public CommunicationDetailViewModel(IMvxNavigationService navigationService)
        {
            this.navigationService = navigationService;
            DeleteCommand = new MvxAsyncCommand(OnDelete);
            SaveCommand = new MvxAsyncCommand(OnSave);
            BeginEditCommand = new MvxAsyncCommand(OnBeginEdit);
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
                CommunicationDetailReq body = new CommunicationDetailReq()
                {
                    projectId = authReply.d.projectId,
                    userId = authReply.d.userId,
                    ID = primaryKey
                };

                HttpResponseMessage communications = await ApiManager.GetCommunicationDetails(new jsonTextObj(body), authReply.d.sessionId);
                PopulateControlsWithData(authReply, primaryKey, communications);
            }
        }

        public override async Task Initialize()
        {
            PageTitle = "Communication";
            await base.Initialize();
            RunSafe(GetFormUIControls(authReply, FormType.Communications), true, "Building Form Controls");
            UpdateTitle();
        }

        private async Task OnDelete()
        {
            var result = await ShowDeleteConfirmation();
            if (result)
            {
                jsonTextObj jsonTextObj = new jsonTextObj(new DelRecReqModel() { KeyId = (int)ScreenKeyIdEnum.Communication, ScreenId = primaryKey });
                HttpResponseMessage events = await ApiManager.DelRec(jsonTextObj, authReply.d.sessionId);

                DelRecReplyModel reply;

                if (events.IsSuccessStatusCode)
                {
                    var response = await events.Content.ReadAsStringAsync();
                    reply = await Task.Run(() => JsonConvert.DeserializeObject<DelRecReplyModel>(response));

                    if (reply.d == "Record deleted")
                    {

                        await PageDialog.AlertAsync("Record deleted", AppRes.saved, AppRes.ok);

                        NavigateToList();
                    }
                    else
                    {
                        await PageDialog.AlertAsync(reply.d, AppRes.error_saving, AppRes.ok);

                    }

                }
                else
                    await PageDialog.AlertAsync(AppRes.msg_error_while_saving, AppRes.api_response_error, AppRes.ok);

            }
        }

        private async Task NavigateToList()
        {
            await navigationService.ChangePresentation(
            new MvvmCross.Presenters.Hints.MvxPopPresentationHint(typeof(CommunicationListViewModel)));
            return;
        }

        internal async Task AddCommunication()
        {
            jsonTextObj jsonTextObj = new jsonTextObj(pageFormValue);
            HttpResponseMessage communications = await ApiManager.AddCommunication(jsonTextObj, authReply.d.sessionId);
            await Add(communications);
        }

        private async Task OnSave()
        {
            if (isFormValid())
            {
                FetchValuesFromFormControls("Communication");
                AddCommunication();
                changeView();

            }
        }
    }
}
