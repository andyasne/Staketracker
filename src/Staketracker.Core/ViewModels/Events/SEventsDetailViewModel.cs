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
using Xamarin.Forms;
using PresentationMode = Staketracker.Core.Models.PresentationMode;
using System.Text.RegularExpressions;
using Staketracker.Core.Res;
using Staketracker.Core.Models.DelRec;

namespace Staketracker.Core.ViewModels.Events
{
    public class SEventDetailViewModel : BaseViewModel<PresentationContext<AuthReply>>
    {
        public SEventDetailViewModel(IMvxNavigationService navigationService)
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
            if (mode == PresentationMode.Create)
            {
                this.IsReading = false;
                this.IsEditing = true;
            }
            if (mode == PresentationMode.Edit || mode == PresentationMode.Read)
            {
                var apiReqExtra = new APIRequestExtraBody(authReply, "PrimaryKey", primaryKey.ToString());
                HttpResponseMessage events = await ApiManager.GetEventDetails(apiReqExtra, authReply.d.sessionId);
                PopulateControlsWithData(authReply, primaryKey, events);
            }

        }
        public override async Task Initialize()
        {
            PageTitle = AppRes.event_;

            await base.Initialize();
            RunSafe(GetFormUIControls(authReply, FormType.Events), true, AppRes.building_form_controls);
            UpdateTitle();
        }
        private async Task OnDeleteSEvent()
        {
            var result = await ShowDeleteConfirmation();

            if (result)
            {
                jsonTextObj jsonTextObj = new jsonTextObj(new DelRecReqModel() { KeyId = primaryKey, ScreenId = (int)ScreenKeyIdEnum.Event });
                HttpResponseMessage respMsg = await ApiManager.DelRec(jsonTextObj, authReply.d.sessionId);
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
            new MvvmCross.Presenters.Hints.MvxPopPresentationHint(typeof(SEventsListViewModel)));
            return;
        }
        internal async Task AddEvent()
        {
            jsonTextObj jsonTextObj = new jsonTextObj(pageFormValue);
            HttpResponseMessage events = await ApiManager.AddEvent(jsonTextObj, authReply.d.sessionId);
            await Add(events);
        }
        private async Task OnSave()
        {
            if (isFormValid())
            {
                FetchValuesFromFormControls("Event");
                AddEvent();
                changeView();

            }


        }

    }
}
