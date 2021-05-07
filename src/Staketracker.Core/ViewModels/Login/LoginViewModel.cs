namespace Staketracker.Core.ViewModels.Login
{
    using System;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using MvvmCross.Navigation;
    using Newtonsoft.Json;
    using Staketracker.Core.Models;
    using Staketracker.Core.Validators;
    using Staketracker.Core.Validators.Rules;
    using Staketracker.Core.ViewModels.TwoStepVerification;
    using Staketracker.Core.ViewModels.Dashboard;
    using Xamarin.Forms;
    using System.Net.Http;

    public class LoginViewModel : BaseViewModel
    {
        internal readonly IMvxNavigationService _navigationService;

        public LoginAPIBody loginApiBody { get; set; }

        public ValidatableObject<string> Username { get; set; } = new ValidatableObject<string>();
        public ValidatableObject<string> Password { get; set; } = new ValidatableObject<string>();

        public ICommand AuthenticateUserCommand { get; set; }

        public AuthReply authReply { get; set; }

        public LoginViewModel(IMvxNavigationService navigationService)
        {
            AddValidationRules();

            authReply = new AuthReply();
            _navigationService = navigationService;

            Username.Value = "Alem";
            Password.Value = "Biniye@99";

            AuthenticateUserCommand = new Command(async () => await RunSafe(AuthenticateUser(loginApiBody)));
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

                if (authResponse.IsSuccessStatusCode)
                {
                    var response = await authResponse.Content.ReadAsStringAsync();
                    authReply = await Task.Run(() => JsonConvert.DeserializeObject<AuthReply>(response));
                    if (authReply.d.sessionId == null)
                    {
                        await PageDialog.AlertAsync("Incorrect Username or Password", "Validation Error", "Ok");
                        return;
                    }
                    String msg = "Logged in successfully, SessionId-" + authReply.d.sessionId;
                    PageDialog.Toast(msg, TimeSpan.FromSeconds(5));
                    //await PageDialog.AlertAsync("Logged in successfully,     SessionId-" + authReply.d.sessionId, "Login", "Ok");

                    bool Is2FEnabled = await GetIs2FEnabled(loginApiBody);//Consume the 'Is2FEnabled'(returns "True" or "False") API right after AuthenticateUsr.
                    if (Is2FEnabled)
                    { await _navigationService.Navigate<TwoStepVerificationViewModel>(); }
                    else
                    {
                        await _navigationService.Navigate<DashboardViewModel>();
                    }
                }
                else
                {
                    //await PageDialog.AlertAsync(authResponse.ReasonPhrase, "Error", "Ok");
                    await PageDialog.AlertAsync("Incorrect Username or Password", "Validation Error", "Ok");
                }
            }
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
                {
                    return true;
                }
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
