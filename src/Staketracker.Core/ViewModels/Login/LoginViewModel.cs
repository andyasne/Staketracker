namespace Staketracker.Core.ViewModels.Login
{
    using MvvmCross.Navigation;
    using MvvmCross.ViewModels;
    using Newtonsoft.Json;
    using Staketracker.Core.Models;
    using Staketracker.Core.Validators;
    using Staketracker.Core.Validators.Rules;
    using Staketracker.Core.ViewModels.Root;
    using Staketracker.Core.ViewModels.TwoStepVerification;
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Xamarin.Forms;

    public class LoginViewModel : BaseViewModel
    {
        private const string sandboxTitle = "Sandbox";
        private const string productionTitle = "Production";
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
                    IsSandboxDisplay = sandboxTitle;
                else
                    IsSandboxDisplay = productionTitle;


            }
        }

        private string? isSandboxDisplay;

        public string? IsSandboxDisplay
        {
            get { return this.isSandboxDisplay; }
            set
            {

                if (this.isSandboxDisplay != value)
                {
                    SetField(ref isSandboxDisplay, value);
                    this.isSandboxDisplay = value;
                }

            }
        }
        public LoginAPIBody loginApiBody { get; set; }
        public ICommand AuthenticateUserCommand { get; set; }
        public ICommand OnDevelopmentCommand { get; set; }

        public AuthReply authReply { get; set; }

        public LoginViewModel(IMvxNavigationService navigationService)
        {
            AddValidationRules();

            authReply = new AuthReply();
            _navigationService = navigationService;
            if (isDevelopmentMode)
            {
                Username.Value = "alem";
                Password.Value = "Biniye@98";
            }

            IsSandboxChecked = false;

            AuthenticateUserCommand = new Command(async () => await RunSafe(AuthenticateUser(loginApiBody), true, "Signing In"));
            OnDevelopmentCommand = new Command(() =>
           {
               OnDevelopment().Start();
           });
        }


        public void AddValidationRules()
        {
            Username.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "User Id is Required" });
            Password.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Password Required" });
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
                        await PageDialog.AlertAsync("Incorrect Username or Password", "Validation Error", "Ok");
                        return;
                    }

                    bool Is2FEnabled = await GetIs2FEnabled(loginApiBody);
                    if (Is2FEnabled)
                    {
                        var dic = new Dictionary<string, string> { { "jsonText", "loginApiBody.jsonText" } };
                        MvxBundle bundle = new MvxBundle(dic);
                        authReply.loginAPIBody = loginApiBody;

                        await this._navigationService.Navigate<TwoStepVerificationViewModel, AuthReply>(authReply);
                    }
                    else
                    {
                        await _navigationService.Navigate<RootViewModel, AuthReply>(authReply);
                    }


                    //   String msg = "Logged in successfully, SessionId-" + authReply.d.sessionId;
                    // PageDialog.Toast(msg, TimeSpan.FromSeconds(5));
                }
                else if (authResponse.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    await PageDialog.AlertAsync("Incorrect Username or Password", "Validation Error", "Ok");
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
                var msg = "This Page is Under Development";
                PageDialog.Toast(msg, TimeSpan.FromSeconds(3));
            });

        }

        internal async Task<bool> GetIs2FEnabled(LoginAPIBody loginApiBody)
        {
            Is2FEnabledResponse is2FEnabledResponse;
            HttpResponseMessage _Is2FEnabled = await ApiManager.Is2FEnabled(loginApiBody);

            if (_Is2FEnabled.IsSuccessStatusCode)
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
                await PageDialog.AlertAsync("API Error While retrieving IS2 Enabled State for the user", "API Response Error", "Ok");
                return false;
            }
        }
    }
}
