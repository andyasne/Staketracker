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

namespace Staketracker.Core.ViewModels.Stakeholder
{
    public class StakeholderDetailViewModel : BaseViewModel<PresentationContext<AuthReply>>
    {
        public StakeholderDetailViewModel(IMvxNavigationService navigationService)
        {

            this.navigationService = navigationService;
            BeginEditCommand = new Command(OnBeginEditStakeholder);
            CommitCommand = new MvxAsyncCommand(OnCommitEditOrder);
            DeleteCommand = new MvxAsyncCommand(OnDeleteStakeholder);
            CancelCommand = new MvxAsyncCommand(OnCancel);
        }

        private IMvxNavigationService navigationService;
        private Models.Stakeholders.Stakeholders targetStakeholder, draftStakeholder;
        private string targetStakeholderId;
        public PresentationMode mode;
        private string title;
        private Models.Stakeholders.Stakeholders _stakeholder;

        public Command BeginEditCommand { get; }
        public IMvxCommand CommitCommand { get; }
        public IMvxCommand CancelCommand { get; }
        public IMvxCommand DeleteCommand { get; }

        public AuthReply authReply;

        public int primaryKey;

        private bool isBusy;
        public Models.Stakeholders.Stakeholders Stakeholder
        {
            get => _stakeholder;
            private set
            {
                if (SetProperty(ref _stakeholder, value))
                {
                    RaisePropertyChanged(() => IsEditing);
                    RaisePropertyChanged(() => IsReading);
                }
            }
        }

        public Models.Stakeholders.Stakeholders DraftStakeholder
        {
            get => draftStakeholder;
            private set
            {
                if (SetProperty(ref draftStakeholder, value))
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

        public bool IsReading => targetStakeholder != null && mode == PresentationMode.Read;

        public bool IsEditing => draftStakeholder != null &&
                                 (mode == PresentationMode.Edit || mode == PresentationMode.Create);

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
        }



        public override async Task Initialize()
        {
            await base.Initialize();

            SelectedIndex = 1;
            if (authReply.attachment.ToString() == "Groups")
                RunSafe(GetFormandDropDownFields(authReply, FormType.GroupedStakeholders), true, "Building Group Form Controls");
            else if (authReply.attachment.ToString() == "Individuals")
                RunSafe(GetFormandDropDownFields(authReply, FormType.IndividualStakeholders), true, "Building Individual Form Controls");
            else
                RunSafe(GetFormandDropDownFields(authReply, FormType.LandParcelStakeholders), true, "Building Land Parcel Form Controls");


            UpdateTitle();

            return;

        }

        private void UpdateTitle()
        {
            switch (mode)
            {
                case PresentationMode.Read:
                    //       Title = Stakeholder.Name;
                    break;
                case PresentationMode.Edit:
                    Title = $"Edit " + authReply.attachment.ToString();
                    break;
                case PresentationMode.Create:
                    Title = "Add New " + authReply.attachment.ToString();
                    break;
            }
        }

        private void OnBeginEditStakeholder()
        {
            if (!IsReading)
                return;

        }


        private async Task OnDeleteStakeholder()
        {
            var result = await PageDialog.ConfirmAsync($"Are you sure you want to delete the Stakeholder?",
                "Delete Stakeholder", "Yes", "No");

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

        private EventFormValue pageFormValue;

        private void getFormValues(string type)
        {
            pageFormValue = new EventFormValue();
            pageFormValue.InputFieldValues = new List<InputFieldValue>(FormContent.Count);
            pageFormValue.UserId = authReply.d.userId;
            pageFormValue.PrimaryKey = primaryKey.ToString();
            pageFormValue.ProjectId = authReply.d.projectId;
            pageFormValue.Type = type;

            foreach (KeyValuePair<string, ValidatableObject<string>> _formContent in FormContent)
            {
                Staketracker.Core.Models.EventsFormValue.InputFieldValue inputValue = new InputFieldValue() { Value = _formContent.Value.ToString(), PrimaryKey = _formContent.Value.PrimaryKey };
                pageFormValue.InputFieldValues.Add(inputValue);
            }

        }

        internal async Task save()
        {

            AddEventsReply responseReply;
            jsonTextObj jsonTextObj = new jsonTextObj(pageFormValue);
            HttpResponseMessage events = await ApiManager.AddEvent(jsonTextObj, authReply.d.sessionId);

            if (events.IsSuccessStatusCode)
            {
                var response = await events.Content.ReadAsStringAsync();
                responseReply = await Task.Run(() => JsonConvert.DeserializeObject<AddEventsReply>(response));

                if (responseReply.d.successful == true)
                {
                    await PageDialog.AlertAsync("Stakeholder Saved Successfully", "Stakeholder Saved", "Ok");
                }
                else
                {
                    await PageDialog.AlertAsync(responseReply.d.message, "Error Saving Stakeholder", "Ok");

                }

            }
            else
                await PageDialog.AlertAsync("API Error While Saving Stakeholder", "API Response Error", "Ok");
            //  return null;
        }
        private async Task OnCommitEditOrder()
        {
            if (isFormValid())
            {

                if (authReply.attachment.ToString() == "Groups")
                    getFormValues(FormType.GroupedStakeholders);
                else if (authReply.attachment.ToString() == "Individuals")
                    getFormValues(FormType.IndividualStakeholders);
                else
                    getFormValues(FormType.LandParcelStakeholders);


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

        public override void ViewAppearing()
        {
            IsBusy = true;
            if (mode == PresentationMode.Edit)
            {
                getReqBody().ContinueWith(f =>
                {
                    PopulateControls(authReply, f.Result);
                }

               );

            }

            IsBusy = false;

        }

        private async Task<HttpResponseMessage> getReqBody()
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


            return responseMessage;
        }


    }
}
