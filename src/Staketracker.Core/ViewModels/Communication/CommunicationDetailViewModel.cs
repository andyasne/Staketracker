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
using Staketracker.Core.ViewModels.CommunicationList;
using Xamarin.Forms;
using PresentationMode = Staketracker.Core.Models.PresentationMode;

namespace Staketracker.Core.ViewModels.Communication
{
    public class CommunicationDetailViewModel : BaseViewModel<PresentationContext<AuthReply>>
    {
        public CommunicationDetailViewModel(IMvxNavigationService navigationService)
        {

            this.navigationService = navigationService;
            BeginEditCommand = new Command(OnBeginEditCommunication);
            CommitCommand = new MvxAsyncCommand(OnCommitEditOrder);
            DeleteCommand = new MvxAsyncCommand(OnDeleteCommunication);
            CancelCommand = new MvxAsyncCommand(OnCancel);
        }

        private IMvxNavigationService navigationService;
        private CommunicationList.Communication targetCommunication, draftCommunication;
        private string targetCommunicationId;
        public PresentationMode mode;
        private string title;
        private CommunicationList.Communication _sEvent;

        public CommunicationList.Communication sEvent
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

        public CommunicationList.Communication DraftCommunication
        {
            get => draftCommunication;
            private set
            {
                if (SetProperty(ref draftCommunication, value))
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

        public bool IsReading => targetCommunication != null && mode == PresentationMode.Read;

        public bool IsEditing => draftCommunication != null &&
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
            RunSafe(GetFormandDropDownFields(authReply, FormType.Communications), true, "Building Form Controls");


            UpdateTitle();

            return;

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

        private void OnBeginEditCommunication()
        {
            if (!IsReading)
                return;


        }

        private async Task OnDeleteCommunication()
        {
            var result = await PageDialog.ConfirmAsync($"Are you sure you want to delete the Event?",
                "Delete Event", "Yes", "No");

            if (!result)
                return;

            await navigationService.Close(this);


        }

        private async Task OnCancel()

        {


        }


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

        private EventFormValue eventFormValue;

        private void getFormValues()
        {
            eventFormValue = new EventFormValue();
            eventFormValue.InputFieldValues = new List<InputFieldValue>(FormContent.Count);
            eventFormValue.UserId = authReply.d.userId;
            eventFormValue.PrimaryKey = primaryKey.ToString();
            eventFormValue.ProjectId = authReply.d.projectId;
            eventFormValue.Type = "Event";


            foreach (KeyValuePair<string, ValidatableObject<string>> _formContent in FormContent)
            {
                Staketracker.Core.Models.EventsFormValue.InputFieldValue inputValue = new InputFieldValue() { Value = _formContent.Value.ToString(), PrimaryKey = _formContent.Value.PrimaryKey };
                eventFormValue.InputFieldValues.Add(inputValue);
            }

        }


        internal async Task saveEvent()
        {

            AddEventsReply eventsReply;
            jsonTextObj jsonTextObj = new jsonTextObj(eventFormValue);
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

                getFormValues();

                saveEvent();

            }

        }


        private void InitializeEditData(CommunicationList.Communication sEvent)
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
                                }
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
