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
    using Staketracker.Core.ViewModels.Contacts;
    using Staketracker.Core.ViewModels.TwoStepVerification;
    using Xamarin.Forms;

    public class LoginViewModel : BaseViewModel
    {
        internal readonly IMvxNavigationService _navigationService;

        public LoginAPIBody loginApiBody { get; set; }

        public JsonText jsonText { get; set; }

        public ValidatableObject<string> username { get; set; } = new ValidatableObject<string>();

        public String password { get; set; }

        public ICommand GetDataCommand { get; set; }

        public ICommand AuthenticateUserCommand { get; set; }

        public ICommand GetTimeLineDataCommand { get; set; }

        public AuthReply authReply { get; set; }

        public LoginViewModel(IMvxNavigationService navigationService)
        {
            AddValidationRules();

            authReply = new AuthReply();
            _navigationService = navigationService;
            username.Value = "Alem";
            password = "Biniye@99";

            AuthenticateUserCommand = new Command(async () => await RunSafe(AuthenticateUser(loginApiBody)));
        }

        public void AddValidationRules()
        {
            username.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "User Id is Required" });
        }

        bool AreFieldsValid()
        {
            bool isFirstNameValid = username.Validate();
            return isFirstNameValid;

        }

        internal async Task AuthenticateUser(LoginAPIBody loginApiBody)
        {
            if (AreFieldsValid())
            {
                loginApiBody = new LoginAPIBody(username.Value, password);

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
