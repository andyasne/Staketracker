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

        private IMvxNavigationService navigationService;
        public IMvxCommand BeginEditCommand { get; }
        public IMvxCommand DeleteCommand { get; }
        public IMvxCommand SaveCommand { get; }
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

                HttpResponseMessage events = await ApiManager.GetEventDetails(apiReqExtra, authReply.d.sessionId);

                PopulateControlsWithData(authReply, primaryKey, events);
            }

        }
        public override async Task Initialize()
        {
            await base.Initialize();

            RunSafe(GetFormUIControls(authReply, FormType.Events), true, "Building Form Controls");

            UpdateTitle();


        }
        private async Task OnBeginEdit()
        {
            changeView();
        }
        private async Task OnDeleteSEvent()
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
