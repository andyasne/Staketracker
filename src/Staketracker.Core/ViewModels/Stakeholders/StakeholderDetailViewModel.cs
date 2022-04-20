using MvvmCross.Commands;
using MvvmCross.Navigation;
using Newtonsoft.Json;
using Staketracker.Core.Helpers;
using Staketracker.Core.Models;
using Staketracker.Core.Models.ApiRequestBody;
using Staketracker.Core.Models.FieldsValue;
using Staketracker.Core.Models.FormAndDropDownField;
using Staketracker.Core.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Staketracker.Core.Models.AddEventsReply;
using Staketracker.Core.Models.EventsFormValue;
using Staketracker.Core.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms;
using PresentationMode = Staketracker.Core.Models.PresentationMode;
using Staketracker.Core.Models.Stakeholders;
using Staketracker.Core.ViewModels.Stakeholders;
using Staketracker.Core.Models.DelRec;
using Staketracker.Core.Res;

namespace Staketracker.Core.ViewModels.Stakeholder
{
    public class StakeholderDetailViewModel : BaseViewModel<PresentationContext<AuthReply>>
    {
        public StakeholderDetailViewModel(IMvxNavigationService navigationService)
        {
            this.navigationService = navigationService;
            DeleteCommand = new MvxAsyncCommand(OnDeleteSEvent);
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
                StakeholderDetailReq body = new StakeholderDetailReq()
                {
                    projectId = authReply.d.projectId,
                    userId = authReply.d.userId,
                    StakeholderPrimaryKey = primaryKey
                };
                jsonTextObj jto = new jsonTextObj(body);
                HttpResponseMessage responseMessage;
                if (authReply.attachment.ToString() == "Groups")
                    responseMessage = await ApiManager.GetGroupStakeholderDetails(jto, authReply.d.sessionId);
                else if (authReply.attachment.ToString() == "Individuals")
                    responseMessage = await ApiManager.GetIndividualStakeholderDetails(jto, authReply.d.sessionId);
                else
                    responseMessage = await ApiManager.GetLandParcelStakeholderDetails(jto, authReply.d.sessionId);

                PopulateControlsWithData(authReply, primaryKey, responseMessage);

            }

        }
        public override async Task Initialize()
        {
            PageTitle = "Stakeholder";
            await base.Initialize();
            SelectedIndex = 1;
            if (authReply.attachment.ToString() == "Groups")
                RunSafe(GetFormUIControls(authReply, FormType.GroupedStakeholders), true, "Building Group Form Controls");
            else if (authReply.attachment.ToString() == "Individuals")
                RunSafe(GetFormUIControls(authReply, FormType.IndividualStakeholders), true, "Building Individual Form Controls");
            else
                RunSafe(GetFormUIControls(authReply, FormType.LandParcelStakeholders), true, "Building Land Parcel Form Controls");

            UpdateTitle();
        }
        private async Task OnDeleteSEvent()
        {
            var result = await ShowDeleteConfirmation();

            if (result)
            {
                DelRecReqModel delReqModel = new DelRecReqModel() { KeyId = primaryKey };

                if (authReply.attachment.ToString() == "Groups")
                    delReqModel.ScreenId = (int)ScreenKeyIdEnum.Stakeholder_Group;
                else if (authReply.attachment.ToString() == "Individuals")
                    delReqModel.ScreenId = (int)ScreenKeyIdEnum.Stakeholder_Individual;
                else
                    delReqModel.ScreenId = (int)ScreenKeyIdEnum.Stakeholder_LandParcel;


                jsonTextObj _jsonTextObj = new jsonTextObj(delReqModel);

                HttpResponseMessage respMsg = await ApiManager.DelRec(_jsonTextObj, authReply.d.sessionId);
                DelRecReplyModel reply;
                if (respMsg.IsSuccessStatusCode)
                {
                    var response = await respMsg.Content.ReadAsStringAsync();
                    reply = await Task.Run(() => JsonConvert.DeserializeObject<DelRecReplyModel>(response));

                    if (reply.d == "Record deleted")
                    {
                        await PageDialog.AlertAsync(AppRes.record_deleted_msg, AppRes.record_deleted, AppRes.ok);
                        NavigateToList();
                    }
                    else
                    {
                        await PageDialog.AlertAsync(AppRes.record_not_deleted_msg, AppRes.record_not_deleted, AppRes.ok);
                    }

                }
                else
                    await PageDialog.AlertAsync(AppRes.server_error_while_delete_msg, AppRes.api_response_error, AppRes.ok);
            }

        }
        private async Task NavigateToList()
        {
            await navigationService.ChangePresentation(
            new MvvmCross.Presenters.Hints.MvxPopPresentationHint(typeof(StakeholderListViewModel)));
            return;
        }
        internal async Task<bool> AddStaketracker()
        {
            jsonTextObj jsonTextObj = new jsonTextObj(pageFormValue);
            HttpResponseMessage resp;
            resp = await ApiManager.AddStakeholder(jsonTextObj, authReply.d.sessionId);
           return  await Add(resp);
        }
        private async Task OnSave()
        {
            if (isFormValid())
            {
                if (authReply.attachment.ToString() == "Groups")
                    FetchValuesFromFormControls("group");
                else if (authReply.attachment.ToString() == "Individuals")
                    FetchValuesFromFormControls("individual");
                else
                    FetchValuesFromFormControls("landparcel");
              if(await  AddStaketracker())
                changeView();

            }


        }
        private int selectedIndex;
        public int SelectedIndex
        {
            get => selectedIndex;
            set
            {
                if (selectedIndex != value)
                {
                    SetField(ref selectedIndex, value);
                    selectedIndex = value;
                }
            }
        }

    }
}
