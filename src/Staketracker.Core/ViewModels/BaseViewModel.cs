using Acr.UserDialogs;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Newtonsoft.Json;
using Plugin.Settings;
using Staketracker.Core.Models;
using Staketracker.Core.Models.AddEventsReply;
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
        private string headerTitle;
        public static string? domainSelected;
        private bool isReading = true;
        private bool isEditing = false;
        public string title;
        public IMvxNavigationService navigationService;
        private string pageTitle;
        public bool IsBusy { get; set; }
        public EventFormValue pageFormValue;
        public AuthReply authReply;
        public string name;
        public int primaryKey;
        protected PresentationMode mode;
        public IMvxCommand BeginEditCommand { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        public IMvxCommand DeleteCommand { get; set; }
        public IMvxCommand SaveCommand { get; set; }
        public async Task OnBeginEdit()
        {
            changeView();
        }
        public string Title
        {
            get => title;
            set => SetField(ref title, value);
        }
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

        public void changeView()
        {
            IsReading = !IsReading;
            IsEditing = !IsEditing;
            RaisePropertyChanged(() => IsEditing);
            RaisePropertyChanged(() => IsReading);
            if (IsReading)
            {

                Title = "View " + PageTitle;
            }
            else
            {
                Title = "Edit " + PageTitle;
            }

        }
        public bool IsReading
        {
            get => isReading;
            private set => SetField(ref isReading, value);
        }
        public bool IsEditing
        {
            get => isEditing;
            private set => SetField(ref isEditing, value);
        }
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
        public string HeaderTitle
        {
            get
            {
                return DomainSelected;
            }

        }
        public string PageTitle
        {
            get { return pageTitle; }
            set
            {
                SetField(ref pageTitle, value);

                pageTitle = value;

            }
        }
        protected void OnPropertyChanged(string propertyName)
             => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        public void UpdateTitle()
        {
            switch (mode)
            {
                case PresentationMode.Read:
                    Title = name;
                    break;
                case PresentationMode.Edit:
                    Title = $"Edit " + PageTitle;
                    break;
                case PresentationMode.Create:
                    Title = "Add New " + PageTitle;
                    break;
            }
        }
        public bool isFormValid()
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
        protected PresentationMode Mode
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
        public void FetchValuesFromFormControls(string type)
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
                InputFieldValue inputValue = new InputFieldValue();
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
        public async Task<bool> ShowDeleteConfirmation() => await PageDialog.ConfirmAsync($"Are you sure you want to delete the Event?",
                        "Delete Event", "Yes", "No");
        public async Task Add(HttpResponseMessage events)
        {
            AddEventsReply reply;

            if (events.IsSuccessStatusCode)
            {
                var response = await events.Content.ReadAsStringAsync();
                reply = await Task.Run(() => JsonConvert.DeserializeObject<AddEventsReply>(response));

                if (reply.d.successful == true)
                {
                    await PageDialog.AlertAsync("Saved Successfully", "Saved", "Ok");
                }
                else
                {
                    await PageDialog.AlertAsync(reply.d.message, "Error Saving", "Ok");

                }

            }
            else
                await PageDialog.AlertAsync("API Error While Saving", "API Response Error", "Ok");
        }
        public async Task PopulateControlsWithData(AuthReply authReply, int primaryKey, HttpResponseMessage resp)
        {
            FieldsValue fieldsValue;

            if (resp.IsSuccessStatusCode)
            {
                var response = await resp.Content.ReadAsStringAsync();
                fieldsValue = await Task.Run(() => JsonConvert.DeserializeObject<FieldsValue>(response));

                foreach (Field field in fieldsValue.d.Fields)
                    foreach (ValidatableObject<string> valObj in FormContent.Values)
                        if (valObj.FormAndDropDownField.PrimaryKey == field.PrimaryKey)
                            try
                            {
                                if (valObj.FormAndDropDownField.InputType == "DropDownList")
                                {
                                    valObj.SelectedItem = valObj.DropdownValues.FirstOrDefault<DropdownValue>();
                                }

                                else if (valObj.FormAndDropDownField.InputType == "ListBoxMulti")
                                {
                                    foreach (Models.FormAndDropDownField.DropdownValue dv in field.DropdownValues)
                                    {
                                        //       valObj.SelectedItems.Add(dv);
                                    }
                                }

                                else if (valObj.FormAndDropDownField.InputType == "CheckBox")
                                {
                                    if (field.Value != null && field.Value.ToString() == "on")
                                        valObj.Value = true.ToString();
                                    else
                                        valObj.Value = false.ToString();
                                }
                                else if (valObj.FormAndDropDownField.InputType == "DateTime")
                                {
                                    string dateval;
                                    if (field.Value != null)
                                    {
                                        dateval = field.Value.ToString();
                                        valObj.SelectedDate = DateTime.Parse(dateval);
                                        //valObj.SelectedDate = DateTime.Today;
                                    }

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

        }
        public async Task GetFormUIControls(AuthReply authReply, string type)
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
                    validatableObj.PrimaryKey = d.PrimaryKey.ToString();

                    if (d.InputType == "DropDownList")
                    {

                        if (d.DropdownValues != null)
                        {
                            validatableObj.DropdownValues = new System.Collections.ObjectModel.ObservableCollection<Staketracker.Core.Models.FormAndDropDownField.DropdownValue>(d.DropdownValues);
                        }
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
