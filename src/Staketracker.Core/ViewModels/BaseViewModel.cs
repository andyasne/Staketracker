using Acr.UserDialogs;
using MvvmCross.ViewModels;
using Newtonsoft.Json;
using Staketracker.Core.Models;
using Staketracker.Core.Models.ApiRequestBody;
using Staketracker.Core.Models.FormAndDropDownField;
using Staketracker.Core.Services;
using Staketracker.Core.Validators;
using Staketracker.Core.Validators.Rules;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Staketracker.Core.ViewModels
{
    public abstract class BaseViewModel : MvxViewModel, INotifyPropertyChanged
    {
        public Boolean isDevelopmentMode = true;


        public IUserDialogs PageDialog = UserDialogs.Instance;
        public IApiManager ApiManager;
        private IApiService<IStaketrackerApi> staketrackerApi = new ApiService<IStaketrackerApi>(Config.StaketrackerApiUrl);
        public ICommand OnDevelopmentNotifyCommand { get; }

        private string headerTitle;
        public string HeaderTitle { get => headerTitle; set => SetProperty(ref headerTitle, value); }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
             => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private Dictionary<string, ValidatableObject<string>> formContent = new Dictionary<string, ValidatableObject<string>>();

        public Dictionary<string, ValidatableObject<string>> FormContent
        {
            get
            {
                return formContent;
            }
            set
            {

                if (this.formContent != value)
                {
                    SetField(ref formContent, value);
                    this.formContent = value;
                }

            }
        }

        public async Task GetFormandDropDownFields(AuthReply authReply, string type)
        {

            FormFieldBody formFieldBody = new FormFieldBody(authReply, type);

            HttpResponseMessage events = await ApiManager.GetFormAndDropDownFieldValues(formFieldBody, authReply.d.sessionId);
            Dictionary<string, ValidatableObject<string>> _formContent = new Dictionary<string, ValidatableObject<string>>();

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
                    validatableObj.PrimaryKey = d.PrimaryKey.ToString();
                  
                    if (d.MandatoryField == true)
                    {
                        if (d.InputType == "DropDownList")
                        {
                            validatableObj.ValidationsList.Add(new IsDropdownSelectedRule { ValidationMessage = d.Label + " is Required" });

                        }
                        else if (d.InputType == "ListBoxMulti")
                        {
                            validatableObj.ValidationsList.Add(new IsDropdownSelectedRule { ValidationMessage = d.Label + " is Required" });

                        }



                        else
                        {
                            validatableObj.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = d.Label + " is Required" });

                        }
                    }

                    _formContent.Add(d.Label, validatableObj);

                }
                FormContent = _formContent;
            }
            else
            {
                await PageDialog.AlertAsync("API Error while Geting Form Fields", "API Response Error", "Ok");
                //  return null;
            }
        }



        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        public bool IsBusy { get; set; }

        public BaseViewModel()
        {
            ApiManager = new ApiManager(staketrackerApi);

            OnDevelopmentNotifyCommand = new Command(() =>
            {
                OnDevelopment().Start();
            });
        }
        public Task OnDevelopment()
        {
            return new Task(() =>
            {
                var msg = "This Page is Under Development";
                PageDialog.Toast(msg, TimeSpan.FromSeconds(3));
            });

        }
        public async Task RunSafe(Task task, bool ShowLoading = true, string loadinMessage = null)
        {
            try
            {
                if (IsBusy)
                    return;

                IsBusy = true;

                if (ShowLoading)
                    UserDialogs.Instance.ShowLoading(loadinMessage ?? "Loading");

                await task;
            }
            catch (Exception e)
            {
                IsBusy = false;
                //   UserDialogs.Instance.HideLoading();
                Debug.WriteLine(e.ToString());
                //await Application.Current.MainPage.DisplayAlert("Eror", "Check your internet connection", "Ok");
                await PageDialog.AlertAsync("Check your internet connection", "Error", "Ok");
            }
            finally
            {
                IsBusy = false;
                if (ShowLoading)
                    UserDialogs.Instance.HideLoading();
            }
        }
    }
}
