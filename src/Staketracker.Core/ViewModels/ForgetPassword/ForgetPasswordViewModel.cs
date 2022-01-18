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

    public class ForgetPasswordViewModel : BaseViewModel
    {
        private string email;
        private AuthReply authReply;
        internal readonly IMvxNavigationService _navigationService;


        public ValidatableObject<string> Email { get; set; } = new ValidatableObject<string>();


        public ICommand SubmitForgetPasswordCommand { get; set; }

        public ForgetPasswordViewModel(IMvxNavigationService navigationService)
        {
            AddValidationRules();
            authReply = new AuthReply();
            _navigationService = navigationService;
            SubmitForgetPasswordCommand = new Command(SubmitForgetUserId);

        }

        private async void SubmitForgetUserId()
        {
            RequestUsrorPwdModel requestUsrorPwdModel = new RequestUsrorPwdModel();
            requestUsrorPwdModel.username = authReply.d.loginName;
            requestUsrorPwdModel.password = Email.Value;

            jsonTextObj _jsonTextObj = new jsonTextObj(requestUsrorPwdModel);
            HttpResponseMessage respMsg = await ApiManager.RequestPwd(_jsonTextObj, authReply.d.sessionId);

            UsrEmailResponse reply;

            if (respMsg.IsSuccessStatusCode)
            {
                var response = await respMsg.Content.ReadAsStringAsync();
                reply = await Task.Run(() => JsonConvert.DeserializeObject<UsrEmailResponse>(response));
                if (reply.d.Equals("Success"))
                {
                    await PageDialog.AlertAsync(AppRes.record_deleted_msg, AppRes.record_deleted, AppRes.ok);
                }
                else
                {
                    await PageDialog.AlertAsync(reply.d.ToString(), AppRes.error, AppRes.ok);
                }

            }
            else
                await PageDialog.AlertAsync(AppRes.server_error_while_delete_msg, AppRes.api_response_error, AppRes.ok);


        }
        public void AddValidationRules()
        {
            Email.Validations.Add(new IsValidEmailRule<string> { ValidationMessage = "Enter Valid Email Address" });
        }

        private bool AreFieldsValid()
        {
            return Email.Validate();
        }

        internal async Task AuthenticateUser(LoginAPIBody loginApiBody)
        {
            if (AreFieldsValid())
            {


            }
        }



    }
}
