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
    using Xamarin.Forms;

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

                var makeUpsResponse = await ApiManager.AuthenticateUser(loginApiBody);

                if (makeUpsResponse.IsSuccessStatusCode)
                {
                    var response = await makeUpsResponse.Content.ReadAsStringAsync();
                    authReply = await Task.Run(() => JsonConvert.DeserializeObject<AuthReply>(response));
                    if (authReply.d.sessionId == null)
                    {
                        await PageDialog.AlertAsync("Incorrect Username or Password", "Validation Error", "Ok");
                        return;
                    }
                    String msg = "Logged in successfully, SessionId-" + authReply.d.sessionId;
                    PageDialog.Toast(msg, TimeSpan.FromSeconds(5));
                    //await PageDialog.AlertAsync("Logged in successfully,     SessionId-" + authReply.d.sessionId, "Login", "Ok");

                    bool Is2FEnabled = GetIs2FEnabled();//Consume the 'Is2FEnabled'(returns "True" or "False") API right after AuthenticateUsr.
                    if (Is2FEnabled)
                        await _navigationService.Navigate<TwoStepVerificationViewModel>();
                }
                else
                {
                    //await PageDialog.AlertAsync(makeUpsResponse.ReasonPhrase, "Error", "Ok");
                    await PageDialog.AlertAsync("Incorrect Username or Password", "Validation Error", "Ok");
                }
            }
        }

        private static bool GetIs2FEnabled()
        {
            //TOD : call the api and retrieve this val
            return true;
        }
    }
}
