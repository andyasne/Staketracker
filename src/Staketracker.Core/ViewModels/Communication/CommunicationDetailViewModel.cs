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
using Staketracker.Core.ViewModels.CommunicationList;
using Xamarin.Forms;
using PresentationMode = Staketracker.Core.Models.PresentationMode;
using Staketracker.Core.Models.Stakeholders;
using Staketracker.Core.Models.Communication;
using Staketracker.Core.Models.EventsFormValue;

namespace Staketracker.Core.ViewModels.Communication
{
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
                var apiReqExtra = new APIRequestExtraBody(authReply, "PrimaryKey", primaryKey.ToString());
                HttpResponseMessage communications = await ApiManager.GetCommunicationDetails(new jsonTextObj(apiReqExtra), authReply.d.sessionId);
                PopulateControlsWithData(authReply, primaryKey, communications);
            }

        }
        public override async Task Initialize()
        {
            await base.Initialize();
            RunSafe(GetFormUIControls(authReply, FormType.Communications), true, "Building Form Controls");
            UpdateTitle();
        }
        private async Task OnDelete()
        {
            var result = await ShowDeleteConfirmation();
            if (result)
            {
                //TODO: Add Delete Logic here

                NavigateToList();
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
