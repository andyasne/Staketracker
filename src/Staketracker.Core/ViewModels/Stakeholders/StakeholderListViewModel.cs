using System;
using System.Net.Http;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using Staketracker.Core.Helpers;
using Staketracker.Core.Models;
using System.Threading.Tasks;
using System.Windows.Input;
using Newtonsoft.Json;
using Staketracker.Core.Models.ApiRequestBody;
using Staketracker.Core.Models.Stakeholders;
using Staketracker.Core.ViewModels.Communication;
using Staketracker.Core.ViewModels.Stakeholder;
using Telerik.XamarinForms.Primitives;
using Xamarin.Forms;
using PresentationMode = Staketracker.Core.Models.PresentationMode;
using Staketracker.Core.Models.EventsFormValue;

namespace Staketracker.Core.ViewModels.Stakeholders
{
    public class StakeholderListViewModel : BaseViewModel<AuthReply>

    {

        public ICommand AddStakeholderCommand { get; }


        private readonly IMvxNavigationService _navigationService;
        public IMvxCommand SearchCommand { get; }

        public StakeholderListViewModel(IMvxNavigationService navigationService)
        {
            this.HeaderTitle = "Stakeholder";

            _navigationService = navigationService;

            this.SearchCommand = new MvxAsyncCommand(OnSearch);
            AddStakeholderCommand = new Command(
                (parameter) =>
                {
                    RadTabView view = parameter as RadTabView;

                    if (view != null)
                    {
                        TabViewItem selectedTab = view.SelectedItem as TabViewItem;
                        selectedHeaderTitle = selectedTab.HeaderText;
                        authReply.attachment = selectedHeaderTitle;
                        _navigationService.Navigate<StakeholderDetailViewModel, PresentationContext<AuthReply>>(
                            new PresentationContext<AuthReply>(authReply, PresentationMode.Create));

                    }
                });



        }


        public ICommand ButtonClickCommand { get; private set; }
        public AuthReply authReply;

        //private async void OnCreateStakeholder((object sender)
        //{
        //    _navigationService.Navigate<StakeholderDetailViewModel, PresentationContext<AuthReply>>(
        //        new PresentationContext<AuthReply>(authReply, PresentationMode.Create));
        //}

        private Staketracker.Core.Models.Stakeholders.GroupedStakeholder _selectedStakeholder;

        public Staketracker.Core.Models.Stakeholders.GroupedStakeholder SelectedStakeholder
        {
            get => _selectedStakeholder;
            set
            {
                if (SetField(ref _selectedStakeholder, value))
                {

                    OnSelectedEventChanged(value);
                }
            }
        }

        private void OnSelectedEventChanged(Staketracker.Core.Models.Stakeholders.GroupedStakeholder groupStakeh)
        {
            authReply.attachment = "Groups";

            _navigationService.Navigate<StakeholderDetailViewModel, PresentationContext<AuthReply>>(
                new PresentationContext<AuthReply>(authReply, PresentationMode.Edit, int.Parse(groupStakeh.PrimaryKey)));


        }

        private Staketracker.Core.Models.Stakeholders.IndividualStakeholder _selectedIndividual;

        public Staketracker.Core.Models.Stakeholders.IndividualStakeholder SelectedIndividual
        {
            get => _selectedIndividual;
            set
            {
                if (SetField(ref _selectedIndividual, value))
                {

                    OnSelectedEventChangedIndividual(value);
                }
            }
        }

        private void OnSelectedEventChangedIndividual(Staketracker.Core.Models.Stakeholders.IndividualStakeholder individual)
        {
            authReply.attachment = "Individuals";

            _navigationService.Navigate<StakeholderDetailViewModel, PresentationContext<AuthReply>>(
                new PresentationContext<AuthReply>(authReply, PresentationMode.Edit, int.Parse(individual.PrimaryKey)));


        }

        private Staketracker.Core.Models.Stakeholders.LandParcelStakeholder _selectedLandParcel;
        public Staketracker.Core.Models.Stakeholders.LandParcelStakeholder SelectedLandParcel
        {
            get => _selectedLandParcel;
            set
            {
                if (SetField(ref _selectedLandParcel, value))
                {

                    OnSelectedEventChangedLandParcel(value);
                }
            }
        }

        private void OnSelectedEventChangedLandParcel(Staketracker.Core.Models.Stakeholders.LandParcelStakeholder landp)
        {
            authReply.attachment = "LandParcel";

            _navigationService.Navigate<StakeholderDetailViewModel, PresentationContext<AuthReply>>(
                new PresentationContext<AuthReply>(authReply, PresentationMode.Edit, int.Parse(landp.PrimaryKey)));


        }

        private string selectedHeaderTitle;
        public override void Prepare(AuthReply parameter)
        {
            base.Prepare();

            this.IsBusy = true;
            this.authReply = parameter;

            //this.Mode = parameter.Mode;
            RunSafe(GetLandParcelStakeholderDetails(authReply), true, "Loading Stakeholders");
            this.IsBusy = false;

        }
        public async override Task Initialize()
        {
            await base.Initialize();

            GetFormandDropDownFields(authReply, FormType.GroupedStakeholders);




        }

        private Models.Stakeholders.Stakeholders _allStakeholders;
        public Models.Stakeholders.Stakeholders allStakeholders
        {
            get => _allStakeholders;
            private set => SetField(ref _allStakeholders, value);
        }



        private Models.Stakeholders.Stakeholders stakeholders;
        internal async Task GetLandParcelStakeholderDetails(AuthReply authReply)
        {


            StakeholderBody body = new StakeholderBody();
            body.projectId = authReply.d.projectId;
            body.userId = authReply.d.userId;

            var apiReq = new jsonTextObj(body);
            HttpResponseMessage stakeholders = await ApiManager.GetAllStakeholders(apiReq, authReply.d.sessionId);

            if (stakeholders.IsSuccessStatusCode)
            {
                var response = await stakeholders.Content.ReadAsStringAsync();
                allStakeholders = await Task.Run(() => JsonConvert.DeserializeObject<Models.Stakeholders.Stakeholders>(response));


            }
            else
                await PageDialog.AlertAsync("API Error While retrieving", "API Response Error", "Ok");

        }


        public async Task Refresh()
        {
        }


        private async Task OnSearch()
        {
            if (Device.Idiom != TargetIdiom.Phone)
                return;

            await this._navigationService.Navigate<SearchResultsViewModel>();
        }
    }
}
