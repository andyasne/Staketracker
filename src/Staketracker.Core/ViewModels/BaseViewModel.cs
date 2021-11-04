using Acr.UserDialogs;
using MvvmCross.ViewModels;
using Newtonsoft.Json;
using Plugin.Settings;
using Staketracker.Core.Models;
using Staketracker.Core.Models.ApiRequestBody;
using Staketracker.Core.Models.EventsFormValue;
using Staketracker.Core.Models.FieldsValue;
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
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using PresentationMode = Staketracker.Core.Models.PresentationMode;

namespace Staketracker.Core.ViewModels
{
    public abstract class BaseViewModel : MvxViewModel, INotifyPropertyChanged
    {
        public Boolean isDevelopmentMode = true;

        public IUserDialogs PageDialog = UserDialogs.Instance;
        public IApiManager ApiManager;
        private IApiService<IStaketrackerApi> staketrackerApi = new ApiService<IStaketrackerApi>(Config.StaketrackerApiUrl);
        public ICommand OnDevelopmentNotifyCommand { get; }
        public static string? domainSelected;

        public static string? DomainSelected
        {
            get
            {
                return CrossSettings.Current.GetValueOrDefault("ProjectName", "Production");

            }
            set
            {
                CrossSettings.Current.AddOrUpdateValue("ProjectName", value);



                if (domainSelected != value)
                {
                    domainSelected = value;
                }

            }
        }

        private string headerTitle;
        public string HeaderTitle
        {
            get
            {
                return DomainSelected;
            }

        }

        private string pageTitle;
        public string PageTitle
        {
            get { return pageTitle; }
            set
            {
                SetField(ref pageTitle, value);

                pageTitle = value;

            }
        }

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
        public EventFormValue pageFormValue;
        public AuthReply authReply;
        public string name;
        public int primaryKey;
        public PresentationMode mode;
        public bool IsReading => mode == PresentationMode.Read;

        public bool IsEditing =>
                                 (mode == PresentationMode.Edit || mode == PresentationMode.Create);
        public PresentationMode Mode
        {
            get => mode;
            set
            {
                if (SetProperty(ref mode, value))
                {
                    RaisePropertyChanged(() => IsEditing);
                    RaisePropertyChanged(() => IsReading);
                }
            }
        }


        public void GetFormValues(string type)
        {
            pageFormValue = new EventFormValue();
            pageFormValue.InputFieldValues = new List<InputFieldValue>(FormContent.Count);
            pageFormValue.UserId = authReply.d.userId;
            if (mode == PresentationMode.Create)
            {
                pageFormValue.PrimaryKey = "";

            }
            else
            {
                pageFormValue.PrimaryKey = primaryKey.ToString();

            }
            pageFormValue.ProjectId = authReply.d.projectId;
            pageFormValue.Type = type;

            foreach (KeyValuePair<string, ValidatableObject<string>> _formContent in FormContent)
            {
                Staketracker.Core.Models.EventsFormValue.InputFieldValue inputValue = new InputFieldValue();
                inputValue.PrimaryKey = _formContent.Value.PrimaryKey;

                try
                {

                    if (_formContent.Value.isSelectOne)
                    {
                        if (_formContent.Value.SelectedItem == null)
                        {
                            inputValue.Value = null;
                        }
                        else
                        {
                            inputValue.Value = _formContent.Value.SelectedItem.PrimaryKey.ToString();

                        }

                    }
                    else if (_formContent.Value.FormAndDropDownField.InputType == "DateTime")
                    {
                        if (_formContent.Value.SelectedDate != null)
                        {
                            long selectedDate = ((long)_formContent.Value.SelectedDate.Value.Date.Ticks);
                            //int unixTimestamp = (int)_formContent.Value.SelectedDate.Value.Ticks;
                            //selectedDate = _formContent.Value.SelectedDate.Value.ToUniversalTime;
                            inputValue.Value = string.Format("/Date({0})/", selectedDate);

                        }
                    }
                    else if (_formContent.Value.FormAndDropDownField.InputType == "ListBoxMulti")
                    {
                        List<string> selectedValues = new List<string>();
                        if (_formContent.Value.SelectedItems != null && _formContent.Value.SelectedItems.Count > 0)
                        {
                            foreach (Models.FormAndDropDownField.DropdownValue selected in _formContent.Value.SelectedItems)
                            {
                                selectedValues.Add(selected.PrimaryKey);

                            }

                        }

                        inputValue.Value = selectedValues;


                    }
                    else
                    {
                        inputValue.Value = _formContent.Value.ToString();
                    }


                }
                catch (Exception ex)
                {

                }
                pageFormValue.InputFieldValues.Add(inputValue);
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
                    validatableObj.DropdownValues = new System.Collections.ObjectModel.ObservableCollection<DropdownValue>(d.DropdownValues);
                    validatableObj.PrimaryKey = d.PrimaryKey.ToString();

                    if (d.InputType == "DropDownList")
                    {
                        validatableObj.isSelectOne = true;

                    }
                    else if (d.InputType == "ListBoxMulti")
                    {
                        validatableObj.isSelectMultiple = true;
                    }

                    if (d.InputType == "DateTime")
                    {
                        validatableObj.isDateType = true;
                        validatableObj.ValidationsDateTime.Add(new IsDateSelectedRule { ValidationMessage = d.Label + " is Required" });

                    }
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
                    String label;

                    if (isDevelopmentMode)
                    {
                        label = d.PrimaryKey.ToString() + "-" + d.Label;
                    }
                    else
                    {
                        label = d.Label;
                    }

                    _formContent.Add(label, validatableObj);

                }
                FormContent = _formContent;
            }
            else
            {
                await PageDialog.AlertAsync("API Error while Getting Form Fields", "API Response Error", "Ok");
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
