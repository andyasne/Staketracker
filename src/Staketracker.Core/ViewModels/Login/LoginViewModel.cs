namespace Staketracker.Core.ViewModels.Login
{
    using System;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using MvvmCross.Navigation;
    using Newtonsoft.Json;
    using Staketracker.Core.Models;
    using Staketracker.Core.ViewModels.Contacts;
    using Xamarin.Forms;

    public class LoginViewModel : BaseViewModel
    {
        internal readonly IMvxNavigationService _navigationService;


        public LoginAPIBody loginApiBody { get; set; }

        public JsonText jsonText { get; set; }

        public String username { get; set; }

        public String password { get; set; }

        public ICommand GetDataCommand { get; set; }

        public ICommand AuthenticateUserCommand { get; set; }

        public ICommand GetTimeLineDataCommand { get; set; }

        public AuthReply authReply { get; set; }

        public LoginViewModel(IMvxNavigationService navigationService)
        {
            authReply = new AuthReply();
            _navigationService = navigationService;
            username = "Alem";
            password = "Biniye@99";

            // loginApiBody.jsonText.username = "alem";
            //loginApiBody.jsonText.password = "Biniye@99";
            //      GetDataCommand = new Command(async () => await RunSafe(GetData()));
            // GetTimeLineDataCommand = new Command(async () => await RunSafe(GetTimeLine()));
            AuthenticateUserCommand = new Command(async () => await RunSafe(AuthenticateUser(loginApiBody)));
        }


        internal async Task AuthenticateUser(LoginAPIBody loginApiBody)
        {
            loginApiBody = new LoginAPIBody(username, password);

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

                await _navigationService.Navigate<ContactsViewModel>();


            }
            else
            {
                //await PageDialog.AlertAsync(makeUpsResponse.ReasonPhrase, "Error", "Ok");
                await PageDialog.AlertAsync("Incorrect Username or Password", "Validation Error", "Ok");
            }
        }
    }
}
