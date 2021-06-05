using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using Newtonsoft.Json;
using Staketracker.Core.Models;
using Staketracker.Core.Models.ApiRequestBody;
using Staketracker.Core.Models.FormAndDropDownField;
using Staketracker.Core.Validators;
using Xamarin.Forms;

namespace Staketracker.Core.ViewModels.CommunicationList
{


    public class Communication
    {
        public string Name { get; set; }
        public string Date { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
    }


    public class CommunicationListViewModel : BaseViewModel<AuthReply>

    {

        private string headerTitle;

        private readonly IMvxNavigationService _navigationService;
        public IMvxCommand SearchCommand { get; }

        public CommunicationListViewModel(IMvxNavigationService navigationService)
        {
            this.headerTitle = "Communication";

            _navigationService = navigationService;

            this.SearchCommand = new MvxAsyncCommand(OnSearch);
        }
        public AuthReply authReply;


        public override void Prepare(AuthReply parameter)
        {
            this.authReply = parameter;
            //this.Mode = parameter.Mode;
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

        public async override Task Initialize()
        {
            await base.Initialize();

            GetFormandDropDownFields(authReply);


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
                    //   validatableObj.DropdownValues = d.DropdownValues;

                    FormContent.Add(d.Label, validatableObj);

                }
            }
            else
            {
                await PageDialog.AlertAsync("API Error while Geting Form Fields", "API Response Error", "Ok");
                //  return null;
            }
        }


        public async Task Refresh()
        {
        }

        public string HeaderTitle
        {
            get => headerTitle;
            private set => SetProperty(ref headerTitle, value);
        }

        private async Task OnSearch()
        {
            if (Device.Idiom != TargetIdiom.Phone)
                return;

            await this._navigationService.Navigate<SearchResultsViewModel>();
        }
    }
}
