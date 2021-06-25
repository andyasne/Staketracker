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
        public PresentationMode mode;
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

        public PresentationMode Mode
        {
            get => mode;
            private set
            {
                if (SetProperty(ref mode, value))
                {
                    RaisePropertyChanged(() => IsEditing);
                    RaisePropertyChanged(() => IsReading);
                }
            }
        }

        public bool IsReading => targetSEvent != null && mode == PresentationMode.Read;

        public bool IsEditing => draftSEvent != null &&
                                 (mode == PresentationMode.Edit || mode == PresentationMode.Create);

        public string Title
        {
            get => title;
            private set => SetProperty(ref title, value);
        }


        public Command BeginEditCommand { get; }
        public IMvxCommand CommitCommand { get; }
        public IMvxCommand CancelCommand { get; }
        public IMvxCommand DeleteCommand { get; }


        public AuthReply authReply;
        public int primaryKey;

        public override void Prepare(PresentationContext<AuthReply> parameter)
        {
            authReply = parameter.Model;
            Mode = parameter.Mode;
            primaryKey = parameter.PrimaryKey;
        }


        public override void ViewAppearing() => PopulateControls(authReply, primaryKey);

        public override async Task Initialize()
        {
            await base.Initialize();

            SelectedIndex = 1;
            RunSafe(GetFormandDropDownFields(authReply, FormType.Events), true, "Building Form Controls");


            UpdateTitle();

            return;
            // SEvent sEvent = null;
            if (mode == PresentationMode.Create)
            {
                sEvent = new SEvent();
                //sEvent.ImageURL = Constants.EmptySEventImage;
                //sEvent.LastOrderDate = DateTime.Today;
                //sEvent.SalesAmount = 500;
                DraftSEvent = sEvent;
                UpdateTitle();
                InitializeEditData(sEvent);
                return;
            }

            if (!string.IsNullOrEmpty(targetSEventId))
            {
                //     sEvent = await this.stkaeTrackerAPI.GetSEventAsync(this.targetSEventId);
            }

            if (sEvent == null)
                return;

            if (mode == PresentationMode.Edit)
            {
                targetSEvent = sEvent;
                SEvent copy = sEvent.Copy();
                DraftSEvent = copy;
                InitializeEditData(copy);
            }
            else
                sEvent = sEvent;

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
            var result = await PageDialog.ConfirmAsync($"Are you sure you want to delete Event {sEvent.Name}?",
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
            if (mode == PresentationMode.Read)
                return;

            DraftSEvent = null;
            UpdateTitle();

            if (mode == PresentationMode.Edit)
                Mode = PresentationMode.Read;

            // await this.navigationService.ChangePresentation(new MvvmCross.Presenters.Hints.MvxPopPresentationHint(typeof(SEventsViewModel)));
        }

        private bool isFormValid()
        {
            var isValid = true;
            foreach (KeyValuePair<string, ValidatableObject<string>> _formContent in FormContent)
                if (_formContent.Value.Validate() == false)
                    isValid = false;
            return isValid;
        }

        private async Task OnCommitEditOrder()
        {
            isFormValid();

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
                                    valObj.SelectedItems.AddRange(valObj.DropdownValues);
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
                await PageDialog.AlertAsync("API Error While Assigning Value", "API Response Error", "Ok");
            //  return null;
        }
    }
}
