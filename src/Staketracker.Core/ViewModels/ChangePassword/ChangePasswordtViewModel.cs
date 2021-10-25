namespace Staketracker.Core.ViewModels.ChangePassword
{
    using MvvmCross.Commands;
    using MvvmCross.Navigation;
    using MvvmCross.ViewModels;
    using Newtonsoft.Json;
    using Staketracker.Core.Models;
    using Staketracker.Core.Models.AddEventsReply;
    using Staketracker.Core.Models.ApiRequestBody;
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

    public class ChangePasswordViewModel : BaseViewModel
    {
        private string email;
        private AuthReply authReply;
        internal readonly IMvxNavigationService _navigationService;


        public ValidatableObject<string> Email { get; set; } = new ValidatableObject<string>();


        public ICommand SubmitForgetPasswordCommand { get; set; }
        public ChangePasswordBody ChangePasswordBodyModel { get; set; }

        public ChangePasswordViewModel(IMvxNavigationService navigationService)
        {
            AddValidationRules();
            authReply = new AuthReply();
            _navigationService = navigationService;
            SubmitForgetPasswordCommand = new MvxAsyncCommand(OnSubmitForgetPasswordCommand);

        }

        private async Task OnSubmitForgetPasswordCommand()
        {
            ChangePassword(ChangePasswordBodyModel);
        }


        public void AddValidationRules()
        {
            Email.Validations.Add(new IsValidEmailRule<string> { ValidationMessage = "Enter Valid Email Address" });
        }


        internal async Task ChangePassword(ChangePasswordBody changePasswordBody)
        {


            AddEventsReply responseReply;
            HttpResponseMessage changePasswordRespMessage = await ApiManager.ChangePassword(changePasswordBody, authReply.d.sessionId);

            if (changePasswordRespMessage.IsSuccessStatusCode)
            {
                var response = await changePasswordRespMessage.Content.ReadAsStringAsync();
                responseReply = await Task.Run(() => JsonConvert.DeserializeObject<AddEventsReply>(response));

                if (responseReply.d.successful == true)
                {
                    //TODO: navigate back
                    await PageDialog.AlertAsync("Password Changed Successfully", "Password Changed", "Ok");
                }
                else
                {
                    await PageDialog.AlertAsync(responseReply.d.message, "Error Saving Communication", "Ok");

                }

            }
            else
                await PageDialog.AlertAsync("API Error While Trying to change password", "API Response Error", "Ok");

        }


    }
}
