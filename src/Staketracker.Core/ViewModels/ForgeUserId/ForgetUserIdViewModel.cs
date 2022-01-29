namespace Staketracker.Core.ViewModels.ForgetUserId
{
    using MvvmCross.Navigation;
    using MvvmCross.ViewModels;
    using Newtonsoft.Json;
    using Staketracker.Core.Models;
    using Staketracker.Core.Models.EventsFormValue;
    using Staketracker.Core.Res;
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

    public class ForgetUserIdViewModel : BaseViewModel<PresentationContext<string>>
    {
        private string email;
        private AuthReply authReply;
        internal readonly IMvxNavigationService _navigationService;


        public ValidatableObject<string> Email { get; set; } = new ValidatableObject<string>();


        public ICommand SubmitForgetUserIdCommand { get; set; }

        public ForgetUserIdViewModel(IMvxNavigationService navigationService)
        {
            AddValidationRules();
            authReply = new AuthReply();
            _navigationService = navigationService;

            SubmitForgetUserIdCommand = new Command(async () => await RunSafe(SubmitForgetUserId(), true, "Requesting User Id"));

        }

        internal async Task SubmitForgetUserId()
        {
            if (AreFieldsValid())
            {
                RequestUsrorPwdModel requestUsrorPwdModel = new RequestUsrorPwdModel();
                requestUsrorPwdModel.username = Email.Value;
                requestUsrorPwdModel.password = "";

                jsonTextObj _jsonTextObj = new jsonTextObj(requestUsrorPwdModel);
                HttpResponseMessage respMsg = await ApiManager.RequestUsr(_jsonTextObj, "");

                UsrEmailResponse reply;

                if (respMsg.IsSuccessStatusCode)
                {
                    var response = await respMsg.Content.ReadAsStringAsync();
                    reply = await Task.Run(() => JsonConvert.DeserializeObject<UsrEmailResponse>(response));
                    if (reply.d.Equals("Email sent"))
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
        }

        private bool AreFieldsValid()
        {
            return Email.Validate();
        }


        private string username;
        public override void Prepare(PresentationContext<string> usernameContext)
        {
            this.username = usernameContext.Model;

        }
    }
}
