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

namespace Staketracker.Core.ViewModels.EventsList
{
    public class SEventDetailViewModel : MvxViewModel<PresentationContext<SEvent>>
    {
        public SEventDetailViewModel(IStaketrackerApi stkaeTrackerAPI, IMvxNavigationService navigationService)
        {
            this.stkaeTrackerAPI = stkaeTrackerAPI;
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
        private PresentationMode mode;
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


        public override void Prepare(PresentationContext<SEvent> parameter)
        {
            this.sEvent = parameter.Model;
            this.Mode = parameter.Mode;
        }

        public async override Task Initialize()
        {
            await base.Initialize();

            SEvent sEvent = null;
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
                    this.Title = this.targetSEvent.Name;
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
            //    bool result = await App.Current.MainPage.DisplayAlert("Delete sEvent", $"Are you sure you want to delete sEvent {this.targetSEvent.Name}?", "Yes", "No");
            //if (!result)
            //    return;

            //await this.stkaeTrackerAPI.RemoveSEventAsync(this.targetSEvent);
            //if (Device.Idiom == TargetIdiom.Phone)
            //{
            //    await this.navigationService.ChangePresentation(new MvvmCross.Presenters.Hints.MvxPopPresentationHint(typeof(SEventsViewModel)));
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
