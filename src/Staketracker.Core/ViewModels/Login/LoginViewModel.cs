namespace Staketracker.Core.ViewModels.Login
{
    using MvvmCross.Navigation;
    using MvvmCross.ViewModels;
    using Newtonsoft.Json;
    using Plugin.Settings;
    using Staketracker.Core.Models;
    using Staketracker.Core.Res;
    using Staketracker.Core.Validators;
    using Staketracker.Core.Validators.Rules;
    using Staketracker.Core.ViewModels.ForgetPassword;
    using Staketracker.Core.ViewModels.ForgetUserId;
    using Staketracker.Core.ViewModels.Root;
    using Staketracker.Core.ViewModels.TwoStepVerification;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Xamarin.Forms;
    public class City
    {
        public string Name { get; set; }
        public int Population { get; set; }
    }
    public class LoginViewModel : BaseViewModel
    {
        private string sandboxTitle = AppRes.sandbox;
        private string productionTitle = AppRes.production;
        internal readonly IMvxNavigationService _navigationService;


        public ValidatableObject<string> Username { get; set; } = new ValidatableObject<string>();
        public ValidatableObject<string> Password { get; set; } = new ValidatableObject<string>();

        private bool? isSandboxChecked;

        public bool? IsSandboxChecked
        {
            get { return this.isSandboxChecked; }
            set
            {

                if (this.isSandboxChecked != value)
                {
                    SetField(ref isSandboxChecked, value);

                }

                if (this.isSandboxChecked == true)
                {
                    SelectedEnviroment = sandboxTitle;
                }
                else
                {
                    SelectedEnviroment = productionTitle;
                }


            }
        }
        public LoginAPIBody loginApiBody { get; set; }
        public String selectedEnviroment;
        public String SelectedEnviroment
        {
            get { return selectedEnviroment; }
            set
            {
                if (this.selectedEnviroment != value)
                {
                    SetField(ref selectedEnviroment, value);

                }
            }
        }
        public ICommand AuthenticateUserCommand { get; set; }
        public ICommand OnDevelopmentCommand { get; set; }
        public ICommand ForgetUserIdCommand { get; set; }
        public ICommand ForgetPasswordCommand { get; set; }


        public AuthReply authReply { get; set; }

        public LoginViewModel(IMvxNavigationService navigationService)
        {
            AddValidationRules();

            authReply = new AuthReply();
            _navigationService = navigationService;
            if (isDevelopmentMode)
            {
                Username.Value = "ALEM";
                Password.Value = "Sustainet0";
            }

            IsSandboxChecked = false;

            AuthenticateUserCommand = new Command(async () => await RunSafe(AuthenticateUser(loginApiBody), true, AppRes.signing_in));
            OnDevelopmentCommand = new Command(() =>
            {
                OnDevelopment().Start();
            });

            ForgetPasswordCommand = new Command(OpenForgetPassword);
            ForgetUserIdCommand = new Command(OpenForgetUserId);

        }

        private async void OpenForgetUserId()
        {

            await _navigationService.Navigate<ForgetUserIdViewModel, PresentationContext<string>>(
                   new PresentationContext<string>(Username.Value.ToString(), Models.PresentationMode.Create));

        }
        private async void OpenForgetPassword()
        {

            await _navigationService.Navigate<ForgetPasswordViewModel, PresentationContext<string>>(
                  new PresentationContext<string>(Username.Value.ToString(), Models.PresentationMode.Create));


        }


        public void AddValidationRules()
        {
            Username.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = AppRes.user_id_required });
            Password.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = AppRes.password_required });
        }

        private bool AreFieldsValid()
        {
            bool isFirstNameValid = Username.Validate();
            bool isPasswordValid = Password.Validate();
            return isFirstNameValid && isPasswordValid;
        }

        internal async Task AuthenticateUser(LoginAPIBody loginApiBody)
        {
            if (AreFieldsValid())
            {
                loginApiBody = new LoginAPIBody(Username.Value, Password.Value);

                var authResponse = await ApiManager.AuthenticateUser(loginApiBody);


                if (authResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {

                    var response = await authResponse.Content.ReadAsStringAsync();
                    authReply = await Task.Run(() => JsonConvert.DeserializeObject<AuthReply>(response));
                    if (authReply.d.sessionId == null)
                    {
                        await PageDialog.AlertAsync(AppRes.incorrect_username_or_password, AppRes.validation_error, AppRes.ok);
                        return;
                    }

                    bool Is2FEnabled = await GetIs2FEnabled(loginApiBody);
                    Is2FEnabled = false;

                    CrossSettings.Current.AddOrUpdateValue("userId", authReply.d.userId);
                    CrossSettings.Current.AddOrUpdateValue("sessionId", authReply.d.sessionId);


                    if (Is2FEnabled)
                    {
                        var dic = new Dictionary<string, string> { { "jsonText", "loginApiBody.jsonText" } };
                        MvxBundle bundle = new MvxBundle(dic);
                        authReply.loginAPIBody = loginApiBody;
                        await this._navigationService.Navigate<TwoStepVerificationViewModel, AuthReply>(authReply);
                    }
                    else
                    {


                        await _navigationService.Navigate<Rvm, AuthReply>(authReply);

                    }


                    //   String msg = "Logged in successfully, SessionId-" + authReply.d.sessionId;
                    // PageDialog.Toast(msg, TimeSpan.FromSeconds(5));
                }
                else if (authResponse.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    await PageDialog.AlertAsync(AppRes.incorrect_username_or_password, AppRes.validation_error, AppRes.ok);
                }
                else if (authResponse.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    throw new Exception("Connection Problem");
                }
            }
        }

        private Task OnDevelopment()
        {
            return new Task(() =>
            {
                var msg = AppRes.under_development;
                PageDialog.Toast(msg, TimeSpan.FromSeconds(3));
            });

        }

        internal async Task<bool> GetIs2FEnabled(LoginAPIBody loginApiBody)
        {
            Is2FEnabledResponse is2FEnabledResponse;
            HttpResponseMessage _Is2FEnabled = await ApiManager.Is2FEnabled(loginApiBody);

            if (_Is2FEnabled.IsSuccessStatusCode)
            {
                try
                {
                    var response = await _Is2FEnabled.Content.ReadAsStringAsync();
                    is2FEnabledResponse = await Task.Run(() => JsonConvert.DeserializeObject<Is2FEnabledResponse>(response));

                    if (is2FEnabledResponse.d == "True")
                    { return true; }
                    else
                    { return false; }
                }
            else
                {
                    await PageDialog.AlertAsync(AppRes.is2_enabled_error, AppRes.api_response_error, AppRes.ok);
                    return false;
                }
            }


        }
    }
