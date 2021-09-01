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

namespace Staketracker.Core.ViewModels.Events
{
    public class SEventDetailViewModel : BaseViewModel<PresentationContext<AuthReply>>
    {
        public SEventDetailViewModel(IMvxNavigationService navigationService)
        {
            //this.stkaeTrackerAPI = stkaeTrackerAPI;
            this.navigationService = navigationService;
            BeginEditCommand = new Command(OnBeginEditSEvent);
            CommitCommand = new MvxAsyncCommand(OnCommitEditOrder);
            DeleteCommand = new MvxAsyncCommand(OnDeleteSEvent);
            CancelCommand = new MvxAsyncCommand(OnCancel);
        }

        private IMvxNavigationService navigationService;
        private SEvent targetSEvent, draftSEvent;
        private string targetSEventId;

        private string title;
        private SEvent _sEvent;

        public SEvent sEvent
        {
            get => _sEvent;
            private set
            {
                if (SetProperty(ref _sEvent, value))
                {
                    RaisePropertyChanged(() => IsEditing);
                    RaisePropertyChanged(() => IsReading);
                }
            }
        }

        public SEvent DraftSEvent
        {
            get => draftSEvent;
            private set
            {
                if (SetProperty(ref draftSEvent, value))
                {
                    RaisePropertyChanged(() => IsEditing);
                    RaisePropertyChanged(() => IsReading);
                }
            }
        }




        public string Title
        {
            get => title;
            private set => SetProperty(ref title, value);
        }

        public Command BeginEditCommand { get; }
        public IMvxCommand CommitCommand { get; }
        public IMvxCommand CancelCommand { get; }
        public IMvxCommand DeleteCommand { get; }


        private bool isBusy;
        public bool IsBusy
        {
            get => isBusy;
            set => SetProperty(ref isBusy, value);
        }

        public override void Prepare(PresentationContext<AuthReply> parameter)
        {
            authReply = parameter.Model;
            Mode = parameter.Mode;
            primaryKey = parameter.PrimaryKey;
        }

        public override void ViewAppearing()
        {
            IsBusy = true;
            if (mode == PresentationMode.Edit)
            {
                PopulateControls(authReply, primaryKey);
            }

            IsBusy = false;

        }

        public override async Task Initialize()
        {
            await base.Initialize();

            SelectedIndex = 1;
            RunSafe(GetFormandDropDownFields(authReply, FormType.Events), true, "Building Form Controls");

            UpdateTitle();


        }

        private void UpdateTitle()
        {
            switch (mode)
            {
                case PresentationMode.Read:
                    Title = sEvent.Name;
                    break;
                case PresentationMode.Edit:
                    Title = $"Edit Event";
                    break;
                case PresentationMode.Create:
                    Title = "Add New Event";
                    break;
            }
        }

        private void OnBeginEditSEvent()
        {
            if (!IsReading)
                return;

            SEvent sEvent = targetSEvent.Copy();
            Mode = PresentationMode.Edit;
            UpdateTitle();
            InitializeEditData(sEvent);
            DraftSEvent = sEvent;
        }

        private async Task OnDeleteSEvent()
        {
            var result = await PageDialog.ConfirmAsync($"Are you sure you want to delete the Event?",
                "Delete Event", "Yes", "No");

            if (!result)
                return;

            //await this.stkaeTrackerAPI.RemoveSEventAsync(this.targetSEvent);
            //if (Device.Idiom == TargetIdiom.Phone)
            //{
            await navigationService.Close(this);

            await navigationService.ChangePresentation(
                new MvvmCross.Presenters.Hints.MvxPopPresentationHint(typeof(SEventsListViewModel)));
            //}
        }

        private async Task OnCancel()

        {

            //  await this.navigationService.Close(this);
            await this.navigationService.ChangePresentation(new MvvmCross.Presenters.Hints.MvxPopPresentationHint(typeof(SEventsListViewModel)));
            return;

            if (mode == PresentationMode.Read)
                return;

            DraftSEvent = null;
            UpdateTitle();

            if (mode == PresentationMode.Edit)
                Mode = PresentationMode.Read;

            // await this.navigationService.ChangePresentation(new MvvmCross.Presenters.Hints.MvxPopPresentationHint(typeof(SEventsViewModel)));
        }

        private List<Staketracker.Core.Models.EventsFormValue.InputFieldValue> valueList;

        private bool isFormValid()
        {
            var isValid = true;
            foreach (KeyValuePair<string, ValidatableObject<string>> _formContent in FormContent)
            {
                if (_formContent.Value.Validate() == false)
                {
                    isValid = false;
                }
            }

            return isValid;
        }


        internal async Task saveEvent()
        {

            AddEventsReply eventsReply;
            jsonTextObj jsonTextObj = new jsonTextObj(pageFormValue);
            HttpResponseMessage events = await ApiManager.AddEvent(jsonTextObj, authReply.d.sessionId);

            if (events.IsSuccessStatusCode)
            {
                var response = await events.Content.ReadAsStringAsync();
                eventsReply = await Task.Run(() => JsonConvert.DeserializeObject<AddEventsReply>(response));

                if (eventsReply.d.successful == true)
                {
                    await PageDialog.AlertAsync("Event Saved Successfully", "Event Saved", "Ok");
                }
                else
                {
                    await PageDialog.AlertAsync(eventsReply.d.message, "Error Saving Event", "Ok");

                }

            }
            else
                await PageDialog.AlertAsync("API Error While Saving Event", "API Response Error", "Ok");
            //  return null;
        }
        private async Task OnCommitEditOrder()
        {
            if (isFormValid())
            {

                getFormValues("Event");

                saveEvent();

            }

            //   await this.navigationService.ChangePresentation(new MvvmCross.Presenters.Hints.MvxPopPresentationHint(typeof(SEventsListViewModel)));

            //Redirect

            if (Mode == PresentationMode.Read)
                return;

            //if (!this.draftSEvent.Validate(out IList<string> errors))
            //{
            //    await App.Current.MainPage.DisplayAlert("Validation failed", "Please check your data and try again" + Environment.NewLine + String.Join(Environment.NewLine, errors), "OK");
            //    return;
            //}

            //   var updatedSEvent = await this.stkaeTrackerAPI.SaveSEventAsync(this.draftSEvent);

            DraftSEvent = null;
            targetSEvent = null;

            //   this.SEvent = updatedSEvent;
            //      this.Mode = PresentationMode.Read;

            //        this.UpdateTitle();

            //    if (Device.Idiom != TargetIdiom.Phone)
            //      await this.navigationService.ChangePresentation(new MvvmCross.Presenters.Hints.MvxPopPresentationHint(typeof(SEventsViewModel)));
        }

        private void InitializeEditData(SEvent sEvent)
        {
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

        internal async Task PopulateControls(AuthReply authReply, int primaryKey)
        {
            FieldsValue fieldsValue;
            var apiReqExtra = new APIRequestExtraBody(authReply, "PrimaryKey", primaryKey.ToString());
            HttpResponseMessage events = await ApiManager.GetEventDetails(apiReqExtra, authReply.d.sessionId);

            if (events.IsSuccessStatusCode)
            {
                var response = await events.Content.ReadAsStringAsync();
                fieldsValue = await Task.Run(() => JsonConvert.DeserializeObject<FieldsValue>(response));

                foreach (Field field in fieldsValue.d.Fields)
                    foreach (ValidatableObject<string> valObj in FormContent.Values)
                        if (valObj.FormAndDropDownField.PrimaryKey == field.PrimaryKey)
                            try
                            {
                                if (valObj.FormAndDropDownField.InputType == "DropDownList")
                                    valObj.SelectedItem = valObj.DropdownValues.FirstOrDefault<DropdownValue>();
                                else if (valObj.FormAndDropDownField.InputType == "ListBoxMulti")
                                {
                                } //  valObj.SelectedItems.AddRange(valObj.DropdownValues);
                                else if (valObj.FormAndDropDownField.InputType == "CheckBox")
                                {
                                    if (field.Value != null && field.Value.ToString() == "on")
                                        valObj.Value = true.ToString();
                                    else
                                        valObj.Value = false.ToString();
                                }

                                else
                                {
                                    if (field.Value != null)
                                        valObj.Value = field.Value.ToString();
                                }
                            }
                            catch (Exception ex)
                            {
                            }

            }
            else
                await PageDialog.AlertAsync("API Error While Assigning Value to UI Controls", "API Response Error", "Ok");
            //  return null;
        }
    }
}
