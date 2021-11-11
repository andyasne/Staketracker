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
using Staketracker.Core.Models.Stakeholders;
using Staketracker.Core.Models.Communication;

namespace Staketracker.Core.ViewModels.Communication
{
    public class CommunicationDetailViewModel : BaseViewModel<PresentationContext<AuthReply>>
    {
        public CommunicationDetailViewModel(IMvxNavigationService navigationService)
        {

            this.navigationService = navigationService;
            BeginEditCommand = new Command(OnBeginEditCommunication);
            SaveCommand = new MvxAsyncCommand(OnCommitEditOrder);
            DeleteCommand = new MvxAsyncCommand(OnDeleteCommunication);
            CancelCommand = new MvxAsyncCommand(OnCancel);
        }
        public IMvxCommand SaveCommand { get; }

        private IMvxNavigationService navigationService;
        private CommunicationList.Communication targetCommunication, draftCommunication;
        private string targetCommunicationId;

        private string title;
        private CommunicationList.Communication _communication;

        public Command BeginEditCommand { get; }
        public IMvxCommand CommitCommand { get; }
        public IMvxCommand CancelCommand { get; }
        public IMvxCommand DeleteCommand { get; }



        private bool isBusy;




        public string Title
        {
            get => title;
            private set => SetProperty(ref title, value);
        }

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
            name = parameter.Name;

        }

        public override void ViewAppearing()
        {
            IsBusy = true;
            if (mode == PresentationMode.Edit || mode == PresentationMode.Read)
            {
                CommunicationDetailReq body = new CommunicationDetailReq()
                {
                    projectId = authReply.d.projectId,
                    userId = authReply.d.userId,
                    ID = primaryKey

                };
                jsonTextObj jto = new jsonTextObj(body);
                PopulateControls(authReply, jto);

            }

            IsBusy = false;

        }

        internal async Task PopulateControls(AuthReply authReply, jsonTextObj jto)
        {
            FieldsValue fieldsValue;
            HttpResponseMessage responseMessage = await ApiManager.GetCommunicationDetails(jto, authReply.d.sessionId);

            if (responseMessage.IsSuccessStatusCode)
            {
                var response = await responseMessage.Content.ReadAsStringAsync();
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
                await PageDialog.AlertAsync("API Error While Assigning Value to UI Controls", "API Response Error", "Ok");
            //  return null;
        }
        public override async Task Initialize()
        {
            await base.Initialize();

            SelectedIndex = 1;

            RunSafe(GetFormandDropDownFields(authReply, FormType.Communications), true, "Building Form Controls");

            UpdateTitle();

            return;

        }


        private void OnBeginEditCommunication()
        {
            if (!IsReading)
                return;

        }

        private async Task OnDeleteCommunication()
        {
            var result = await PageDialog.ConfirmAsync($"Are you sure you want to delete the Communication?",
                "Delete Communication", "Yes", "No");

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



        internal async Task save()
        {

            AddEventsReply responseReply;
            jsonTextObj jsonTextObj = new jsonTextObj(pageFormValue);
            HttpResponseMessage communication = await ApiManager.AddCommunication(jsonTextObj, authReply.d.sessionId);

            if (communication.IsSuccessStatusCode)
            {
                var response = await communication.Content.ReadAsStringAsync();
                responseReply = await Task.Run(() => JsonConvert.DeserializeObject<AddEventsReply>(response));

                if (responseReply.d.successful == true)
                {
                    await PageDialog.AlertAsync("Communication Saved Successfully", "Communication Saved", "Ok");
                }
                else
                {
                    await PageDialog.AlertAsync(responseReply.d.message, "Error Saving Communication", "Ok");

                }

            }
            else
                await PageDialog.AlertAsync("API Error While Saving Communication", "API Response Error", "Ok");

        }
        private async Task OnCommitEditOrder()
        {
            if (isFormValid())
            {

                GetFormValues("Communication");

                save();

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
