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
using Staketracker.Core.Models.LinkedTo;
using Staketracker.Core.Res;
using Staketracker.Core.Services;
using Staketracker.Core.Validators;
using Staketracker.Core.Validators.Rules;
using Staketracker.Core.ViewModels.Linked.Communication;
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
        public ICommand OpenLinkPage { get; set; }

        public BaseViewModel()
        {
            ApiManager = new ApiManager(staketrackerApi);

            OnDevelopmentNotifyCommand = new Command(() =>
            {
                OnDevelopment().Start();
            });
            OpenLinkPage = new Command(OpenLinkPage_);

        }


        private async void OpenLinkPage_()
        {
            await navigationService.Navigate<CommunicationLinkedListViewModel, AuthReply>(
                 authReply);
        }

        public Task OnDevelopment()
        {
            return new Task(() =>
            {
                var msg = AppRes.under_development;
                PageDialog.Toast(msg, TimeSpan.FromSeconds(3));
            });

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

                Title = "View " + name;
            }
            else
            {
                Title = "Edit " + name;
            }

        }
        public bool IsReading
        {
            get => isReading;
            set => SetField(ref isReading, value);
        }
        public bool IsEditing
        {
            get => isEditing;
            set => SetField(ref isEditing, value);
        }
        public string DomainSelected
        {
            get
            {
                if (authReply != null)
                    return CrossSettings.Current.GetValueOrDefault("ProjectName_" + authReply.d.loginName, AppRes.production);
                else
                    return CrossSettings.Current.GetValueOrDefault("ProjectName_", AppRes.production);

            }
            set
            {
                if (authReply != null)
                    CrossSettings.Current.AddOrUpdateValue("ProjectName_" + authReply.d.loginName, value);
                else
                    CrossSettings.Current.AddOrUpdateValue("ProjectName_", value);


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
                    Title = AppRes.edit + " " + name;
                    break;
                case PresentationMode.Create:
                    Title = AppRes.add_new + " " + PageTitle;
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
        public PresentationMode Mode
        {
            get => mode;
            set
            {
                if (SetProperty(ref mode, value))
                {
                    if (mode == PresentationMode.Create)
                    {
                        IsReading = false;
                        isEditing = true;
                    }
                    else if (mode == PresentationMode.Read)
                    {
                        IsReading = false;
                        isEditing = true;
                    }
                    else
                    {
                        IsReading = true;
                        isEditing = false;
                    }
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
                            inputValue.Value = "";
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

                            DateTime utc = _formContent.Value.SelectedDate.Value.Date.ToUniversalTime();
                            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                            TimeSpan diff = utc - origin;
                            inputValue.Value = string.Format("/Date({0})/", Math.Floor(diff.TotalMilliseconds));

                        }
                    }
                    else if (_formContent.Value.FormAndDropDownField.InputType == "ListBoxMulti")
                    {
                        List<string> selectedValues = new List<string>();

                        if (_formContent.Value.SelectedItem == null)
                        {
                            inputValue.Value = "";
                        }
                        else
                        {
                            selectedValues.Add(_formContent.Value.SelectedItem.PrimaryKey.ToString());
                        }

                        inputValue.Value = "[" + string.Join(",", selectedValues) + "]";

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
        public async Task<bool> ShowDeleteConfirmation() => await PageDialog.ConfirmAsync(AppRes.msg_delete_confirm,
                        AppRes.delete_event, AppRes.yes, AppRes.no);
        public async Task Add(HttpResponseMessage events)
        {
            AddEventsReply reply;

            if (events.IsSuccessStatusCode)
            {
                var response = await events.Content.ReadAsStringAsync();
                reply = await Task.Run(() => JsonConvert.DeserializeObject<AddEventsReply>(response));

                if (reply.d.successful == true)
                {
                    await PageDialog.AlertAsync(AppRes.saved_successfully, AppRes.saved, AppRes.ok);
                }
                else
                {
                    await PageDialog.AlertAsync(reply.d.message, AppRes.error_saving, AppRes.ok);

                }

            }
            else
                await PageDialog.AlertAsync(AppRes.msg_error_while_saving, AppRes.api_response_error, AppRes.ok);
        }
        public async Task PopulateControlsWithData(AuthReply authReply, int primaryKey, HttpResponseMessage resp)
        {
            try
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
                                        valObj.SelectedItem = field.DropdownValues.FirstOrDefault();

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
                    await PageDialog.AlertAsync(AppRes.msg_error_while_assigning_ui, AppRes.api_response_error, AppRes.ok);
            }
            catch (Exception ex)
            {
                await PageDialog.AlertAsync(ex.Message, AppRes.server_response_error + " " + AppRes.please_try_again, AppRes.ok);


            }
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
                        if (d.DropdownValues != null)
                        {
                            validatableObj.DropdownValues = new System.Collections.ObjectModel.ObservableCollection<Staketracker.Core.Models.FormAndDropDownField.DropdownValue>(d.DropdownValues);
                        }
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
                LinkedToConfig linkedToConfig = new LinkedToConfig();
                List<KeyValuePair<String, Staketracker.Core.Models.LinkedTo.LinkedTo>> linkedPage;
                if (PageTitle == "Event")
                    linkedPage = linkedToConfig.EventsPage;
                else if (PageTitle == "Communication")
                    linkedPage = linkedToConfig.CommunicationsPage;
                else if (PageTitle == "Stakeholder")
                    linkedPage = linkedToConfig.ProjectTeamPage;

                bool linkedToLabel = false;

                foreach (KeyValuePair<String, Staketracker.Core.Models.LinkedTo.LinkedTo> linked in linkedToConfig.EventsPage)
                {
                    ValidatableObject<string> validatableObj = new ValidatableObject<string>();
                    Staketracker.Core.Models.LinkedTo.LinkedTo linkedTo = linked.Value;

                    if (Mode == PresentationMode.Create || Mode == PresentationMode.Edit)
                    {
                        if (linkedTo.enableEditing == true)
                        {
                            AddLinkToControls(_formContent, ref linkedToLabel, linked, ref validatableObj);

                        }
                    }
                    else
                    {
                        if (linkedTo.enableEditing == false)
                        {
                            AddLinkToControls(_formContent, ref linkedToLabel, linked, ref validatableObj);


                        }
                    }



                }


                FormContent = _formContent;


            }
            else
            {
                await PageDialog.AlertAsync(AppRes.msg_err_getting_form_fields, AppRes.api_response_error, AppRes.ok);
                //  return null;
            }
        }

        private void AddLinkToControls(Dictionary<string, ValidatableObject<string>> _formContent, ref bool linkedToLabel, KeyValuePair<string, Models.LinkedTo.LinkedTo> linked, ref ValidatableObject<string> validatableObj)
        {
            AddLinkToLabel(_formContent, ref linkedToLabel, ref validatableObj);

            validatableObj = AddLinkToButton(_formContent, linked);
        }

        private static ValidatableObject<string> AddLinkToButton(Dictionary<string, ValidatableObject<string>> _formContent, KeyValuePair<string, Models.LinkedTo.LinkedTo> linked)
        {
            var validatableObj = new ValidatableObject<string>();
            validatableObj.LinkedControlType = "button";
            _formContent.Add(linked.Key.ToString(), validatableObj);
            return validatableObj;
        }

        private void AddLinkToLabel(Dictionary<string, ValidatableObject<string>> _formContent, ref bool linkedToLabel, ref ValidatableObject<string> validatableObj)
        {
            if (linkedToLabel == false)
            {
                validatableObj = new ValidatableObject<string>();
                validatableObj.PageTitle = "Link " + pageTitle + " to : ";
                validatableObj.LinkedControlType = "label";
                _formContent.Add(validatableObj.PageTitle, validatableObj);
                linkedToLabel = true;
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
        public async Task RunSafe(Task task, bool ShowLoading = true, string loadinMessage = null)
        {
            try
            {
                if (IsBusy)
                    return;

                IsBusy = true;

                if (ShowLoading)
                    UserDialogs.Instance.ShowLoading(loadinMessage ?? AppRes.loading);

                await task;
            }
            catch (Exception e)
            {
                IsBusy = false;
                //   UserDialogs.Instance.HideLoading();
                Debug.WriteLine(e.ToString());
                //await Application.Current.MainPage.DisplayAlert("Eror", "Check your internet connection", "Ok");
                await PageDialog.AlertAsync(AppRes.msg_check_internet_conn, AppRes.error, AppRes.ok);
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
