namespace Staketracker.Core.ViewModels.Communication
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using MvvmCross.Commands;
    using MvvmCross.Navigation;
    using MvvmCross.ViewModels;
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
                PopulateForm(authReply, primaryKey, communications);
            }
        }
        protected override void InitFromBundle(IMvxBundle parameters)
        {

        }

        protected override void ReloadFromBundle(IMvxBundle state)
        {

        }

        //protected override void Sa(IMvxBundle state)
        //{

        //}

        public override async Task Initialize()
        {
            PageTitle = "Communication";
            await base.Initialize();
            RunSafe(BuildUIControls(authReply, FormType.Communications), true, "Building Form Controls");
            UpdateTitle();
        }

        private async Task OnDelete()
        {
            var result = await ShowDeleteConfirmation();

            if (result)
            {
                DelRecReplyModel reply;
                DelRecReqModel delReqModel = new DelRecReqModel();
                delReqModel.KeyId = (int)primaryKey;
                delReqModel.ScreenId = (int)ScreenKeyIdEnum.Communication;
                jsonTextObj _jsonTextObj = new jsonTextObj(delReqModel);

                HttpResponseMessage respMsg = await ApiManager.DelRec(_jsonTextObj, authReply.d.sessionId);

                if (respMsg.IsSuccessStatusCode)
                {
                    var response = await respMsg.Content.ReadAsStringAsync();
                    reply = await Task.Run(() => JsonConvert.DeserializeObject<DelRecReplyModel>(response));
                    if (reply.d.Equals("Record deleted"))
                    {
                        await PageDialog.AlertAsync(AppRes.record_deleted_msg, AppRes.record_deleted, AppRes.ok);
                        NavigateToList();
                    }
                    else
                    {
                        //await PageDialog.AlertAsync(primaryKey.ToString() + reply.d, AppRes.record_not_deleted, AppRes.ok);
                        await PageDialog.AlertAsync(reply.d, AppRes.record_not_deleted, AppRes.ok);
                    }

                }
                else
                    await PageDialog.AlertAsync(AppRes.server_error_while_delete_msg, AppRes.api_response_error, AppRes.ok);
            }
        }

        private async Task NavigateToList()
        {
            await navigationService.ChangePresentation(
            new MvvmCross.Presenters.Hints.MvxPopPresentationHint(typeof(CommunicationListViewModel)));
            return;
        }

        internal async Task<bool> AddCommunication()
        {
            GetLinkedToData();

            jsonTextObj jsonTextObj = new jsonTextObj(pageFormValue);
            HttpResponseMessage communications = await ApiManager.AddCommunication(jsonTextObj, authReply.d.sessionId);
            return await Add(communications);
        }

        private async Task OnSave()
        {
            if (isFormValid())
            {
                GetFormData("Communication");
                if (await AddCommunication())
                    changeView();

            }
        }
    }
}
