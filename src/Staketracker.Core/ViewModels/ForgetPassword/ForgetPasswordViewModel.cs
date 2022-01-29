namespace Staketracker.Core.ViewModels.ForgetPassword
{
    using MvvmCross.Navigation;
    using Newtonsoft.Json;
    using Staketracker.Core.Models;
    using Staketracker.Core.Models.EventsFormValue;
    using Staketracker.Core.Res;
    using Staketracker.Core.Validators;
    using Staketracker.Core.Validators.Rules;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Xamarin.Forms;

    public class ForgetPasswordViewModel : BaseViewModel<PresentationContext<string>>
    {
        private string email;
        private AuthReply authReply;
        internal readonly IMvxNavigationService _navigationService;


        public ValidatableObject<string> Email { get; set; } = new ValidatableObject<string>();

        public ValidatableObject<string> Username { get; set; } = new ValidatableObject<string>();

        public ICommand SubmitForgetPasswordCommand { get; set; }

        public ForgetPasswordViewModel(IMvxNavigationService navigationService)
        {
            AddValidationRules();
            authReply = new AuthReply();
            _navigationService = navigationService;
            SubmitForgetPasswordCommand = new Command(async () => await RunSafe(SubmitForgetPassword(), true, "Requesting New Password"));


        }

        async Task SubmitForgetPassword()
        {
            if (AreFieldsValid())
            {
                UsrEmailResponse reply;
                RequestUsrorPwdModel requestUsrorPwdModel = new RequestUsrorPwdModel();
                requestUsrorPwdModel.username = Username.Value;
                requestUsrorPwdModel.password = Email.Value;

                jsonTextObj _jsonTextObj = new jsonTextObj(requestUsrorPwdModel);
                HttpResponseMessage respMsg = await ApiManager.RequestPwd(_jsonTextObj, "");

                if (respMsg.IsSuccessStatusCode)
                {
                    var response = await respMsg.Content.ReadAsStringAsync();
                    reply = await Task.Run(() => JsonConvert.DeserializeObject<UsrEmailResponse>(response));
                    if (reply.d.Equals("Success"))
                    {


                        await PageDialog.AlertAsync(AppRes.msg_email_sent_success, AppRes.email_sent, AppRes.ok);
                    }
                    else
                    {
                        await PageDialog.AlertAsync(reply.d.ToString(), AppRes.error, AppRes.ok);
                    }

                }
                else
                    await PageDialog.AlertAsync(AppRes.server_response_error, AppRes.api_response_error, AppRes.ok);

            }
        }
        public void AddValidationRules()
        {
            Email.Validations.Add(new IsValidEmailRule<string> { ValidationMessage = "Enter Valid Email Address" });
            Username.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = AppRes.user_id_required });

        }

        private bool AreFieldsValid()
        {
            bool isFirstNameValid = Username.Validate();
            bool isEmailValid = Email.Validate();
            return isFirstNameValid && isEmailValid;
        }



        private string username;
        public override void Prepare(PresentationContext<string> usernameContext)
        {
            this.username = usernameContext.Model;

        }
    }
}
