using System.Collections.ObjectModel;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using Staketracker.Core.Models;
using Xamarin.Forms;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Xamarin.Forms;
using PresentationMode = Staketracker.Core.Models.PresentationMode;
using System.Collections.Generic;
using Staketracker.Core.Validators;
using System;
using System.ComponentModel;
using Staketracker.Core.Models.ApiRequestBody;
using System.Net.Http;
using Newtonsoft.Json;
using Staketracker.Core.Models.FormAndDropDownField;

namespace Staketracker.Core.ViewModels.Events
{
    public class SEventDetailViewModel : BaseViewModel<PresentationContext<AuthReply>>
    {
        public SEventDetailViewModel(IMvxNavigationService navigationService)
        {
            //this.stkaeTrackerAPI = stkaeTrackerAPI;
            this.navigationService = navigationService;
            this.BeginEditCommand = new Command(OnBeginEditSEvent);
            this.CommitCommand = new MvxAsyncCommand(OnCommitEditOrder);
            this.DeleteCommand = new MvxAsyncCommand(OnDeleteSEvent);
            this.CancelCommand = new MvxAsyncCommand(OnCancel);
        }

        private IStaketrackerApi stkaeTrackerAPI;
        private IMvxNavigationService navigationService;
        private SEvent targetSEvent, draftSEvent;
        private string targetSEventId;
        public PresentationMode mode;
        private string title;
        private SEvent _sEvent;
        public SEvent sEvent
        {
            get => this._sEvent;
            private set
            {
                if (SetProperty(ref this._sEvent, value))
                {
                    RaisePropertyChanged(() => IsEditing);
                    RaisePropertyChanged(() => IsReading);
                }
            }
        }

        public SEvent DraftSEvent
        {
            get => this.draftSEvent;
            private set
            {
                if (SetProperty(ref this.draftSEvent, value))
                {
                    RaisePropertyChanged(() => IsEditing);
                    RaisePropertyChanged(() => IsReading);
                }
            }
        }

        public PresentationMode Mode
        {
            get => this.mode;
            private set
            {
                if (SetProperty(ref this.mode, value))
                {
                    RaisePropertyChanged(() => IsEditing);
                    RaisePropertyChanged(() => IsReading);
                }
            }
        }

        public bool IsReading => this.targetSEvent != null && this.mode == PresentationMode.Read;
        public bool IsEditing => this.draftSEvent != null &&
            (this.mode == PresentationMode.Edit || this.mode == PresentationMode.Create);

        public string Title
        {
            get => this.title;
            private set => SetProperty(ref this.title, value);
        }

     

        public Command BeginEditCommand { get; }
        public IMvxCommand CommitCommand { get; }
        public IMvxCommand CancelCommand { get; }
        public IMvxCommand DeleteCommand { get; }


        public AuthReply authReply;
        public override void Prepare(PresentationContext<AuthReply> parameter)
        {
            this.authReply = parameter.Model;
            this.Mode = parameter.Mode;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private Dictionary<string, ValidatableObject<string>> formContent = new Dictionary<string, ValidatableObject<string>>();

        public Dictionary<string, ValidatableObject<string>> FormContent
        {
            get { return formContent; }
            set
            {
                formContent = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("FormContent"));
            }
        }



        async void GetFormandDropDownFields(AuthReply authReply)
        {

            FormFieldBody formFieldBody = new FormFieldBody(authReply, "Events");

            HttpResponseMessage events = await ApiManager.GetFormAndDropDownFieldValues(formFieldBody, authReply.d.sessionId);

            if (events.IsSuccessStatusCode)
            {
                var response = await events.Content.ReadAsStringAsync();
                FormAndDropDownField formAndDropDownField = await Task.Run(() => JsonConvert.DeserializeObject<FormAndDropDownField>(response));
                // return eventsReply;

                foreach (Models.FormAndDropDownField.D d in formAndDropDownField.d)
                {

                    ValidatableObject<string> validatableObj = new ValidatableObject<string>();
                    validatableObj.FormAndDropDownField = d;
                    validatableObj.DropdownValues = d.DropdownValues;

                    formContent.Add(d.InputType, validatableObj);

                }
            }
            else
            {
                await PageDialog.AlertAsync("API Error while Geting Form Fields", "API Response Error", "Ok");
                //  return null;
            }
        }

        public async override Task Initialize()
        {
            await base.Initialize();

            GetFormandDropDownFields(authReply);

            return;
            // SEvent sEvent = null;
            if (this.mode == PresentationMode.Create)
            {
                sEvent = new SEvent();
                //sEvent.ImageURL = Constants.EmptySEventImage;
                //sEvent.LastOrderDate = DateTime.Today;
                //sEvent.SalesAmount = 500;
                this.DraftSEvent = sEvent;
                this.UpdateTitle();
                this.InitializeEditData(sEvent);
                return;
            }

            if (!string.IsNullOrEmpty(this.targetSEventId))
            {
                //     sEvent = await this.stkaeTrackerAPI.GetSEventAsync(this.targetSEventId);
            }

            if (sEvent == null)
                return;

            if (this.mode == PresentationMode.Edit)
            {
                this.targetSEvent = sEvent;
                var copy = sEvent.Copy();
                this.DraftSEvent = copy;
                this.InitializeEditData(copy);
            }
            else
            {
                this.sEvent = sEvent;
            }
            this.UpdateTitle();
        }

        private void UpdateTitle()
        {
            switch (this.mode)
            {
                case PresentationMode.Read:
                    this.Title = this.sEvent.Name;
                    break;
                case PresentationMode.Edit:
                    this.Title = $"Edit Event";
                    break;
                case PresentationMode.Create:
                    this.Title = "Add New Event";
                    break;
            }
        }

        private void OnBeginEditSEvent()
        {
            if (!this.IsReading)
                return;

            var sEvent = this.targetSEvent.Copy();
            this.Mode = PresentationMode.Edit;
            UpdateTitle();
            this.InitializeEditData(sEvent);
            this.DraftSEvent = sEvent;
        }

        private async Task OnDeleteSEvent()
        {
            bool result = await PageDialog.ConfirmAsync($"Are you sure you want to delete Event {this.sEvent.Name}?", "Delete Event", "Yes", "No");

            if (!result)
                return;

            //await this.stkaeTrackerAPI.RemoveSEventAsync(this.targetSEvent);
            //if (Device.Idiom == TargetIdiom.Phone)
            //{
            await this.navigationService.Close(this);

            await this.navigationService.ChangePresentation(new MvvmCross.Presenters.Hints.MvxPopPresentationHint(typeof(SEventsListViewModel)));
            //}


        }

        private async Task OnCancel()
        {
            if (this.mode == PresentationMode.Read)
                return;

            this.DraftSEvent = null;
            this.UpdateTitle();

            if (this.mode == PresentationMode.Edit)
            {
                this.Mode = PresentationMode.Read;
            }

            // await this.navigationService.ChangePresentation(new MvvmCross.Presenters.Hints.MvxPopPresentationHint(typeof(SEventsViewModel)));
        }

        private async Task OnCommitEditOrder()
        {
            if (this.Mode == PresentationMode.Read)
                return;

            //if (!this.draftSEvent.Validate(out IList<string> errors))
            //{
            //    await App.Current.MainPage.DisplayAlert("Validation failed", "Please check your data and try again" + Environment.NewLine + String.Join(Environment.NewLine, errors), "OK");
            //    return;
            //}

            //   var updatedSEvent = await this.stkaeTrackerAPI.SaveSEventAsync(this.draftSEvent);

            this.DraftSEvent = null;
            this.targetSEvent = null;
            //   this.SEvent = updatedSEvent;
            this.Mode = PresentationMode.Read;

            this.UpdateTitle();

            //    if (Device.Idiom != TargetIdiom.Phone)
            //      await this.navigationService.ChangePresentation(new MvvmCross.Presenters.Hints.MvxPopPresentationHint(typeof(SEventsViewModel)));
        }


        private void InitializeEditData(SEvent sEvent)
        {
        }


    }
}
